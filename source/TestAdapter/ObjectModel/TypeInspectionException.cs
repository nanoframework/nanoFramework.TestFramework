//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel
{
    using System;

    /// <summary>
    /// Internal class to indicate type inspection failure
    /// </summary>
    [Serializable]
    internal class TypeInspectionException : Exception
    {
        public TypeInspectionException()
            : base()
        {
        }

        public TypeInspectionException(string message)
            : base(message)
        {
        }

        public TypeInspectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
