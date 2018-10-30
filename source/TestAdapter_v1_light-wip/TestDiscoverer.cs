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

namespace nanoFramework.TestPlatform.TestAdapter
{
    [DefaultExecutorUri(TestInterface.Constants.NFExecutorUriString)]
    [FileExtension(".exe")]
    [FileExtension(".dll")]
    public class TestDiscoverer : ITestDiscoverer
    {
        private IDiscoveryContext _discoveryContext = null;
        private IMessageLogger _logger = null;
        private ITestCaseDiscoverySink _discoverySink = null;
        private nFTestSettings _settings = new nFTestSettings();

        #region ITestDiscoverer

        public void DiscoverTests(
            IEnumerable<string> sources, 
            IDiscoveryContext discoveryContext, 
            IMessageLogger logger, 
            ITestCaseDiscoverySink discoverySink
            )
        {
            _discoveryContext = discoveryContext ?? throw new ArgumentNullException(nameof(discoveryContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _discoverySink = discoverySink ?? throw new ArgumentNullException(nameof(discoverySink));

            logger.SendMessage(TestMessageLevel.Informational, "Hello from nF DiscoverTests");

            //// Retrieve nanoFramework specific settings
            //if (!PopulateSettings())
            //{
            //    _logger.ErrorMessage(StringResources.SettingsMissingDiscoveryWarning);
            //    return;
            //}

            // Check if adapter is disabled
            if (_settings.Disabled)
            {
                _logger.InformationalMessage(StringResources.TestAdapterDisabled);
                return;
            }

            // start discovery
            _logger.InformationalMessage(StringResources.StartingDiscovery);

            DiscoverTests(sources);
            
            // done with discovery
            _logger.InformationalMessage(StringResources.DiscoveryCompleted);
        }

        #endregion


        private void DiscoverTests(IEnumerable<string> sources)
        {
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
                _discoverySink.SendTestCase(test.ToVSTestCase());
                //LogDebug(TestMessageLevel.Informational, $"  {testcase.Name}");
            }
            //LogDebug(TestMessageLevel.Informational, "Finished adding test cases to discovery sink");
        }

        private bool PopulateSettings()
        {
            // Populate our test setting with whatever is coming in the runsettings

            var settingsprovider = _discoveryContext?.RunSettings?.GetSettings(nFTestSettings.SettingsName) as SettingsProvider;

            _settings = settingsprovider?.nFTestSettings;

            return _settings != null;
        }
    }
}
