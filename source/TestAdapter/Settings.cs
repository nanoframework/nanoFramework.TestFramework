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
    /// Settings for .NET nanoFramework Test Adapter.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// True to run the tests on real hardware.
        /// </summary>
        public bool IsRealHardware { get; set; } = false;

        /// <summary>
        /// The serial port number to run the tests on a real hardware.
        /// </summary>
        public string RealHardwarePort { get; set; } = string.Empty;

        /// <summary>
        /// Path to a local nanoCLR instance to use to run Unit Tests.
        /// </summary>
        public string PathToLocalCLRInstance { get; set; } = string.Empty;

        /// <summary>
        /// Version of nanoCLR instance to use when running Unit Tests.
        /// </summary>
        public string CLRVersion { get; set; } = string.Empty;

        /// <summary>
        /// Level of logging for Unit Test execution.
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
                var isrealhard = node.SelectSingleNode(nameof(IsRealHardware))?.FirstChild;
                if (isrealhard != null && isrealhard.NodeType == XmlNodeType.Text)
                {
                    settings.IsRealHardware = isrealhard.Value.ToLower() == "true" ? true : false;
                }

                var realhardport = node.SelectSingleNode(nameof(RealHardwarePort))?.FirstChild;
                if (realhardport != null && realhardport.NodeType == XmlNodeType.Text)
                {
                    settings.RealHardwarePort = realhardport.Value;
                }

                var loggingLevel = node.SelectSingleNode(nameof(Logging))?.FirstChild;
                if (loggingLevel != null && loggingLevel.NodeType == XmlNodeType.Text)
                {
                    if (Enum.TryParse(loggingLevel.Value, out LoggingLevel logging))
                    {
                        settings.Logging = logging;
                    }
                }

                var clrversion = node.SelectSingleNode(nameof(CLRVersion))?.FirstChild;
                if (clrversion != null && clrversion.NodeType == XmlNodeType.Text)
                {
                    settings.CLRVersion = clrversion.Value;
                }

                var pathtolocalclrinstance = node.SelectSingleNode(nameof(PathToLocalCLRInstance))?.FirstChild;
                if (pathtolocalclrinstance != null && pathtolocalclrinstance.NodeType == XmlNodeType.Text)
                {
                    settings.PathToLocalCLRInstance = pathtolocalclrinstance.Value;
                }
            }

            return settings;
        }

        /// <summary>
        /// The log level.
        /// </summary>
        public enum LoggingLevel
        {
            None = 0,

            Detailed = 1,

            Verbose = 2,

            Error = 3
        }
    }
}
