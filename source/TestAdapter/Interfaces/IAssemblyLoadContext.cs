//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.Interfaces
{
    using System.Reflection;

    /// <summary>
    /// Abstraction for Assembly Methods
    /// </summary>
    public interface IAssemblyLoadContext
    {
        /// <summary>
        /// Loads assembly from given path
        /// </summary>
        /// <param name="assemblyPath">Assembly path</param>
        /// <returns>Assembly from given path</returns>
        Assembly LoadAssemblyFromPath(string assemblyPath);

        /// <summary>
        /// Gets Assembly Name from given path
        /// </summary>
        /// <param name="assemblyPath">Assembly path</param>
        /// <returns>AssemblyName from given path</returns>
        AssemblyName GetAssemblyNameFromPath(string assemblyPath);
    }
}
