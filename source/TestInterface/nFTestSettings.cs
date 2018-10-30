//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System.Xml;

namespace nanoFramework.TestPlatform.TestInterface
{
#pragma warning disable IDE1006 // Naming Styles // following project name with lower case
    public class nFTestSettings
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        /// The settings name
        /// </summary>
        public const string SettingsName = "NanoFrameworkAdapter";


        #region properties

        public bool Disabled { get; private set; }

        #endregion


        /// <summary>
        /// Initializes a new instance of <see cref="nFSettings"/> class.
        /// </summary>
        public nFTestSettings()
        {
            Disabled = false;
        }

        public static nFTestSettings Extract(XmlNode node)
        {
            nFTestSettings settings = new nFTestSettings();

            // Make sure we have the correct node, and extract settings
            if (node.Name == SettingsName)
            {
                // Check if test adapter is disabled
                var disabled = node.Attributes["disabled"]?.Value;
                if (disabled != null && TestInterface.RegExHelper.Regex_TrueFalse.IsMatch(disabled))
                {
                    settings.Disabled = TestInterface.RegExHelper.Regex_TrueFalse.IsMatch(disabled);
                }

                if (settings.Disabled)
                {
                    // is disabled, return now, don't bother with parsing the rest of it
                    return settings;
                }
            }

            return settings;
        }

    }
}
