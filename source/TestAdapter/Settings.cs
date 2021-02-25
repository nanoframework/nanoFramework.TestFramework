//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Xml;

namespace nanoFramework.TestPlatform.TestAdapter
{
    /// <summary>
    /// Settings for the nanoFramweork tests
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// How long maximum the tests can run.
        /// </summary>
        /// <remarks>Make sure to adjust the default value in .runsettings first</remarks>
        public int TestTimeOutSeconds { get; set; } = 60;

        /// <summary>
        /// True to run the tests on real hardware
        /// </summary>
        public bool IsRealHarware { get; set; } = false;

        /// <summary>
        /// The serial port number to run the tests on a real hardware
        /// </summary>
        public string RealHarwarePort { get; set; } = string.Empty;

        /// <summary>
        /// Level of logging for test execution.
        /// </summary>
        public LoggingLevel Logging { get; set; } = LoggingLevel.None;

        /// <summary>
        /// Get settings from an XML node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Settings Extract(XmlNode node)
        {
            Settings settings = new Settings();

            if (node.Name == TestsConstants.SettingsName)
            {
                var timeout = node.SelectSingleNode(nameof(TestTimeOutSeconds))?.FirstChild;
                if (timeout != null && timeout.NodeType == XmlNodeType.Text)
                {
                    if (int.TryParse(timeout.Value, out int timeoutNum))
                    {
                        settings.TestTimeOutSeconds = timeoutNum;
                    }
                }

                var isrealhard = node.SelectSingleNode(nameof(IsRealHarware))?.FirstChild;
                if (isrealhard != null && isrealhard.NodeType == XmlNodeType.Text)
                {
                    settings.IsRealHarware = isrealhard.Value.ToLower() == "true" ? true : false;
                }

                var realhardport = node.SelectSingleNode(nameof(RealHarwarePort))?.FirstChild;
                if (realhardport != null && realhardport.NodeType == XmlNodeType.Text)
                {
                    settings.RealHarwarePort = realhardport.Value;
                }

                var loggingLevel = node.SelectSingleNode(nameof(Logging))?.FirstChild;
                if (loggingLevel != null && loggingLevel.NodeType == XmlNodeType.Text)
                {
                    if (Enum.TryParse(loggingLevel.Value, out LoggingLevel logging))
                    {
                        settings.Logging = logging;
                    }
                }
            }

            return settings;
        }

        public enum LoggingLevel
        {
            None = 0,

            Detailed = 1,

            Verbose = 2,

            Error = 3
        }
    }
}
