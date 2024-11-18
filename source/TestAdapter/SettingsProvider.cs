// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace nanoFramework.TestPlatform.TestAdapter
{
    /// <summary>
    /// Setting Provider class
    /// </summary>
    [SettingsName(TestsConstants.SettingsName)]
    public class SettingsProvider : ISettingsProvider
    {
        #region Properties

        /// <summary>
        /// Settings
        /// </summary>
        public Settings Settings { get; private set; }

        #endregion // Properties

        /// <summary>
        /// Loading the XML elements
        /// </summary>
        /// <param name="reader"></param>
        public void Load(XmlReader reader)
        {
            var xml = new XmlDocument();
            reader.Read();
            Settings = Settings.Extract(xml.ReadNode(reader));
        }
    }
}
