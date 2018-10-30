//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel
{
    using System;

    internal class AdapterSettingsException : Exception
    {
        internal AdapterSettingsException(string message)
            : base(message)
        {
        }
    }
}
