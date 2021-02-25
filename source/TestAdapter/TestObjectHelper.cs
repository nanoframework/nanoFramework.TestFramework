//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System.IO;
using System.Reflection;

namespace nanoFramework.TestPlatform.TestAdapter
{
    /// <summary>
    /// Test object helper to find path
    /// </summary>
    public static class TestObjectHelper
    {
        /// <summary>
        /// Get the execution directory for the nanoCLR.exe
        /// </summary>
        /// <returns></returns>
        public static string GetNanoClrLocation()
        {
            var thisAssemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var nanoClrFullPath = Path.Combine(thisAssemblyDir, "nanoFramework.nanoCLR.exe");

            return nanoClrFullPath;
        }
    }
}
