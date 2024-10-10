// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using CliWrap;
using CliWrap.Buffered;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using nanoFramework.TestAdapter;
using nanoFramework.Tools.Debugger;
using nanoFramework.Tools.Debugger.Extensions;
using nanoFramework.Tools.Debugger.NFDevice;

namespace nanoFramework.TestPlatform.TestAdapter
{
    /// <summary>
    /// An Executor class
    /// </summary>
    [ExtensionUri(TestsConstants.NanoExecutor)]
    class Executor : ITestExecutor
    {
        private const string TestPassed = "Test passed";
        private const string TestFailed = "Test failed";
        private const string TestSkipped = "Test skipped";
        private const string Exiting = "Exiting.";
        private const string Done = "Done.";
        private Settings _settings;
        private LogMessenger _logger;
        private Process _nanoClr;

        // number of retries when performing a deploy operation
        private const int _numberOfRetries = 5;

        // timeout when performing a deploy operation
        private const int _timeoutMiliseconds = 1000;

        /// test session timeout (from the runsettings file)
        private int _testSessionTimeout = 30_0000;

        // timeout to get exclusive access to a device
        private const int _timeoutExclusiveAccess = 5000;

        private IFrameworkHandle _frameworkHandle = null;

        /// <inheritdoc/>
        public void Cancel()
        {
            try
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
            }
            catch (Exception ex)
            {
                _logger?.LogPanicMessage($"Exception thrown while killing the process: {ex}");
            }
        }

        /// <inheritdoc/>
        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            try
            {
                InitializeLogger(runContext, frameworkHandle);

                foreach (var source in sources)
                {
                    var testsCases = TestDiscoverer.ComposeTestCases(source);

                    RunTests(testsCases, runContext, frameworkHandle);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogPanicMessage($"Exception raised in the process: {ex}");
            }
        }

        /// <inheritdoc/>
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            try
            {
                InitializeLogger(runContext, frameworkHandle);
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

                    List<TestResult> results;

                    if (_settings.IsRealHardware)
                    {
                        // we are connecting to a real device
                        results = RunTestOnHardwareAsync(groups.ToList()).GetAwaiter().GetResult();
                    }
                    else
                    {
                        // we are connecting to nanoCLR CLI
                        results = RunTestOnEmulatorAsync(
                            groups.ToList(),
                            _logger).GetAwaiter().GetResult();
                    }

                    foreach (var result in results)
                    {
                        frameworkHandle.RecordResult(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogPanicMessage($"Exception raised in the process: {ex}");
            }
        }

        private void InitializeLogger(IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            if (_logger != null)
            {
                return;
            }

            var settingsProvider = runContext.RunSettings.GetSettings(TestsConstants.SettingsName) as SettingsProvider;

            _logger = new LogMessenger(frameworkHandle, settingsProvider);

            if (settingsProvider != null)
            {
                // get TestSessionTimeout from runsettings
                var xml = new XmlDocument();
                xml.LoadXml(runContext.RunSettings.SettingsXml);
                var timeout = xml.SelectSingleNode("RunSettings//RunConfiguration//TestSessionTimeout");
                if (timeout != null && timeout.NodeType == XmlNodeType.Element)
                {
                    int.TryParse(timeout.InnerText, out _testSessionTimeout);
                }

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
        }

        private async Task<List<TestResult>> RunTestOnHardwareAsync(List<TestCase> tests)
        {
            _logger.LogMessage(
                "Setting up test runner in *** CONNECTED DEVICE ***",
                Settings.LoggingLevel.Detailed);

            List<TestResult> results = PrepareListResult(tests);
            List<byte[]> assemblies = new List<byte[]>();
            int retryCount = 0;
            NanoDeviceBase device = null;
            GlobalExclusiveDeviceAccess exclusiveAccess = null;
            try
            {

                bool realHardwarePortSet = !string.IsNullOrEmpty(_settings.RealHardwarePort);

                PortBase serialDebugClient;

                if (realHardwarePortSet)
                {
                    serialDebugClient = PortBase.CreateInstanceForSerial(false);

                    _logger.LogMessage($"Checking device on port {_settings.RealHardwarePort}.", Settings.LoggingLevel.Verbose);

                    exclusiveAccess = GlobalExclusiveDeviceAccess.TryGet(_settings.RealHardwarePort, _timeoutExclusiveAccess);
                    if (exclusiveAccess is null)
                    {
                        results.First().Outcome = TestOutcome.Skipped;
                        results.First().ErrorMessage = $"Couldn't access the device @ {_settings.RealHardwarePort}. Another application is using the device. If the situation persists reboot the device and/or disconnect and connect it again.";

                        _logger.LogMessage($"Couldn't get exclusive access to the nanoDevice @ {_settings.RealHardwarePort}.", Settings.LoggingLevel.Verbose);

                        return results;
                    }

                    try
                    {
                        serialDebugClient.AddDevice(_settings.RealHardwarePort);

                        device = serialDebugClient.NanoFrameworkDevices[0];

                        // all good here, proceed to execute tests
                        goto executeTests;
                    }
#if DEBUG
                    catch (Exception ex)
#else
                catch
#endif
                    {
                        results.First().Outcome = TestOutcome.Failed;
                        results.First().ErrorMessage = $"Couldn't find any valid nanoDevice @ {_settings.RealHardwarePort}. Maybe try to disable the device watchers in Visual Studio Extension! If the situation persists reboot the device and/or disconnect and connect it again.";

                        _logger.LogMessage($"Couldn't find any valid nanoDevice @ {_settings.RealHardwarePort}.", Settings.LoggingLevel.Verbose);

                        return results;
                    }
                }
                else
                {
                    serialDebugClient = PortBase.CreateInstanceForSerial(true,
                                                                         2000);
                }

            retryConnection:

                if (string.IsNullOrEmpty(_settings.RealHardwarePort))
                {
                    _logger.LogMessage($"Waiting for device enumeration to complete.", Settings.LoggingLevel.Verbose);
                }

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
                        results.First().ErrorMessage = "Couldn't find any valid nanoDevice. Maybe try to disable the device watchers in Visual Studio Extension! If the situation persists reboot the device and/or disconnect and connect it again.";

                        _logger.LogMessage("Couldn't find any valid nanoDevice.", Settings.LoggingLevel.Verbose);

                        return results;
                    }
                    else
                    {
                        // add retry counter before trying again
                        retryCount++;

                        // re-scan devices
                        serialDebugClient.ReScanDevices();

                        goto retryConnection;
                    }
                }

                retryCount = 0;

                // grab the 1st device available
                device = serialDebugClient.NanoFrameworkDevices[0];

                if (exclusiveAccess is null)
                {
                    exclusiveAccess = GlobalExclusiveDeviceAccess.TryGet(device, _timeoutExclusiveAccess);
                    if (exclusiveAccess is null)
                    {
                        results.First().Outcome = TestOutcome.Skipped;
                        results.First().ErrorMessage = $"Couldn't access the device {device.Description}. Another application is using the device. If the situation persists reboot the device and/or disconnect and connect it again.";

                        _logger.LogMessage($"Couldn't get exclusive access to the nanoDevice @ {device.Description}.", Settings.LoggingLevel.Verbose);

                        return results;
                    }
                }

            executeTests:

                _logger.LogMessage(
                    $"Getting things ready with {device.Description}",
                    Settings.LoggingLevel.Detailed);

                // check if debugger engine exists
                if (device.DebugEngine == null)
                {
                    device.CreateDebugEngine();
                    _logger.LogMessage($"Debug engine created.", Settings.LoggingLevel.Verbose);
                }

                bool deviceIsInInitializeState = false;

            retryDebug:
                bool connectResult = device.DebugEngine.Connect(5000, true, true);
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
                _logger.LogMessage($"Erase deployment block storage. Attempt {retryCount}/{_numberOfRetries}.", Settings.LoggingLevel.Verbose);

                var eraseResult = device.Erase(
                        EraseOptions.Deployment,
                        null,
                        null);

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

                    var deploymentLogger = new Progress<string>((m) => _logger.LogMessage(m, Settings.LoggingLevel.Detailed));

                    await Task.Run(async delegate
                    {
                        // OK to skip erase as we just did that
                        // no need to reboot device
                        if (!device.DebugEngine.DeploymentExecute(
                            assemblyCopy,
                            false,
                            false,
                            null,
                            deploymentLogger))
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
                                deploymentLogger))
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
                    ManualResetEvent testExecutionCompleted = new ManualResetEvent(false);

                    // attach listener for messages
                    device.DebugEngine.OnMessage += (message, text) =>
                    {
                        _logger.LogMessage(text, Settings.LoggingLevel.Verbose);
                        output.Append(text);
                        if (text.Contains(Done))
                        {
                            // signal test execution completed
                            testExecutionCompleted.Set();
                        }
                    };

                    device.DebugEngine.RebootDevice(RebootOptions.ClrOnly);

                    DateTime timeoutForExecution = DateTime.UtcNow.AddMilliseconds(_testSessionTimeout);

                    if (testExecutionCompleted.WaitOne(_testSessionTimeout))
                    {
                        _logger.LogMessage($"Tests finished.", Settings.LoggingLevel.Verbose);

                        ParseTestResults(output.ToString(), results);
                    }
                    else
                    {
                        _logger.LogMessage($"Tests timed out.", Settings.LoggingLevel.Error);
                        results.First().Outcome = TestOutcome.Failed;
                        results.First().ErrorMessage = $"Tests timed out in {device.Description}";
                    }
                }
                else
                {
                    _logger.LogMessage("Failed to initialize device.", Settings.LoggingLevel.Error);
                }
            }
            finally
            {
                exclusiveAccess?.Dispose();
            }

            return results;
        }

        private List<TestResult> PrepareListResult(List<TestCase> tests)
        {
            List<TestResult> results = new List<TestResult>();

            foreach (var test in tests)
            {
                TestResult result = new TestResult(test) { Outcome = TestOutcome.None };

                foreach (var t in result.Traits)
                {
                    result.Traits.Add(new Trait(t.Name, ""));
                }

                results.Add(result);
            }

            return results;
        }

        private async Task<List<TestResult>> RunTestOnEmulatorAsync(
            List<TestCase> tests,
            LogMessenger _logger)
        {
            List<TestResult> results = PrepareListResult(tests);

            _logger.LogMessage(
                "Setting up test runner in *** nanoCLR CLI ***",
                Settings.LoggingLevel.Detailed);

            _logger.LogMessage(
                $"Timeout set to {_testSessionTimeout}ms",
                Settings.LoggingLevel.Verbose);

            // check if nanoCLR needs to be installed/updated
            if (!NanoCLRHelper.NanoClrIsInstalled
                && !NanoCLRHelper.InstallNanoClr(_logger))
            {
                results.First().Outcome = TestOutcome.Failed;
                results.First().ErrorMessage = "Failed to install/update nanoCLR CLI. Check log for details.";

                return results;
            }

            // update nanoCLR instance, if not running a local one
            if (string.IsNullOrEmpty(_settings.PathToLocalCLRInstance))
            {
                NanoCLRHelper.UpdateNanoCLRInstance(
                    _settings.CLRVersion,
                    _logger);
            }

            _logger.LogMessage(
                "Processing assemblies to load into test runner...",
                Settings.LoggingLevel.Verbose);

            var source = tests.First().Source;
            var workingDirectory = Path.GetDirectoryName(source);
            var allPeFiles = Directory.GetFiles(workingDirectory, "*.pe");

            // prepare launch of nanoCLR CLI
            StringBuilder arguments = new StringBuilder();

            // assemblies to load
            arguments.Append("run --assemblies ");

            foreach (var pe in allPeFiles)
            {
                arguments.Append($" \"{Path.Combine(workingDirectory, pe)}\"");
            }

            // should we use a local nanoCLR instance?
            if (!string.IsNullOrEmpty(_settings.PathToLocalCLRInstance))
            {
                arguments.Append($"  --localinstance \"{_settings.PathToLocalCLRInstance}\"");
            }

            // if requested, set diagnostic output
            if (_settings.Logging > Settings.LoggingLevel.None)
            {
                arguments.Append(" -v diag");
            }

            // add any extra arguments
            if (!string.IsNullOrEmpty(_settings.RunnerExtraArguments))
            {
                arguments.Append($" {_settings.RunnerExtraArguments} ");
            }

            _logger.LogMessage(
                $"Launching nanoCLR with these arguments: '{arguments}'",
                Settings.LoggingLevel.Verbose);

            // launch nanoCLR
            var cmd = Cli.Wrap("nanoclr")
                 .WithArguments(arguments.ToString())
                 .WithValidation(CommandResultValidation.None);

            // setup cancellation token with the timeout from settings
            using (var cts = new CancellationTokenSource(_testSessionTimeout))
            {
                var cliResult = await cmd.ExecuteBufferedAsync(cts.Token);
                var exitCode = cliResult.ExitCode;

                // read standard output
                var output = cliResult.StandardOutput;

                if (exitCode == 0)
                {
                    try
                    {
                        // process output to gather tests results
                        ParseTestResults(output, results);

                        _logger.LogMessage(output, Settings.LoggingLevel.Verbose);

                        if (!output.Contains(Done))
                        {
                            results.First().Outcome = TestOutcome.Failed;
                            results.First().ErrorMessage = output;
                        }

                        var notPassedOrFailed = results.Where(m => m.Outcome != TestOutcome.Failed
                                                                   && m.Outcome != TestOutcome.Passed
                                                                   && m.Outcome != TestOutcome.Skipped);

                        if (notPassedOrFailed.Any())
                        {
                            notPassedOrFailed.First().ErrorMessage = output;
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogMessage(
                            $"Fatal exception when processing test results: >>>{ex.Message}\r\n{output}",
                            Settings.LoggingLevel.Detailed);

                        results.First().Outcome = TestOutcome.Failed;
                    }
                }
                else
                {
                    _logger.LogPanicMessage($"nanoCLR ended with '{exitCode}' exit code.\r\n>>>>>>>>>>>>>\r\n{output}\r\n>>>>>>>>>>>>>");

                    results.First().Outcome = TestOutcome.Failed;
                    results.First().ErrorMessage = $"nanoCLR execution ended with exit code: {exitCode}. Check log for details.";

                    return results;
                }
            }

            return results;
        }

        private void ParseTestResults(string rawOutput, List<TestResult> results)
        {
            var outputStrings = Regex.Replace(
                rawOutput,
                @"^\s+$[\r\n]*",
                "",
                RegexOptions.Multiline).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            _logger.LogMessage(
                "Parsing test results...",
                Settings.LoggingLevel.Verbose);

            StringBuilder testOutput = new StringBuilder();

            bool readyFound = false;
            string method;
            TestResult testResult = new TestResult(new TestCase());
            string[] resultDataSet = default;

            foreach (var line in outputStrings)
            {
                if ((line.Contains(TestPassed)
                    || (line.Contains(TestFailed))
                    || (line.Contains(TestSkipped))))
                {
                    resultDataSet = line.Split(',');

                    // sanity check for enough data
                    if (resultDataSet.Length != 3)
                    {
                        // something wrong!
                        _logger.LogPanicMessage($"*** ERROR: can't parse test result {line}");

                        continue;
                    }

                    method = resultDataSet[1].Trim();

                    // Find the test
                    testResult = results.FirstOrDefault(m => m.TestCase.FullyQualifiedName == method);

                    if (testResult is null)
                    {
                        // something wrong!
                        _logger.LogPanicMessage($"*** ERROR: can't find test result for test {method}");

                        continue;
                    }
                }

                if (line.Contains(TestPassed))
                {
                    // Format is "Test passed,MethodName,ticks";

                    string ticks = resultDataSet[2];
                    long.TryParse(ticks, out long ticksNum);

                    testResult.Duration = TimeSpan.FromTicks(ticksNum);
                    testResult.Outcome = TestOutcome.Passed;
                    testResult.Messages.Add(new TestResultMessage(
                        TestResultMessage.StandardOutCategory,
                        testOutput.ToString()));

                    // reset test output
                    testOutput = new StringBuilder();
                }
                else if (line.Contains(TestFailed))
                {
                    // Format is "Test failed,MethodName,Exception message";

                    testResult.ErrorMessage = resultDataSet[2];
                    testResult.Outcome = TestOutcome.Failed;
                    testResult.Messages.Add(new TestResultMessage(
                        TestResultMessage.StandardErrorCategory,
                        testOutput.ToString()));

                    // reset test output
                    testOutput = new StringBuilder();
                }
                else if (line.Contains(TestSkipped))
                {
                    // Format is "Test failed,MethodName,Exception message";

                    testResult.ErrorMessage = resultDataSet[2];
                    testResult.Outcome = TestOutcome.Skipped;
                    testResult.Messages.Add(new TestResultMessage(
                        TestResultMessage.StandardErrorCategory,
                        testOutput.ToString()));

                    // If this is a Steup Test, set all the other tests from the class to skipped as well
                    var trait = testResult.TestCase.Traits.FirstOrDefault();

                    if (trait != null)
                    {
                        if (trait.Value == "Setup" && trait.Name == "Type")
                        {
                            // A test name is the full qualify name of the metho.methodname, finding the list . index will give all the familly name
                            var testCasesToSkipName = testResult.TestCase.FullyQualifiedName.Substring(0, testResult.TestCase.FullyQualifiedName.LastIndexOf('.'));
                            var allTestToSkip = results.Where(m => m.TestCase.FullyQualifiedName.Contains(testCasesToSkipName));
                            foreach (var testToSkip in allTestToSkip)
                            {
                                if (testToSkip.TestCase.FullyQualifiedName == resultDataSet[1])
                                {
                                    continue;
                                }

                                testToSkip.Outcome = TestOutcome.Skipped;
                                testResult.Messages.Add(new TestResultMessage(
                                    TestResultMessage.StandardErrorCategory,
                                    $"Setup method '{testResult.DisplayName}' has been skipped."));
                            }
                        }
                    }

                    // reset test output
                    testOutput = new StringBuilder();
                }
                else
                {
                    if (readyFound)
                    {
                        testOutput.AppendLine(line);

                        continue;
                    }

                    if (line.StartsWith("Ready."))
                    {
                        readyFound = true;
                    }
                }
            }
        }
    }
}
