//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions
{
    using nanoFramework.TestPlatform.MSTest.TestAdapter.Interfaces;
    using System.Reflection;

    /// <inheritdoc/>
    public class PlatformAssemblyLoadContext : IAssemblyLoadContext
    {
        /// <inheritdoc/>
        public AssemblyName GetAssemblyNameFromPath(string assemblyPath)
        {
            return AssemblyName.GetAssemblyName(assemblyPath);
        }

        public Assembly LoadAssemblyFromPath(string assemblyPath)
        {
            return Assembly.LoadFrom(assemblyPath);
        }
    }
}
