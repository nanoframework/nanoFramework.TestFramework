//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using nanoFramework.TestFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace nanoFramework.TestPlatform.TestAdapter
{
    /// <summary>
    /// A Test Discoverer class
    /// </summary>
    [DefaultExecutorUri(TestsConstants.Executornano)]
    [FileExtension(".exe")]
    [FileExtension(".dll")]
    public class TestDiscoverer : ITestDiscoverer
    {
        private IMessageLogger _logger;
        private List<TestCase> _testCases;

        /// <inheritdoc/>
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _testCases = new List<TestCase>();

            _logger.SendMessage(TestMessageLevel.Informational, "Hello from nF DiscoverTests");
            foreach (var source in sources)
            {
                _logger.SendMessage(TestMessageLevel.Informational, $"  New file processed: {source}");
                if (!File.Exists(source))
                {
                    _logger.SendMessage(TestMessageLevel.Error, $"  File doesn't exist: {source}");
                    continue;
                }

                var cases = FindTestCases(source);
                if (cases.Count > 0)
                {
                    _logger.SendMessage(TestMessageLevel.Informational, $"  Adding {cases.Count} new tests");
                    _testCases.AddRange(cases);
                }
            }

            foreach (var testCase in _testCases)
            {
                discoverySink.SendTestCase(testCase);
            }            

            _logger.SendMessage(TestMessageLevel.Informational, "Finished adding files");
        }

        /// <summary>
        /// Find tests cases based on the source file
        /// </summary>
        /// <param name="source">the link on a file</param>
        /// <returns>A list of test cases</returns>
        public static List<TestCase> FindTestCases(string source)
        {
            List<TestCase> testCases = new List<TestCase>();

            var nfprojSources = FindNfprojSources(source);
            if (nfprojSources.Length == 0)
            {
                return testCases;
            }

            var allCsFils = GetAllCsFileNames(nfprojSources);

            Assembly test = Assembly.LoadFile(source);
            AppDomain.CurrentDomain.AssemblyResolve += App_AssemblyResolve;
            AppDomain.CurrentDomain.Load(test.GetName());

            Type[] allTypes = test.GetTypes();
            foreach (var type in allTypes)
            {
                if (type.IsClass)
                {
                    var typeAttribs = type.GetCustomAttributes(true);
                    foreach (var typeAttrib in typeAttribs)
                    {
                        if (typeof(TestClassAttribute).FullName == typeAttrib.GetType().FullName)
                        {
                            var methods = type.GetMethods();
                            // First we look at Setup
                            foreach (var method in methods)
                            {
                                var attribs = method.GetCustomAttributes(true);

                                foreach (var attrib in attribs)
                                {
                                    if (attrib.GetType().FullName == typeof(SetupAttribute).FullName ||
                                    attrib.GetType().FullName == typeof(TestMethodAttribute).FullName ||
                                    attrib.GetType().FullName == typeof(CleanupAttribute).FullName)
                                    {
                                        var testCase = GetFileNameAndLineNumber(allCsFils, type, method);
                                        testCase.Source = source;
                                        testCase.ExecutorUri = new Uri(TestsConstants.Executornano);
                                        testCase.FullyQualifiedName = $"{type.FullName}.{testCase.DisplayName}";
                                        testCase.Traits.Add(new Trait("Type", attrib.GetType().Name.Replace("Attribute","")));
                                        testCases.Add(testCase);
                                    }
                                }
                            }

                        }
                    }
                }
            }

            return testCases;
        }

        private static Assembly App_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Split(new[] { ',' })[0] + ".dll";
            string path = Path.GetDirectoryName(args.RequestingAssembly.Location);
            return Assembly.LoadFrom(Path.Combine(path, dllName));
        }

        private static string[] GetAllCsFileNames(FileInfo[] nfprojSources)
        {
            List<string> allCsFiles = new List<string>();
            foreach (var nfproj in nfprojSources)
            {
                var csFiles = Directory.GetFiles(Path.GetDirectoryName(nfproj.FullName), "*.cs", SearchOption.AllDirectories);
                // Get rid of those in /bin / obj
                var csFilesClean = csFiles.Where(m => !(m.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") || m.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")));
                allCsFiles.AddRange(csFilesClean);
            }

            return allCsFiles.ToArray();
        }

        private static FileInfo[] FindNfprojSources(string source)
        {
            if (Path.GetDirectoryName(source) == null)
            {
                return new FileInfo[0];
            }

            // Find all the potential *.cs files present at same level or above a nfproj file,
            // if no nfproj file, then we will skip this source
            var mainDirectory = new DirectoryInfo(Path.GetDirectoryName(source));
            var nfproj = mainDirectory.GetFiles("*.nfproj");
            if (nfproj.Length == 0)
            {
                var ret = FindNfprojSources(mainDirectory.Parent.FullName);
                return ret;
            }

            return nfproj;
        }

        private static TestCase GetFileNameAndLineNumber(string[] csFiles, Type className, MethodInfo method)
        {
            var clName = className.Name;
            var methodName = method.Name;
            TestCase flret = new TestCase();
            foreach (var csFile in csFiles)
            {
                StreamReader sr = new StreamReader(csFile);
                var allFile = sr.ReadToEnd();
                if (allFile.Contains($"class {clName}"))
                {
                    if (allFile.Contains($" {methodName}("))
                    {
                        // We found it!
                        int lineNum = 1;
                        foreach (var line in allFile.Split('\r'))
                        {
                            if (line.Contains($" {methodName}("))
                            {
                                flret.CodeFilePath = csFile;
                                flret.LineNumber = lineNum;
                                flret.DisplayName = method.Name;
                                return flret;
                            }

                            lineNum++;
                        }
                    }
                }
            }

            return flret;
        }
    }
}

