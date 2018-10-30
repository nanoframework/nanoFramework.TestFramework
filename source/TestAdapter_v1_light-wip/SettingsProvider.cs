//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using nanoFramework.TestPlatform.TestInterface;
using System.Xml;

namespace nanoFramework.TestPlatform.TestAdapter
{
    [SettingsName(nFTestSettings.SettingsName)]
    public class SettingsProvider : ISettingsProvider
    {
        public nFTestSettings nFTestSettings { get; private set; }

        #region ISettingsProvider

        public void Load(XmlReader reader)
        {
            var xml = new XmlDocument();
            reader.Read();
            nFTestSettings = nFTestSettings.Extract(xml.ReadNode(reader));
        }

        #endregion

    }
}
