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
using System.Text.RegularExpressions;

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

            foreach (var sourceFile in sources)
            {
                _logger.LogMessage(
                    $"  New file processed: {sourceFile}",
                    Settings.LoggingLevel.Detailed);

                if (!File.Exists(sourceFile))
                {
                    _logger.LogMessage(
                        $"  File doesn't exist: {sourceFile}",
                        Settings.LoggingLevel.Detailed);

                    continue;
                }

                var cases = ComposeTestCases(sourceFile);
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
        /// Compose tests cases for the Unit Test assembly.
        /// </summary>
        /// <param name="sourceFile">Path to the assembly file containing the Unit Tests.</param>
        /// <returns>A list of <see cref="TestCase"/>.</returns>
        public static List<TestCase> ComposeTestCases(string sourceFile)
        {
            List<TestCase> collectionOfTestCases = new List<TestCase>();

            // try to find nfproj file for this unit test assembly
            var nfprojFile = FindNfprojFile(sourceFile);

            if (!nfprojFile.Any())
            {
                return collectionOfTestCases;
            }

            var allCsFiles = GetAllCsFiles(nfprojFile);

            // developer note: we have to use LoadFile() and not Load() which loads the assembly into the caller domain
            Assembly test = Assembly.LoadFile(sourceFile);
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

                            var testCase = BuildTestCaseFromSourceFile(
                                allCsFiles,
                                typeCandidate,
                                method);

                            testCase.Source = sourceFile;
                            testCase.ExecutorUri = new Uri(TestsConstants.NanoExecutor);
                            testCase.FullyQualifiedName = $"{typeCandidate.FullName}.{method.Name}.{i}";
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

        private static string[] GetAllCsFiles(FileInfo[] nfprojFiles)
        {
            List<string> allCsFiles = new List<string>();

            foreach (var nfproj in nfprojFiles)
            {
                // read nfproj file content
                var nfprojContent = File.ReadAllText(nfproj.FullName);

                // get all Compile items from the project file
                string compilePattern = "(?><Compile Include=\")(?<source_file>.+)(?>\")";
                var compileItems = Regex.Matches(nfprojContent, compilePattern, RegexOptions.IgnoreCase);

                foreach (System.Text.RegularExpressions.Match compileItem in compileItems)
                {
                    allCsFiles.Add($"{Path.GetFullPath(nfproj.DirectoryName)}\\{compileItem.Groups["source_file"].Value}");
                }
            }

            return allCsFiles.ToArray();
        }

        private static FileInfo[] FindNfprojFile(string source)
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

                // iterate through the parent folders until an nfproj file is found
                var mainDirectory = new DirectoryInfo(Path.GetDirectoryName(source));

                FileInfo[] nfproj = mainDirectory?.GetFiles("*.nfproj");

                if (nfproj.Length == 0
                    && mainDirectory?.Parent != null)
                {
                    return FindNfprojFile(mainDirectory?.Parent.FullName);
                }

                return nfproj;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"Exception raised when finding NF project file: '{ex}' searching for {source}");
            }
        }

        private static TestCase BuildTestCaseFromSourceFile(
            string[] csFiles,
            Type className,
            MethodInfo method)
        {
            TestCase testCase = new TestCase();

            foreach (var sourceFile in csFiles)
            {
                var fileContent = File.ReadAllText(sourceFile);

                if (!fileContent.Contains($"class {className.Name}"))
                {
                    continue;
                }

                if (!fileContent.Contains($" {method.Name}("))
                {
                    continue;
                }

                // We've found the file
                int lineNumber = 1;

                foreach (var line in fileContent.Split('\r'))
                {
                    if (line.Contains($" {method.Name}("))
                    {
                        testCase.CodeFilePath = sourceFile;
                        testCase.LineNumber = lineNumber;
                        testCase.DisplayName = method.Name;

                        return testCase;
                    }

                    lineNumber++;
                }
            }

            return testCase;
        }
    }
}
