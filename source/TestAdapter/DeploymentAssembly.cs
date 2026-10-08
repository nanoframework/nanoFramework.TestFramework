// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.TestPlatform.TestAdapter
{
    public class DeploymentAssembly
    {
        /// <summary>
        /// Path to the PE file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Assembly version, as stored in the PE header.
        /// </summary>
        public string Version { get; set; }

        public DeploymentAssembly(string path, string version)
        {
            Path = path;
            Version = version;
        }
    }
}