using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (isrealhard != null && timeout.NodeType == XmlNodeType.Text)
                {
                    settings.IsRealHarware = isrealhard.Value.ToLower() == "true" ? true : false;
                }

                var realhardport = node.SelectSingleNode(nameof(RealHarwarePort))?.FirstChild;
                if (realhardport != null && timeout.NodeType == XmlNodeType.Text)
                {
                    settings.RealHarwarePort = realhardport.Value;
                }
            }

            return settings;
        }
    }
}
