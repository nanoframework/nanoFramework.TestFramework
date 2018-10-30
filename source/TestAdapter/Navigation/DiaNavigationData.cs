//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;
    using System.Diagnostics.CodeAnalysis;
    
    /// <summary>
    /// A struct that stores the infomation needed by the navigation: file name, line number, column number.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Dia is a specific name.")]
    public class DiaNavigationData : INavigationData
    {
        public string FileName { get; set; }

        public int MinLineNumber { get; set; }

        public int MaxLineNumber { get; set; }

        public DiaNavigationData(string fileName, int minLineNumber, int maxLineNumber)
        {
            this.FileName = fileName;
            this.MinLineNumber = minLineNumber;
            this.MaxLineNumber = maxLineNumber;
        }
    }

}