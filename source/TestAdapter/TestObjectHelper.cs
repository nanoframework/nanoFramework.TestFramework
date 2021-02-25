//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System.IO;
using System.Reflection;

namespace nanoFramework.TestPlatform.TestAdapter
{
    public static class TestObjectHelper
    {

        public static string GetTestPath(string testToRun, string ext)
        {
            const string TestsDirName = "Tests";
            var thisAssemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Check if we have Debug or Release
            string buildType = thisAssemblyDir.Contains("Release") ? "Release" : "Debug";
            // Find the root Tests folder
            var testIdx = thisAssemblyDir.IndexOf(TestsDirName);
            string rootTests = thisAssemblyDir.Substring(0, testIdx + TestsDirName.Length);
            // Find all directories under this one
            var files = Directory.GetFiles(rootTests, $"{testToRun}.{ext}", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.EndsWith(Path.Combine("bin", buildType, $"{testToRun}.{ext}")))
                {
                    return file;
                }
            }

            return string.Empty;
        }

        public static string GetNanoClrLocation()
        {
            var thisAssemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var nanoClrFullPath = Path.Combine(thisAssemblyDir, "nanoFramework.nanoCLR.exe");

            return nanoClrFullPath;
        }
    }
}
