//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.Execution;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel;
    using Newtonsoft.Json;

    [ExtensionUri(TestAdapter.Constants.NFExecutorUriString)]
    public class TestExecutor : ITestExecutor
    {
        /// <summary>
        /// Token for cancelling the test run.
        /// </summary>
        private TestRunCancellationToken cancellationToken = null;

        /// <summary>
        /// Gets or sets the ms test execution manager.
        /// </summary>
        public TestExecutionManager TestExecutionManager { get; protected set; }

        /// <summary>
        /// Gets discoverer used for validating the sources.
        /// </summary>
        private TestDiscoverer TestDiscoverer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestExecutor"/> class.
        /// </summary>
        public TestExecutor()
        {
            TestExecutionManager = new TestExecutionManager();
            TestDiscoverer = new TestDiscoverer();
        }

        public void RunTests(
            IEnumerable<TestCase> tests,
            IRunContext runContext,
            IFrameworkHandle frameworkHandle)
        {
            ValidateArg.NotNull(frameworkHandle, "frameworkHandle");
            ValidateArg.NotNullOrEmpty(tests, "tests");

            if (!this.TestDiscoverer.AreValidSources(from test in tests select test.Source))
            {
                throw new NotSupportedException();
            }

            // Populate the runsettings.
            try
            {
                MSTestSettings.PopulateSettings(runContext);
            }
            catch (AdapterSettingsException ex)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, ex.Message);
                return;
            }

            this.cancellationToken = new TestRunCancellationToken();
            this.TestExecutionManager.RunTests(tests, runContext, frameworkHandle, this.cancellationToken);
            this.cancellationToken = null;
        }


        public void RunTests(
            IEnumerable<string> sources, 
            IRunContext runContext, 
            IFrameworkHandle frameworkHandle)
        {
            ValidateArg.NotNull(frameworkHandle, "frameworkHandle");
            ValidateArg.NotNullOrEmpty(sources, "sources");

            if (!this.TestDiscoverer.AreValidSources(sources))
            {
                throw new NotSupportedException();
            }

            // Populate the runsettings.
            try
            {
                MSTestSettings.PopulateSettings(runContext);
            }
            catch (AdapterSettingsException ex)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, ex.Message);
                return;
            }

            sources = PlatformServiceProvider.Instance.TestSource.GetTestSources(sources);
            this.cancellationToken = new TestRunCancellationToken();
            this.TestExecutionManager.RunTests(sources, runContext, frameworkHandle, this.cancellationToken);

            this.cancellationToken = null;
        }

        public void Cancel()
        {
            this.cancellationToken?.Cancel();
        }
    }
}
