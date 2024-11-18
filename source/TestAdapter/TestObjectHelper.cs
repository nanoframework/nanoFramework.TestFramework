// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Linq;
using System.Reflection;

namespace nanoFramework.TestPlatform.TestAdapter
{
    /// <summary>
    /// Test object helper to find path
    /// </summary>
    public static class TestObjectHelper
    {
        private const string NanoClrName = "nanoFramework.nanoCLR.exe";
        /// <summary>
        /// Get the execution directory for the nanoCLR.exe
        /// </summary>
        /// <returns></returns>
        public static string GetNanoClrLocation()
        {
            var thisAssemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var nanoClrFullPath = Path.Combine(thisAssemblyDir, NanoClrName);
            if (File.Exists(nanoClrFullPath))
            {
                return nanoClrFullPath;
            }

            var inititialDir = new DirectoryInfo(Path.GetDirectoryName(thisAssemblyDir));
            return FindNanoClr(inititialDir);
        }

        private static string FindNanoClr(DirectoryInfo initialPath)
        {
            var dir = initialPath.Parent;
            if (dir != null)
            {
                var findnanoClr = dir.GetFiles(NanoClrName, SearchOption.AllDirectories);
                if (findnanoClr.Any())
                {
                    return findnanoClr.First().FullName;
                }
                return FindNanoClr(dir);
            }

            throw new FileNotFoundException($"Unable to find nanoCLR.");
        }
    }
}
