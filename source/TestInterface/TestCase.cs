//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System.Collections.Generic;

namespace nanoFramework.TestPlatform.TestInterface
{
    // This class is a replacement for Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase.
    // The goal is to have the TestInterface assembly independent from Microsoft.TestPlatform.ObjectModel package.
    public class TestCase
    {
        public string Name { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public int Line { get; set; } = -1;
        public List<string> Tags { get; set; } = new List<string>();
    }
}