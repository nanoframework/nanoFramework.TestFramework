//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using nanoFramework.TestAdapter;
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
    [DefaultExecutorUri(TestsConstants.NanoExecutor)]
    [FileExtension(".exe")]
    [FileExtension(".dll")]
    public class TestDiscoverer : ITestDiscoverer
    {
        private LogMessenger _logger;
        private List<TestCase> _testCases;

        /// <inheritdoc/>
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            _testCases = new List<TestCase>();

            var settingsProvider = discoveryContext.RunSettings.GetSettings(TestsConstants.SettingsName) as SettingsProvider;

            _logger = new LogMessenger(
                logger,
                settingsProvider);

            if (settingsProvider != null)
            {
                _logger.LogMessage(
                    "Getting ready to discover tests...",
                    Settings.LoggingLevel.Detailed);

                _logger.LogMessage(
                    "Settings parsed",
                    Settings.LoggingLevel.Verbose);
            }
            else
            {
                _logger.LogMessage(
                    "Getting ready to discover tests...",
                    Settings.LoggingLevel.Detailed);

                _logger.LogMessage(
                    "No settings for nanoFramework adapter",
                    Settings.LoggingLevel.Verbose);
            }

            foreach (var source in sources)
            {
                _logger.LogMessage(
                    $"  New file processed: {source}",
                    Settings.LoggingLevel.Detailed);

                if (!File.Exists(source))
                {
                    _logger.LogMessage(
                        $"  File doesn't exist: {source}",
                        Settings.LoggingLevel.Detailed);

                    continue;
                }

                var cases = FindTestCases(source);
                if (cases.Count > 0)
                {
                    _logger.LogMessage(
                        $"  Adding {cases.Count} new tests",
                        Settings.LoggingLevel.Detailed);

                    _testCases.AddRange(cases);
                }
            }

            foreach (var testCase in _testCases)
            {
                discoverySink.SendTestCase(testCase);
            }

            _logger.LogMessage(
                "Finished adding files",
                Settings.LoggingLevel.Detailed);
        }

        /// <summary>
        /// Find tests cases based on the source file
        /// </summary>
        /// <param name="source">the link on a file</param>
        /// <returns>A list of test cases</returns>
        public static List<TestCase> FindTestCases(string source)
        {
            List<TestCase> collectionOfTestCases = new List<TestCase>();

            var nfprojSources = FindNfprojSources(source);
            if (nfprojSources.Length == 0)
            {
                return collectionOfTestCases;
            }

            var allCsFiles = GetAllCsFileNames(nfprojSources);

            // developer note: we have to use LoadFile() and not Load() which loads the assembly into the caller domain
            Assembly test = Assembly.LoadFile(source);
            AppDomain.CurrentDomain.AssemblyResolve += App_AssemblyResolve;
            AppDomain.CurrentDomain.Load(test.GetName());

            var typeCandidatesForTests = test.GetTypes()
                                            .Where(x => x.IsClass);

            foreach (var typeCandidate in typeCandidatesForTests)
            {
                var testClasses = typeCandidate.GetCustomAttributes(true)
                                      .Where(x => x.GetType().FullName == typeof(TestClassAttribute).FullName);

                foreach (var testClassAttrib in testClasses)
                {
                    var methods = typeCandidate.GetMethods();

                    // First we look at Setup
                    foreach (var method in methods)
                    {
                        var methodAttribs = method.GetCustomAttributes(true);
                        methodAttribs = Helper.RemoveTestMethodIfDataRowExists(methodAttribs);

                        var testMethodsToItterate = methodAttribs.Where(x => IsTestMethod(x)).ToArray();

                        for (int i = 0; i < testMethodsToItterate.Length; i++)
                        {
                            var testMethodAttrib = testMethodsToItterate[i];
                            var testCase = GetFileNameAndLineNumber(
                                allCsFiles,
                                typeCandidate,
                                method,
                                testMethodAttrib,
                                i);

                            testCase.Source = source;
                            testCase.ExecutorUri = new Uri(TestsConstants.NanoExecutor);
                            testCase.FullyQualifiedName = $"{typeCandidate.FullName}.{testCase.DisplayName}";
                            testCase.Traits.Add(new Trait("Type", testMethodAttrib.GetType().Name.Replace("Attribute", "")));

                            collectionOfTestCases.Add(testCase);
                        }
                    }
                }
            }

            return collectionOfTestCases;
        }

        private static bool IsTestMethod(object attrib)
        {
            var attributeName = attrib.GetType().FullName;

            if (attributeName == typeof(SetupAttribute).FullName)
            {
                return true;
            }

            if (attributeName == typeof(TestMethodAttribute).FullName)
            {
                return true;
            }

            if (attributeName == typeof(CleanupAttribute).FullName)
            {
                return true;
            }

            if (attributeName == typeof(DataRowAttribute).FullName)
            {
                return true;
            }

            return false;
        }

        private static Assembly App_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                string dllName = args.Name.Split(new[] { ',' })[0] + ".dll";
                string path = Path.GetDirectoryName(args.RequestingAssembly.Location);
                return Assembly.LoadFrom(Path.Combine(path, dllName));
            }
            catch 
            {
                // this is called on several occasions, some are not related with our types or assemblies
                // therefore there are calls that can't be resolved and that's OK
                return null;
            }
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
            if (string.IsNullOrEmpty(source))
            {
                return new FileInfo[0];
            }

            try
            {
                if (Path.GetDirectoryName(source) == null)
                {
                    return new FileInfo[0];
                }

                // Find all the potential *.cs files present at same level or above a nfproj file,
                // if no nfproj file, then we will skip this source
                var mainDirectory = new DirectoryInfo(Path.GetDirectoryName(source));

                FileInfo[] nfproj = mainDirectory?.GetFiles("*.nfproj");

                if (nfproj.Length == 0
                    && mainDirectory?.Parent != null)
                {
                    return FindNfprojSources(mainDirectory?.Parent.FullName);
                }

                return nfproj;
            }
            catch(Exception ex)
            {
                throw new FileNotFoundException($"Exception raised when finding NF project sources: '{ex}' searching for {source}");
            }
        }

        private static TestCase GetFileNameAndLineNumber(
            string[] csFiles,
            Type className,
            MethodInfo method,
            object attribute,
            int attributeIndex)
        {
            TestCase testCase = new TestCase();

            foreach (var csFile in csFiles)
            {
                using (StreamReader sr = new StreamReader(csFile))
                {
                    var fileContent = sr.ReadToEnd();

                    if (!fileContent.Contains($"class {className.Name}"))
                    {
                        continue;
                    }

                    if (!fileContent.Contains($" {method.Name}("))
                    {
                        continue;
                    }

                    // We found it!
                    int lineNumber = 1;

                    foreach (var line in fileContent.Split('\r'))
                    {
                        if (line.Contains($" {method.Name}("))
                        {
                            testCase.CodeFilePath = csFile;
                            testCase.LineNumber = lineNumber;
                            testCase.DisplayName = Helper.GetTestDisplayName(
                                method,
                                attribute,
                                attributeIndex);

                            return testCase;
                        }

                        lineNumber++;
                    }
                }
            }

            return testCase;
        }
    }
}
