//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System.Xml;

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
