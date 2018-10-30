//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using nanoFramework.TestPlatform.TestInterface;
using nanoFramework.TestPlatform.TestInterface.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using TestCase = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase;

namespace nanoFramework.TestPlatform.TestAdapter
{
    [ExtensionUri(TestInterface.Constants.NFExecutorUriString)]

    public class TestExecutor : ITestExecutor
    {
        public static readonly Uri ExecutorUri = new Uri(TestInterface.Constants.NFExecutorUriString);

        private bool _cancelled = false;
        private IRunContext _runContext = null;
        private IFrameworkHandle _frameworkHandle = null;
        private nFTestSettings _settings = new nFTestSettings();


        #region ITestExecutor

        public void Cancel()
        {
            _cancelled = true;
            //_executor.Cancel();
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            _cancelled = false;
            _runContext = runContext;
            _frameworkHandle = frameworkHandle;

            _frameworkHandle.SendMessage(TestMessageLevel.Error, "Hello from nF RunTests1");

            //// Retrieve nanoFramework specific settings
            //if (!PopulateSettings())
            //{
            //    _frameworkHandle.ErrorMessage(StringResources.SettingsMissingDiscoveryWarning);
            //    return;
            //}

            // Check if adapter is disabled
            if (_settings.Disabled)
            {
                _frameworkHandle.InformationalMessage(StringResources.TestAdapterDisabled);
                return;
            }

            // start execution
            _frameworkHandle.InformationalMessage(StringResources.StartingExecution);

            RunTests(tests);

            // done with execution
            _frameworkHandle.InformationalMessage(StringResources.ExecutionCompleted);

        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            _cancelled = false;
            _frameworkHandle = frameworkHandle;
            _runContext = runContext;

            _frameworkHandle.SendMessage(TestMessageLevel.Error, "Hello from nF RunTests2");

            //// Retrieve nanoFramework specific settings
            //if (!PopulateSettings())
            //{
            //    _frameworkHandle.ErrorMessage(StringResources.SettingsMissingDiscoveryWarning);
            //    return;
            //}

            // Check if adapter is disabled
            if (_settings.Disabled)
            {
                _frameworkHandle.InformationalMessage(StringResources.TestAdapterDisabled);
                return;
            }


            // start discovery
            _frameworkHandle.InformationalMessage(StringResources.StartingDiscovery);

            var tests = DiscoverTests(sources);

            // done with discovery
            _frameworkHandle.InformationalMessage(StringResources.DiscoveryCompleted);

            // run tests
            _frameworkHandle.InformationalMessage(StringResources.StartingExecution);

            RunTests(tests);

            // done with discovery
            _frameworkHandle.InformationalMessage(StringResources.ExecutionCompleted);

        }

        #endregion

        private bool PopulateSettings()
        {
            // Populate our test setting with whatever is coming in the runsettings
            SettingsProvider settingsprovider = null;
            bool found = false;
            try
            {

                var sP = _runContext?.RunSettings?.GetSettings(nFTestSettings.SettingsName);
                var sP1 = sP as SettingsProvider;

                settingsprovider = sP1;

                found = true;
            }
            catch (Exception ex)
            {
                _settings = new nFTestSettings();
            }

            //_executor = new Catch2Interface.Executor(_settings, _runContext.SolutionDirectory, _runContext.TestRunDirectory);

            return found;
        }


        private List<TestCase> DiscoverTests(IEnumerable<string> sources)
        {
            var tests = new List<TestCase>();

            var discoverer = new UnitTestDiscoverer(_settings);

            var testCases = discoverer.DiscoverTests(sources);
            //if (!string.IsNullOrEmpty(discoverer.Log))
            //{
            //    _logger.InformationalMessage($"Discover log:{Environment.NewLine}{discoverer.Log}");
            //}

            // Add testcases to discovery sink
            //LogDebug(TestMessageLevel.Informational, "Start adding test cases to discovery sink");
            foreach (var test in testCases)
            {
                tests.Add(test.ToVSTestCase());
                //LogDebug(TestMessageLevel.Informational, $"  {testcase.Name}");
            }
            //LogDebug(TestMessageLevel.Informational, "Finished adding test cases to discovery sink");

            return tests;
        }

        private void RunTests(IEnumerable<TestCase> tests)
        {
            //_executor.InitTestRuns();

            foreach (var test in tests)
            {
                if (_cancelled) break;

                _frameworkHandle.RecordStart(test);

                var result = RunTest(test);

                _frameworkHandle.RecordResult(result);
            }
        }
        private TestResult RunTest(TestCase test)
        {
            //LogVerbose(TestMessageLevel.Informational, $"Run test: {test.FullyQualifiedName}");
            _frameworkHandle.InformationalMessage($"Source: {test.Source}");
            _frameworkHandle.InformationalMessage($"SolutionDirectory: {_runContext.SolutionDirectory}");
            _frameworkHandle.InformationalMessage($"TestRunDirectory: {_runContext.TestRunDirectory}");

            TestResult result = new TestResult(test);

            // Check if file exists
            if (!File.Exists(test.Source))
            {
                result.Outcome = TestOutcome.NotFound;
            }

            // Run test
            if (_runContext.IsBeingDebugged)
            {
                _frameworkHandle.InformationalMessage("Start debug run.");
                //_frameworkHandle
                //    .LaunchProcessWithDebuggerAttached(test.Source
                //                                      , null
                //                                      , _executor.GenerateCommandlineArguments(test.DisplayName, true)
                //                                      , null);

                // Do not process output in Debug mode
                result.Outcome = TestOutcome.None;
            }
            else
            {
                result.Outcome = TestOutcome.Passed;


                //var testresult = _executor.Run(test.DisplayName, test.Source);

                //if (!string.IsNullOrEmpty(_executor.Log))
                //{
                //    LogNormal(TestMessageLevel.Informational, $"Executor log:{Environment.NewLine}{_executor.Log}");
                //}

                // Process test results
                //switch (testresult.Outcome)
                //{
                //    case Catch2Interface.TestOutcomes.Timedout:
                //        LogVerbose(TestMessageLevel.Warning, "Time out");
                //        result.Outcome = TestOutcome.Skipped;
                //        result.ErrorMessage = testresult.ErrorMessage;
                //        result.Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, testresult.StandardOut));
                //        result.Duration = testresult.Duration;
                //        break;
                //    case Catch2Interface.TestOutcomes.Cancelled:
                //        result.Outcome = TestOutcome.None;
                //        break;
                //    case Catch2Interface.TestOutcomes.Skipped:
                //        result.Outcome = TestOutcome.Skipped;
                //        result.ErrorMessage = testresult.ErrorMessage;
                //        break;
                //    default:
                //        if (testresult.Outcome == Catch2Interface.TestOutcomes.Passed)
                //        {
                //            result.Outcome = TestOutcome.Passed;
                //        }
                //        else
                //        {
                //            result.Outcome = TestOutcome.Failed;
                //        }
                //        result.Duration = testresult.Duration;
                //        result.ErrorMessage = testresult.ErrorMessage;
                //        result.ErrorStackTrace = testresult.ErrorStackTrace;

                //        if (!string.IsNullOrEmpty(testresult.StandardOut))
                //        {
                //            result.Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, testresult.StandardOut));
                //        }

                //        if (!string.IsNullOrEmpty(testresult.StandardError))
                //        {
                //            result.Messages.Add(new TestResultMessage(TestResultMessage.StandardErrorCategory, testresult.StandardError));
                //        }

                //        if (!string.IsNullOrEmpty(testresult.AdditionalInfo))
                //        {
                //            result.Messages.Add(new TestResultMessage(TestResultMessage.AdditionalInfoCategory, testresult.AdditionalInfo));
                //        }
                //        break;
                //}
            }

            _frameworkHandle.InformationalMessage($"Finished test: {test.FullyQualifiedName}");

            return result;
        }

    }
}
