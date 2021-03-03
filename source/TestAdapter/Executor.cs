//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
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
using System.Runtime.InteropServices.WindowsRuntime;
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

                if (_settings.IsRealHardware)
                {
                    // we are connecting to a real device
                    results = RunTestOnHardwareAsync(groups.ToList()).GetAwaiter().GetResult();
                }
                else
                {
                    // we are connecting to WIN32 nanoCLR
                    results = RunTestOnEmulator(groups.ToList());
                }

                foreach (var result in results)
                {
                    frameworkHandle.RecordResult(result);
                }
            }
        }

        private async Task<List<TestResult>> RunTestOnHardwareAsync(List<TestCase> tests)
        {
            _logger.LogMessage(
                "Setting up test runner in *** CONNECTED DEVICE ***",
                Settings.LoggingLevel.Detailed);

            List<TestResult> results = PrepareListResult(tests);
            List<byte[]> assemblies = new List<byte[]>();
            int retryCount = 0;
            string port = _settings.RealHardwarePort == string.Empty ? "COM4" : _settings.RealHardwarePort;
            //var serialDebugClient = PortBase.CreateInstanceForSerial("", null, true, new List<string>() { port });
            var serialDebugClient = PortBase.CreateInstanceForSerial("", null, true, null);

        retryConnection:
            _logger.LogMessage($"Checking device on port {port}.", Settings.LoggingLevel.Verbose);
            while (!serialDebugClient.IsDevicesEnumerationComplete)
            {
                Thread.Sleep(1);
            }

            _logger.LogMessage($"Found: {serialDebugClient.NanoFrameworkDevices.Count} devices", Settings.LoggingLevel.Verbose);

            if (serialDebugClient.NanoFrameworkDevices.Count == 0)
            {
                if (retryCount > _numberOfRetries)
                {
                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = $"Couldn't find any device, please try to disable the device scanning in the Visual Studio Extension! If the situation persists reboot the device as well.";
                    return results;
                }
                else
                {
                    retryCount++;
                    serialDebugClient.ReScanDevices();
                    goto retryConnection;
                }
            }

            retryCount = 0;
            NanoDeviceBase device;
            if (serialDebugClient.NanoFrameworkDevices.Count > 1)
            {
                device = serialDebugClient.NanoFrameworkDevices.Where(m => m.SerialNumber == port).First();
            }
            else
            {
                device = serialDebugClient.NanoFrameworkDevices[0];
            }

            _logger.LogMessage(
                $"Getting things with {device.Description}",
                Settings.LoggingLevel.Detailed);

            // check if debugger engine exists
            if (device.DebugEngine == null)
            {
                device.CreateDebugEngine();
                _logger.LogMessage($"Debug engine created.", Settings.LoggingLevel.Verbose);
            }

            bool deviceIsInInitializeState = false;            

        retryDebug:
            bool connectResult = await device.DebugEngine.ConnectAsync(5000, true);
            _logger.LogMessage($"Device connect result is {connectResult}. Attempt {retryCount}/{_numberOfRetries}", Settings.LoggingLevel.Verbose);

            if (!connectResult)
            {
                if (retryCount < _numberOfRetries)
                {
                    // Give it a bit of time
                    await Task.Delay(100);
                    retryCount++;
                    goto retryDebug;
                }
                else
                {
                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = $"Couldn't connect to the device, please try to disable the device scanning in the Visual Studio Extension! If the situation persists reboot the device as well.";
                    return results;
                }
            }

            retryCount = 0;
        retryErase:
            // erase the device
            var eraseResult = await Task.Run(async delegate
            {
                _logger.LogMessage($"Erase deployment block storage. Attempt {retryCount}/{_numberOfRetries}.", Settings.LoggingLevel.Verbose);
                return await device.EraseAsync(
                    EraseOptions.Deployment,
                    CancellationToken.None,
                    null,
                    null);
            });

            _logger.LogMessage($"Erase result is {eraseResult}.", Settings.LoggingLevel.Verbose);
            if (!eraseResult)
            {
                if (retryCount < _numberOfRetries)
                {
                    // Give it a bit of time
                    await Task.Delay(400);
                    retryCount++;
                    goto retryErase;
                }
                else
                {
                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = $"Couldn't erase the device, please try to disable the device scanning in the Visual Studio Extension! If the situation persists reboot the device as well.";
                    return results;
                }
            }

            retryCount = 0;
            // initial check 
            if (device.DebugEngine.IsDeviceInInitializeState())
            {
                    _logger.LogMessage($"Device status verified as being in initialized state. Requesting to resume execution. Attempt {retryCount}/{_numberOfRetries}.", Settings.LoggingLevel.Error);
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
                    _logger.LogMessage($"Device has completed initialization.", Settings.LoggingLevel.Verbose);
                    // done here
                    deviceIsInInitializeState = false;
                    break;
                }

                _logger.LogMessage($"Waiting for device to report initialization completed ({retryCount}/{_numberOfRetries}).", Settings.LoggingLevel.Verbose);
                // provide feedback to user on the 1st pass
                if (retryCount == 0)
                {
                    _logger.LogMessage($"Waiting for device to initialize.", Settings.LoggingLevel.Verbose);
                }

                if (device.DebugEngine.IsConnectedTonanoBooter)
                {
                    _logger.LogMessage($"Device reported running nanoBooter. Requesting to load nanoCLR.", Settings.LoggingLevel.Verbose);
                    // request nanoBooter to load CLR
                    device.DebugEngine.ExecuteMemory(0);
                }
                else if (device.DebugEngine.IsConnectedTonanoCLR)
                {
                    _logger.LogMessage($"Device reported running nanoCLR. Requesting to reboot nanoCLR.", Settings.LoggingLevel.Error);
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
                _logger.LogMessage($"Device is initialized and ready!", Settings.LoggingLevel.Verbose);
                await Task.Yield();


                //////////////////////////////////////////////////////////
                // sanity check for devices without native assemblies ?!?!
                if (device.DeviceInfo.NativeAssemblies.Count == 0)
                {
                    _logger.LogMessage($"Device reporting no assemblies loaded. This can not happen. Sanity check failed.", Settings.LoggingLevel.Error);
                    // there are no assemblies deployed?!
                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = $"Couldn't find any native assemblies deployed in {device.Description}, {device.TargetName} on {device.SerialNumber}! If the situation persists reboot the device.";
                    return results;
                }

                _logger.LogMessage($"Computing deployment blob.", Settings.LoggingLevel.Verbose);
                // build a list with the full path for each DLL, referenced DLL and EXE
                List<DeploymentAssembly> assemblyList = new List<DeploymentAssembly>();

                var source = tests.First().Source;
                var workingDirectory = Path.GetDirectoryName(source);
                var allPeFiles = Directory.GetFiles(workingDirectory, "*.pe");

                // load tests in case we don't need to check the version:
                //foreach (var pe in allPeFiles)
                //{
                //    assemblyList.Add(
                //        new DeploymentAssembly(Path.Combine(workingDirectory, pe), "", ""));
                //}

                // TODO do we need to check versions?
                var decompilerSettings = new DecompilerSettings
                {
                    LoadInMemory = false,
                    ThrowOnAssemblyResolveErrors = false
                };

                foreach (string assemblyPath in allPeFiles)
                {
                    // load assembly in order to get the versions
                    var file = Path.Combine(workingDirectory, assemblyPath.Replace(".pe", ".dll"));
                    if (!File.Exists(file))
                    {
                        // Check with an exe
                        file = Path.Combine(workingDirectory, assemblyPath.Replace(".pe", ".exe"));
                    }

                    var decompiler = new CSharpDecompiler(file, decompilerSettings); ;
                    var assemblyProperties = decompiler.DecompileModuleAndAssemblyAttributesToString();

                    // read attributes using a Regex

                    // AssemblyVersion
                    string pattern = @"(?<=AssemblyVersion\("")(.*)(?=\""\)])";
                    var match = Regex.Matches(assemblyProperties, pattern, RegexOptions.IgnoreCase);
                    string assemblyVersion = match[0].Value;

                    // AssemblyNativeVersion
                    pattern = @"(?<=AssemblyNativeVersion\("")(.*)(?=\""\)])";
                    match = Regex.Matches(assemblyProperties, pattern, RegexOptions.IgnoreCase);

                    // only class libs have this attribute, therefore sanity check is required
                    string nativeVersion = "";
                    if (match.Count == 1)
                    {
                        nativeVersion = match[0].Value;
                    }

                    assemblyList.Add(new DeploymentAssembly(Path.Combine(workingDirectory, assemblyPath), assemblyVersion, nativeVersion));
                }

                _logger.LogMessage($"Added {assemblyList.Count} assemblies to deploy.", Settings.LoggingLevel.Verbose);
                await Task.Yield();

                // Keep track of total assembly size
                long totalSizeOfAssemblies = 0;

                // TODO use this code to load the PE files

                // now we will re-deploy all system assemblies
                foreach (DeploymentAssembly peItem in assemblyList)
                {
                    // append to the deploy blob the assembly
                    using (FileStream fs = File.Open(peItem.Path, FileMode.Open, FileAccess.Read))
                    {
                        long length = (fs.Length + 3) / 4 * 4;
                        _logger.LogMessage($"Adding {Path.GetFileNameWithoutExtension(peItem.Path)} v{peItem.Version} ({length} bytes) to deployment bundle", Settings.LoggingLevel.Verbose);
                        byte[] buffer = new byte[length];

                        await Task.Yield();

                        await fs.ReadAsync(buffer, 0, (int)fs.Length);
                        assemblies.Add(buffer);

                        // Increment totalizer
                        totalSizeOfAssemblies += length;
                    }
                }

                _logger.LogMessage($"Deploying {assemblyList.Count:N0} assemblies to device... Total size in bytes is {totalSizeOfAssemblies}.", Settings.LoggingLevel.Verbose);
                // need to keep a copy of the deployment blob for the second attempt (if needed)
                var assemblyCopy = new List<byte[]>(assemblies);

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

                        _logger.LogMessage("Deploying assemblies. Second attempt.", Settings.LoggingLevel.Verbose);

                        // !! need to use the deployment blob copy
                        assemblyCopy = new List<byte[]>(assemblies);

                        // can't skip erase as we just did that
                        // no need to reboot device
                        if (!device.DebugEngine.DeploymentExecute(
                        assemblyCopy,
                        false,
                        false,
                        null,
                        null))
                        {
                            _logger.LogMessage("Deployment failed.", Settings.LoggingLevel.Error);

                            // throw exception to signal deployment failure
                            results.First().Outcome = TestOutcome.Failed;
                            results.First().ErrorMessage = $"Deployment failed in {device.Description}, {device.TargetName} on {device.SerialNumber}! If the situation persists reboot the device.";
                        }
                    }
                });

                await Task.Yield();
                // If there has been an issue before, the first test is marked as failed
                if (results.First().Outcome == TestOutcome.Failed)
                {
                    return results;
                }

                StringBuilder output = new StringBuilder();
                bool isFinished = false;
                // attach listner for messages
                device.DebugEngine.OnMessage += (message, text) =>
                {
                    _logger.LogMessage(text, Settings.LoggingLevel.Verbose);
                    output.Append(text);
                    if (text.Contains(Done))
                    {
                        isFinished = true;
                    }
                };

                device.DebugEngine.RebootDevice(RebootOptions.ClrOnly);

                while (!isFinished)
                {
                    Thread.Sleep(1);
                }

                _logger.LogMessage($"Tests finished.", Settings.LoggingLevel.Verbose);
                CheckAllTests(output.ToString(), results);
            }
            else
            {
                _logger.LogMessage("Failed to initialize device.", Settings.LoggingLevel.Error);
            }


            return results;
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

        private List<TestResult> PrepareListResult(List<TestCase> tests)
        {
            List<TestResult> results = new List<TestResult>();

            foreach (var test in tests)
            {
                TestResult result = new TestResult(test) { Outcome = TestOutcome.None };
                results.Add(result);
            }

            return results;
        }

        private List<TestResult> RunTestOnEmulator(List<TestCase> tests)
        {
            _logger.LogMessage(
                "Setting up test runner in *** nanoCLR WIN32***",
                Settings.LoggingLevel.Detailed);

            int runTimeout = 10000;
            if (_settings != null)
            {
                runTimeout = _settings.TestTimeOutSeconds * 1000;
            }

            _logger.LogMessage(
                $"Timeout set to {runTimeout}ms",
                Settings.LoggingLevel.Verbose);

            List<TestResult> results = PrepareListResult(tests);

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
                foreach (var pe in allPeFiles)
                {
                    str.Append($" -load {Path.Combine(workingDirectory, pe)}");
                }

                string parameter = str.ToString();

                _logger.LogMessage(
                    $"Parameters to pass to nanoCLR: <{parameter}>",
                    Settings.LoggingLevel.Verbose);

                var nanoClrLocation = TestObjectHelper.GetNanoClrLocation();
                if(string.IsNullOrEmpty(nanoClrLocation))
                {
                    _logger.LogPanicMessage("Can't find nanoCLR Win32 in any of the directories!");
                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = "Can't find nanoCLR Win32 in any of the directories!";
                    return results;
                }

                _logger.LogMessage($"Found nanoCLR Win32: {nanoClrLocation}", Settings.LoggingLevel.Verbose);
                _nanoClr.StartInfo = new ProcessStartInfo(nanoClrLocation, parameter)
                {
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                _logger.LogMessage(
                    $"Launching process with nanoCLR (from {Path.GetFullPath(TestObjectHelper.GetNanoClrLocation())})",
                    Settings.LoggingLevel.Verbose);

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

                CheckAllTests(output.ToString(), results);
                _logger.LogMessage(output.ToString(), Settings.LoggingLevel.Verbose);
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

        private void CheckAllTests(string toCheck, List<TestResult> results)
        {
            var outputStrings = Regex.Split(toCheck, @"((\r)+)?(\n)+((\r)+)?").Where(m => !string.IsNullOrEmpty(m));

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
        }
    }
}
