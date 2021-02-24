//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace nanoFramework.TestPlatform.TestAdapter
{
    /// <summary>
    /// An Executor class
    /// </summary>
    [ExtensionUri(TestsConstants.Executornano)]
    class Executor : ITestExecutor
    {
        private const string TestPassed = "Test passed: ";
        private const string TestFailed = "Test failed: ";
        private const string Exiting = "Exiting.";
        private const string Done = "Done.";
        private bool _cancel = false;
        private Settings _settings;

        /// <inheritdoc/>
        public void Cancel()
        {
            _cancel = true;
            return;
        }

        /// <inheritdoc/>
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var settingsProvider = runContext.RunSettings.GetSettings(TestsConstants.SettingsName) as SettingsProvider;
            _settings = settingsProvider.Settings;
            var uniqueSources = tests.Select(m => m.Source).Distinct();
            foreach (var source in uniqueSources)
            {
                var groups = tests.Where(m => m.Source == source);
                var results = RunTest(groups.ToList());
                foreach (var result in results)
                {
                    frameworkHandle.RecordResult(result);
                }
            }
        }

        /// <inheritdoc/>
        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            foreach (var source in sources)
            {
                var testsCases = TestDiscoverer.FindTestCases(source);
                RunTests(testsCases, runContext, frameworkHandle);
            }
        }

        private List<TestResult> RunTest(List<TestCase> tests)
        {
            int runTimeout = _settings.TestTimeOutSeconds * 1000;
            bool isStillRunning = true;
            List<TestResult> results = new List<TestResult>();
            foreach (var test in tests)
            {
                TestResult result = new TestResult(test) { Outcome = TestOutcome.None };
                results.Add(result);
            }

            var source = tests.First().Source;
            var nfTestAppLocation = source.Replace(Path.GetFileName(source), "nanoFramework.UnitTestLauncher.pe"); // TestObjectHelper.GetTestPath("UnitTestLauncher", "pe");
            var workingDirectory = Path.GetDirectoryName(nfTestAppLocation);
            var mscorlibLocation = source.Replace(Path.GetFileName(source), "mscorlib.pe");  //nfTestAppLocation.Replace("UnitTestLauncher.pe", "mscorlib.pe");
            var nfTestClassLibLocation = source.Replace(Path.GetFileName(source), "nanoFramework.TestFramework.pe"); // TestObjectHelper.GetTestPath("nanoFramework.TestFramework", "pe");
            var nfTestOfTestClassLibLocation = source.Replace(".dll", ".pe"); //TestObjectHelper.GetTestPath(Path.GetFileNameWithoutExtension(tests.First().Source), "pe");

            // prepare the process start of the WIN32 nanoCLR
            Process nanoClr = new Process();

            AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
            AutoResetEvent errorWaitHandle = new AutoResetEvent(false);
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            try
            {
                // load only mscorlib
                string parameter = $"-load {nfTestAppLocation} -load {mscorlibLocation} -load {nfTestClassLibLocation} -load {nfTestOfTestClassLibLocation}";

                nanoClr.StartInfo = new ProcessStartInfo(TestObjectHelper.GetNanoClrLocation(), parameter)
                {
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                // launch nanoCLR
                if (!nanoClr.Start())
                {
                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = "Impossible to start nano CLR";
                }

                nanoClr.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };
                nanoClr.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                nanoClr.Start();

                nanoClr.BeginOutputReadLine();
                nanoClr.BeginErrorReadLine();

                Console.WriteLine($"nanoCLR started @ process ID: {nanoClr.Id}");


                // wait for exit, no worries about the outcome
                nanoClr.WaitForExit(runTimeout);

                var outputStrings = Regex.Split(output.ToString(), @"((\r)+)?(\n)+((\r)+)?").Where(m => !string.IsNullOrEmpty(m));

                foreach (var line in outputStrings)
                {
                    if (line.Contains(TestPassed))
                    {
                        // Format is "Test passed: MethodName, ticks";
                        // We do get split with space if the coma is missing, happens time to time
                        string method = line.Substring(line.IndexOf(TestPassed) + TestPassed.Length).Split(',')[0].Split(' ')[0];
                        string ticks = line.Substring(line.IndexOf(TestPassed) + TestPassed.Length + method.Length + 2);
                        long ticksNum = 0;
                        try
                        {
                            ticksNum = Convert.ToInt64(ticks);
                        }
                        catch (Exception)
                        {
                            // We won't do anything
                        }

                        // Find the test
                        var res = results.Where(m => m.TestCase.DisplayName == method);
                        if (res.Any())
                        {
                            res.First().Duration = TimeSpan.FromTicks(ticksNum);
                            res.First().Outcome = TestOutcome.Passed;
                        }
                    }
                    else if (line.Contains(TestFailed))
                    {
                        // Format is "Test passed: MethodName, Exception message";
                        string method = line.Substring(line.IndexOf(TestFailed) + TestFailed.Length).Split(',')[0].Split(' ')[0];
                        string exception = line.Substring(line.IndexOf(TestFailed) + TestPassed.Length + method.Length + 2);
                        // Find the test
                        var res = results.Where(m => m.TestCase.DisplayName == method);
                        if (res.Any())
                        {
                            res.First().ErrorMessage = exception;
                            res.First().Outcome = TestOutcome.Failed;
                        }
                    }
                }

                if (!output.ToString().Contains(Done))
                {
                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = output.ToString();
                }

                var notPassedOrFailed = results.Where(m => m.Outcome != TestOutcome.Failed && m.Outcome != TestOutcome.Passed);
                if (notPassedOrFailed.Any())
                {
                    notPassedOrFailed.First().ErrorMessage = output.ToString();
                }

            }
            catch (Exception ex)
            {
                results.First().Outcome = TestOutcome.Failed;
                results.First().ErrorMessage = $"{ex.Message}\r\n{output}\r\n{error}";
            }
            finally
            {
                if (!nanoClr.HasExited)
                {
                    nanoClr.Kill();
                    nanoClr.WaitForExit(runTimeout);
                }
            }

            return results;
        }
    }
}
