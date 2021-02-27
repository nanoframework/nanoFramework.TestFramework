//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using nanoFramework.TestAdapter;
using nanoFramework.Tools.Debugger;
using nanoFramework.Tools.Debugger.Extensions;
using nanoFramework.Tools.Debugger.WireProtocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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
        private Settings _settings;
        private LogMessenger _logger;
        private Process _nanoClr;

        // number of retries when performing a deploy operation
        private const int _numberOfRetries = 5;

        // timeout when performing a deploy operation
        private const int _timeoutMiliseconds = 1000;

        private IFrameworkHandle _frameworkHandle = null;

        /// <inheritdoc/>
        public void Cancel()
        {
            if (!_nanoClr.HasExited)
            {
                _logger.LogMessage(
                    "Canceling to test process. Attempting to kill nanoCLR process...",
                    Settings.LoggingLevel.Verbose);

                _nanoClr.Kill();
                // Wait 5 seconds maximum
                _nanoClr.WaitForExit(5000);
            }

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

                // 
                List<TestResult> results;

                if(_settings.IsRealHardware)
                {
                    // we are connecting to a real device
                    results = RunTestOnHardwareAsync(groups.ToList());
                }
                else
                {
                    // we are connecting to WIN32 nanoCLR
                    results = RunTest(groups.ToList());
                }

                foreach (var result in results)
                {
                    frameworkHandle.RecordResult(result);
                }
            }
        }

        private async System.Threading.Tasks.Task<List<TestResult>> RunTestOnHardwareAsync(List<TestCase> lists)
        {
            var serialDebugClient = PortBase.CreateInstanceForSerial("", new System.Collections.Generic.List<string>() { "COM16" });

            var device = serialDebugClient.NanoFrameworkDevices[0];

            // check if debugger engine exists
            if (device.DebugEngine == null)
            {
                device.CreateDebugEngine();
            }

            bool deviceIsInInitializeState = false;
            int retryCount = 0;

            bool connectResult = await device.DebugEngine.ConnectAsync(5000, true);

            if (connectResult)
            {
                // erase the device
                var eraseResult = await Task.Run(async delegate
                {
                    //MessageCentre.InternalErrorMessage("Erase deployment block storage.");

                    return await device.EraseAsync(
                        EraseOptions.Deployment,
                        CancellationToken.None,
                        null,
                        null);
                });

                if (eraseResult)
                {

                    // initial check 
                    if (device.DebugEngine.IsDeviceInInitializeState())
                    {
                        //MessageCentre.InternalErrorMessage("Device status verified as being in initialized state. Requesting to resume execution.");

                        // set flag
                        deviceIsInInitializeState = true;

                        // device is still in initialization state, try resume execution
                        device.DebugEngine.ResumeExecution();
                    }

                    // handle the workflow required to try resuming the execution on the device
                    // only required if device is not already there
                    // retry 5 times with a 500ms interval between retries
                    while (retryCount++ < _numberOfRetries && deviceIsInInitializeState)
                    {
                        if (!device.DebugEngine.IsDeviceInInitializeState())
                        {
                            //MessageCentre.InternalErrorMessage("Device has completed initialization.");

                            // done here
                            deviceIsInInitializeState = false;
                            break;
                        }

                        //MessageCentre.InternalErrorMessage($"Waiting for device to report initialization completed ({retryCount}/{_numberOfRetries}).");

                        // provide feedback to user on the 1st pass
                        if (retryCount == 0)
                        {
                            //await outputPaneWriter.WriteLineAsync(ResourceStrings.WaitingDeviceInitialization);
                        }

                        if (device.DebugEngine.IsConnectedTonanoBooter)
                        {
                            // MessageCentre.InternalErrorMessage("Device reported running nanoBooter. Requesting to load nanoCLR.");

                            // request nanoBooter to load CLR
                            device.DebugEngine.ExecuteMemory(0);
                        }
                        else if (device.DebugEngine.IsConnectedTonanoCLR)
                        {
                            //MessageCentre.InternalErrorMessage("Device reported running nanoCLR. Requesting to reboot nanoCLR.");

                            await Task.Run(delegate
                            {
                            // already running nanoCLR try rebooting the CLR
                            device.DebugEngine.RebootDevice(RebootOptions.ClrOnly);
                            });
                        }

                        // wait before next pass
                        // use a back-off strategy of increasing the wait time to accommodate slower or less responsive targets (such as networked ones)
                        await Task.Delay(TimeSpan.FromMilliseconds(_timeoutMiliseconds * (retryCount + 1)));

                        await Task.Yield();
                    }

                    // check if device is still in initialized state
                    if (!deviceIsInInitializeState)
                    {
                        // device has left initialization state
                        //await outputPaneWriter.WriteLineAsync(ResourceStrings.DeviceInitialized);

                        await Task.Yield();

                   

                        //////////////////////////////////////////////////////////
                        // sanity check for devices without native assemblies ?!?!
                        if (device.DeviceInfo.NativeAssemblies.Count == 0)
                        {
                           // MessageCentre.InternalErrorMessage("Device reporting no assemblies loaded. This can not happen. Sanity check failed.");

                            // there are no assemblies deployed?!
                            //throw new DeploymentException($"Couldn't find any native assemblies deployed in {_viewModelLocator.DeviceExplorer.SelectedDevice.Description}! If the situation persists reboot the device.");
                        }

                        //MessageCentre.InternalErrorMessage("Computing deployment blob.");

       

                        // build a list with the full path for each DLL, referenced DLL and EXE
                        List<DeploymentAssembly> assemblyList = new List<DeploymentAssembly>();


                        //var source = tests.First().Source;
                        //var nfUnitTestLauncherLocation = source.Replace(Path.GetFileName(source), "nanoFramework.UnitTestLauncher.pe");
                        //var workingDirectory = Path.GetDirectoryName(nfUnitTestLauncherLocation);

                        // load tests
                        assemblyList.Add(
                            new DeploymentAssembly(source, "", ""));

                        // TODO


                        //var mscorlibLocation = source.Replace(Path.GetFileName(source), "mscorlib.pe");
                        //var nfTestFrameworkLocation = source.Replace(Path.GetFileName(source), "nanoFramework.TestFramework.pe");
                        //var nfAssemblyUnderTestLocation = source.Replace(".dll", ".pe");

                        // TODO do we need to check vbersions?

                        //foreach (string assemblyPath in assemblyPathsToDeploy)
                        //{
                        //    // load assembly in order to get the versions
                        //    var decompiler = new CSharpDecompiler(assemblyPath, decompilerSettings);
                        //    var assemblyProperties = decompiler.DecompileModuleAndAssemblyAttributesToString();

                        //    // read attributes using a Regex

                        //    // AssemblyVersion
                        //    string pattern = @"(?<=AssemblyVersion\("")(.*)(?=\""\)])";
                        //    var match = Regex.Matches(assemblyProperties, pattern, RegexOptions.IgnoreCase);
                        //    string assemblyVersion = match[0].Value;

                        //    // AssemblyNativeVersion
                        //    pattern = @"(?<=AssemblyNativeVersion\("")(.*)(?=\""\)])";
                        //    match = Regex.Matches(assemblyProperties, pattern, RegexOptions.IgnoreCase);

                        //    // only class libs have this attribute, therefore sanity check is required
                        //    string nativeVersion = "";
                        //    if (match.Count == 1)
                        //    {
                        //        nativeVersion = match[0].Value;
                        //    }

                        //    assemblyList.Add(new DeploymentAssembly(assemblyPath, assemblyVersion, nativeVersion));
                        //}

                        //// if there are referenced project, the assembly list contains repeated assemblies so need to use Linq Distinct()
                        //// an IEqualityComparer is required implementing the proper comparison
                        //List<DeploymentAssembly> distinctAssemblyList = assemblyList.Distinct(new DeploymentAssemblyDistinctEquality()).ToList();

                        //// build a list with the PE files corresponding to each DLL and EXE
                        //List<DeploymentAssembly> peCollection = distinctAssemblyList.Select(a => new DeploymentAssembly(a.Path.Replace(".dll", ".pe").Replace(".exe", ".pe"), a.Version, a.NativeVersion)).ToList();

                        //// build a list with the PE files corresponding to a DLL for native support checking
                        //// only need to check libraries because EXEs don't have native counterpart
                        //List<DeploymentAssembly> peCollectionToCheck = distinctAssemblyList.Where(i => i.Path.EndsWith(".dll")).Select(a => new DeploymentAssembly(a.Path.Replace(".dll", ".pe"), a.Version, a.NativeVersion)).ToList();

                        //await Task.Yield();

                        //var checkAssembliesResult = await CheckNativeAssembliesAvailabilityAsync(device.DeviceInfo.NativeAssemblies, peCollectionToCheck);
                        //if (checkAssembliesResult != "")
                        //{
                        //    MessageCentre.InternalErrorMessage("Found assemblies mismatches when checking for deployment pre-check.");

                        //    // can't deploy
                        //    throw new DeploymentException(checkAssembliesResult);
                        //}

                        //await Task.Yield();

                        // Keep track of total assembly size
                        long totalSizeOfAssemblies = 0;

                        // TODO use this code to load the PE files

                        //// now we will re-deploy all system assemblies
                        //foreach (DeploymentAssembly peItem in peCollection)
                        //{
                        //    // append to the deploy blob the assembly
                        //    using (FileStream fs = File.Open(peItem.Path, FileMode.Open, FileAccess.Read))
                        //    {
                        //        long length = (fs.Length + 3) / 4 * 4;
                        //        await outputPaneWriter.WriteLineAsync($"Adding {Path.GetFileNameWithoutExtension(peItem.Path)} v{peItem.Version} ({length.ToString()} bytes) to deployment bundle");
                        //        byte[] buffer = new byte[length];

                        //        await Task.Yield();

                        //        await fs.ReadAsync(buffer, 0, (int)fs.Length);
                        //        assemblies.Add(buffer);

                        //        // Increment totalizer
                        //        totalSizeOfAssemblies += length;
                        //    }
                        //}

                        //await outputPaneWriter.WriteLineAsync($"Deploying {peCollection.Count:N0} assemblies to device... Total size in bytes is {totalSizeOfAssemblies.ToString()}.");
                        //MessageCentre.InternalErrorMessage("Deploying assemblies.");

                        //// need to keep a copy of the deployment blob for the second attempt (if needed)
                        //var assemblyCopy = new List<byte[]>(assemblies);

                        await Task.Yield();

                        await Task.Run(async delegate
                        {
                            // OK to skip erase as we just did that
                            // no need to reboot device
                            if (!device.DebugEngine.DeploymentExecute(
                                assemblyCopy,
                                false,
                                true,
                                null,
                                null))
                            {
                                // if the first attempt fails, give it another try

                                // wait before next pass
                                await Task.Delay(TimeSpan.FromSeconds(1));

                                await Task.Yield();

                                //MessageCentre.InternalErrorMessage("Deploying assemblies. Second attempt.");

                                //// !! need to use the deployment blob copy
                                //assemblyCopy = new List<byte[]>(assemblies);

                                //// can't skip erase as we just did that
                                //// no need to reboot device
                                //if (!device.DebugEngine.DeploymentExecute(
                                //    assemblyCopy,
                                //    false,
                                //    false,
                                //    progressIndicator,
                                //    logProgressIndicator))
                                //{
                                //    MessageCentre.InternalErrorMessage("Deployment failed.");

                                //    // throw exception to signal deployment failure
                                //    throw new DeploymentException("Deploy failed.");
                                //}
                            }
                        });

                        await Task.Yield();

                        // attach listner for messages
                        device.DebugEngine.OnMessage -= new MessageEventHandler(OnMessage);


                        device.DebugEngine.RebootDevice(RebootOptions.ClrOnly);


                    }
                    else
                    {
                        // after retry policy applied seems that we couldn't resume execution on the device...

//                        MessageCentre.InternalErrorMessage("Failed to initialize device.");

                    }
                }
            }
        }

        private void OnMessage(IncomingMessage message, string text)
        {
            throw new NotImplementedException();
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
            var workingDirectory = Path.GetDirectoryName(source);
            var allPeFiles = Directory.GetFiles(workingDirectory, "*.pe");            

            // prepare the process start of the WIN32 nanoCLR
            _nanoClr = new Process();

            AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
            AutoResetEvent errorWaitHandle = new AutoResetEvent(false);
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            try
            {
                // prepare parameters to load nanoCLR, include:
                // 1. unit test launcher
                // 2. mscorlib
                // 3. test framework
                // 4. test application
                StringBuilder str = new StringBuilder();
                foreach(var pe in allPeFiles)
                {
                    str.Append($" -load {Path.Combine(workingDirectory, pe)}");
                }

                string parameter = str.ToString();

                _logger.LogMessage(
                    "Launching process with nanoCLR...",
                    Settings.LoggingLevel.Verbose);

                _nanoClr.StartInfo = new ProcessStartInfo(TestObjectHelper.GetNanoClrLocation(), parameter)
                {
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                // launch nanoCLR
                if (!_nanoClr.Start())
                {
                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = "Failed to start nanoCLR";

                    _logger.LogPanicMessage(
                        "Failed to start nanoCLR!");
                }

                _nanoClr.OutputDataReceived += (sender, e) =>
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

                _nanoClr.ErrorDataReceived += (sender, e) =>
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

                _nanoClr.Start();

                _nanoClr.BeginOutputReadLine();
                _nanoClr.BeginErrorReadLine();

                _logger.LogMessage(
                    $"nanoCLR started @ process ID: {_nanoClr.Id}",
                    Settings.LoggingLevel.Detailed);


                // wait for exit, no worries about the outcome
                _nanoClr.WaitForExit(runTimeout);

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
                if (!_nanoClr.HasExited)
                {
                    _logger.LogMessage(
                        "Attempting to kill nanoCLR process...",
                        Settings.LoggingLevel.Verbose);

                    _nanoClr.Kill();
                    _nanoClr.WaitForExit(runTimeout);
                }
            }

            return results;
        }
    }
}
