//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using nanoFramework.TestAdapter;
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
    [ExtensionUri(TestsConstants.NanoExecutor)]
    class Executor : ITestExecutor
    {
        private const string TestPassed = "Test passed: ";
        private const string TestFailed = "Test failed: ";
        private const string Exiting = "Exiting.";
        private const string Done = "Done.";
        private bool _cancel = false;
        private Settings _settings;
        private LogMessenger _logger;

        private IFrameworkHandle _frameworkHandle = null;

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

             _logger = new LogMessenger(frameworkHandle, settingsProvider);

            if (settingsProvider != null)
            {
                _settings = settingsProvider.Settings;

                _logger.LogMessage(
                    "Getting ready to run tests...",
                    Settings.LoggingLevel.Detailed);

                _logger.LogMessage(
                    "Settings parsed",
                    Settings.LoggingLevel.Verbose);
            }
            else
            {
                _logger.LogMessage(
                    "Getting ready to run tests...",
                    Settings.LoggingLevel.Detailed);

                _logger.LogMessage(
                    "No settings for nanoFramework adapter",
                    Settings.LoggingLevel.Verbose);
            }

            var uniqueSources = tests.Select(m => m.Source).Distinct();

            _logger.LogMessage(
                "Test sources enumerated",
                Settings.LoggingLevel.Verbose);

            foreach (var source in uniqueSources)
            {
                var groups = tests.Where(m => m.Source == source);

                _logger.LogMessage(
                    $"Test group is '{source}'",
                    Settings.LoggingLevel.Detailed);

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
                _logger.LogMessage(
                    $"Finding test cases for '{source}'...",
                    Settings.LoggingLevel.Detailed);

                var testsCases = TestDiscoverer.FindTestCases(source);

                RunTests(testsCases, runContext, frameworkHandle);
            }
        }

        private List<TestResult> RunTest(List<TestCase> tests)
        {
            _logger.LogMessage(
                "Setting up test runner...",
                Settings.LoggingLevel.Detailed);

            int runTimeout = 10000;
            if (_settings != null)
            {
                runTimeout = _settings.TestTimeOutSeconds * 1000;
            }

            _logger.LogMessage(
                $"Timeout set to {runTimeout}ms",
                Settings.LoggingLevel.Verbose);

            bool isStillRunning = true;

            List<TestResult> results = new List<TestResult>();
            
            foreach (var test in tests)
            {
                TestResult result = new TestResult(test) { Outcome = TestOutcome.None };
                results.Add(result);
            }

            _logger.LogMessage(
                "Processing assemblies to load into test runner...",
                Settings.LoggingLevel.Verbose);

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
                // prepare parameters to load nanoCLR, include:
                // 1. mscorlib
                // 2. test application
                // 3. unit test launcher
                string parameter = $"-load {nfTestAppLocation} -load {mscorlibLocation} -load {nfTestClassLibLocation} -load {nfTestOfTestClassLibLocation}";

                _logger.LogMessage(
                    "Launching process with nanoCLR...",
                    Settings.LoggingLevel.Verbose);

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
                    results.First().ErrorMessage = "Failed to start nanoCLR";

                    _logger.LogPanicMessage(
                        "Failed to start nanoCLR!");
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

                _logger.LogMessage(
                    $"nanoCLR started @ process ID: {nanoClr.Id}",
                    Settings.LoggingLevel.Detailed);


                // wait for exit, no worries about the outcome
                nanoClr.WaitForExit(runTimeout);

                var outputStrings = Regex.Split(output.ToString(), @"((\r)+)?(\n)+((\r)+)?").Where(m => !string.IsNullOrEmpty(m));

                _logger.LogMessage(
                    "Parsing test results...",
                    Settings.LoggingLevel.Verbose);

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
                _logger.LogMessage(
                    $"Fatal exception when processing test results: >>>{ex.Message}\r\n{output}\r\n{error}",
                    Settings.LoggingLevel.Detailed);

                results.First().Outcome = TestOutcome.Failed;
                results.First().ErrorMessage = $"Fatal exception when processing test results. Set logging to 'Detailed' for details.";
            }
            finally
            {
                if (!nanoClr.HasExited)
                {
                    _logger.LogMessage(
                        "Attempting to kill nanoCLR process...",
                        Settings.LoggingLevel.Verbose);

                    nanoClr.Kill();
                    nanoClr.WaitForExit(runTimeout);
                }
            }

            return results;
        }
    }
}
