
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System.Xml;

namespace nanoFramework.TestPlatform.TestAdapter
{
    [SettingsName(TestsConstants.SettingsName)]
    public class SettingsProvider : ISettingsProvider
    {
        #region Properties

        public Settings Settings { get; private set; }

        #endregion // Properties

        #region ISettingsProvider

        public void Load(XmlReader reader)
        {
            var xml = new XmlDocument();
            reader.Read();
            Settings = Settings.Extract(xml.ReadNode(reader));
        }

        #endregion // ISettingsProvider
    }
}
