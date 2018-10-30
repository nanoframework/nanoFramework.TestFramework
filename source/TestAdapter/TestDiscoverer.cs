//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.Discovery;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    [FileExtension(".exe")]
    [FileExtension(".dll")]
    [DefaultExecutorUri(Constants.NFExecutorUriString)]
    public class TestDiscoverer : ITestDiscoverer
    {
        /// <summary>
        /// ITestDiscover, Given a list of test sources this method pulls out the test cases
        /// </summary>
        /// <param name="sources">List of test sources passed from client (Client can be VS or command line)</param>
        /// <param name="discoveryContext">Context and runSettings for current run.  Discoverer pulls out the tests based on current context</param>
        /// <param name="logger">Used to relay messages to registered loggers</param>
        /// <param name="discoverySink">Callback used to notify client upon discovery of test cases</param>
        public void DiscoverTests(
            IEnumerable<string> sources, 
            IDiscoveryContext discoveryContext, 
            IMessageLogger logger, 
            ITestCaseDiscoverySink discoverySink)
        {
            ValidateArg.NotNull(sources, "sources");
            ValidateArg.NotNull(discoverySink, "discoverySink");
            ValidateArg.NotNull(logger, "logger");

            // Populate the runsettings.
            try
            {
                MSTestSettings.PopulateSettings(discoveryContext);
            }
            catch (AdapterSettingsException ex)
            {
                logger.SendMessage(TestMessageLevel.Error, ex.Message);
                return;
            }

            new UnitTestDiscoverer().DiscoverTests(sources, logger, discoverySink, discoveryContext);
        }

        /// <summary>
        /// Verifies if the sources are valid for the target platform.
        /// </summary>
        /// <param name="sources">The test sources</param>
        /// <remarks>Sources cannot be null.</remarks>
        /// <returns>True if the source has a valid extension for the current platform.</returns>
        internal bool AreValidSources(IEnumerable<string> sources)
        {
            // ValidSourceExtensions is always expected to return a non-null list.
            return
                sources.Any(
                    source =>
                    PlatformServiceProvider.Instance.TestSource.ValidSourceExtensions.Any(
                        extension =>
                        string.Compare(Path.GetExtension(source), extension, StringComparison.OrdinalIgnoreCase) == 0));
        }
    }
}
