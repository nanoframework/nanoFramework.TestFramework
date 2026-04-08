// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using nanoFramework.TestAdapter;
using nanoFramework.TestFramework;

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

                var cases = ComposeTestCases(sourceFile, logger);
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
        public static List<TestCase> ComposeTestCases(string sourceFile, IMessageLogger logger = null)
        {
            List<TestCase> collectionOfTestCases = new List<TestCase>();

            try
            {
                // Skip core library assemblies — they can't be reflected into by the desktop CLR
                // and will never contain test classes.
                string sourceFileName = Path.GetFileName(sourceFile);
                if (sourceFileName.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase))
                {
                    logger?.SendMessage(
                        TestMessageLevel.Informational,
                        $"  Skipping core library: {sourceFile}");

                    return collectionOfTestCases;
                }

                // try to find project file for this unit test assembly (.nfproj or .csproj)
                var projectFile = FindProjectFile(sourceFile);

                if (!projectFile.Any())
                {
                    logger?.SendMessage(
                        TestMessageLevel.Informational,
                        $"  No project file found for: {sourceFile}");

                    return collectionOfTestCases;
                }

                logger?.SendMessage(
                    TestMessageLevel.Informational,
                    $"  Found project file: {projectFile.First().FullName}");

                var allCsFiles = GetAllCsFiles(projectFile);

                logger?.SendMessage(
                    TestMessageLevel.Informational,
                    $"  Found {allCsFiles.Length} source files");

                // Load assembly from a byte array to avoid locking the file on disk.
                // This prevents MSBuild copy errors when rebuilding while VS test discovery has loaded the DLL.
                string sourceDir = Path.GetDirectoryName(sourceFile);

                // Register resolve handler BEFORE loading so dependencies are found during load.
                // Byte-loaded assemblies have empty Location, so we capture sourceDir in the closure.
                ResolveEventHandler resolveHandler = (sender, args) =>
                {
                    try
                    {
                        string assemblyName = args.Name.Split(new[] { ',' })[0];

                        // Never load nanoFramework's mscorlib into the desktop CLR.
                        // The CLR will unify mscorlib references to its own version,
                        // which lets nanoFramework types (System.Object, System.Attribute, etc.)
                        // resolve against the real desktop types.
                        if (assemblyName.Equals("mscorlib", StringComparison.OrdinalIgnoreCase))
                        {
                            return null;
                        }

                        string dllName = assemblyName + ".dll";
                        string candidatePath = Path.Combine(sourceDir, dllName);

                        if (File.Exists(candidatePath))
                        {
                            return Assembly.Load(File.ReadAllBytes(candidatePath));
                        }
                    }
                    catch
                    {
                        // not our assembly, ignore
                    }

                    return null;
                };

                AppDomain.CurrentDomain.AssemblyResolve += resolveHandler;

                try
                {
                byte[] assemblyBytes = File.ReadAllBytes(sourceFile);
                Assembly test = Assembly.Load(assemblyBytes);

                logger?.SendMessage(
                    TestMessageLevel.Informational,
                    $"  Assembly loaded: {test.FullName}");

                Type[] allTypes;
                try
                {
                    allTypes = test.GetTypes();
                }
                catch (ReflectionTypeLoadException rtle)
                {
                    logger?.SendMessage(
                        TestMessageLevel.Warning,
                        $"  GetTypes() partial load — {rtle.LoaderExceptions?.Length ?? 0} loader exceptions");

                    foreach (var lex in rtle.LoaderExceptions ?? Array.Empty<Exception>())
                    {
                        logger?.SendMessage(
                            TestMessageLevel.Warning,
                            $"    {lex?.Message}");
                    }

                    // Use the types that did load successfully
                    allTypes = rtle.Types.Where(t => t != null).ToArray();
                }

                var typeCandidatesForTests = allTypes.Where(x => x.IsClass);

                logger?.SendMessage(
                    TestMessageLevel.Informational,
                    $"  Found {allTypes.Length} types, {typeCandidatesForTests.Count()} classes");

            foreach (var typeCandidate in typeCandidatesForTests)
            {
                object[] attrs;
                try
                {
                    attrs = typeCandidate.GetCustomAttributes(true);
                }
                catch (Exception attrEx)
                {
                    logger?.SendMessage(
                        TestMessageLevel.Warning,
                        $"  GetCustomAttributes failed for {typeCandidate.FullName}: {attrEx.Message}");

                    continue;
                }

                var testClasses = attrs
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
                }
                finally
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= resolveHandler;
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                logger?.SendMessage(
                    TestMessageLevel.Warning,
                    $"  ReflectionTypeLoadException for {sourceFile}: {ex.Message}");

                foreach (var loaderEx in ex.LoaderExceptions ?? Array.Empty<Exception>())
                {
                    logger?.SendMessage(
                        TestMessageLevel.Warning,
                        $"    Loader exception: {loaderEx?.Message}");
                }
            }
            catch (Exception ex)
            {
                logger?.SendMessage(
                    TestMessageLevel.Warning,
                    $"  Exception discovering tests in {sourceFile}: {ex}");
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

        private static string[] GetAllCsFiles(FileInfo[] projectFiles)
        {
            List<string> allCsFiles = new List<string>();

            foreach (var projectFile in projectFiles)
            {
                // read project file content
                var projectContent = File.ReadAllText(projectFile.FullName);

                // get all Compile items from the project file
                string compilePattern = "<Compile Include=\"(?<source_file>[^\"]+)\"";
                var compileItems = Regex.Matches(projectContent, compilePattern, RegexOptions.IgnoreCase);

                if (compileItems.Count > 0)
                {
                    // Legacy .nfproj style: explicit Compile includes
                    foreach (System.Text.RegularExpressions.Match compileItem in compileItems)
                    {
                        var filePath = compileItem.Groups["source_file"].Value;
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            filePath = filePath.Replace("/", "\\");
                        }
                        else
                        {
                            filePath = filePath.Replace("\\", "/");
                        }

                        allCsFiles.Add($"{Path.Combine(Path.GetFullPath(projectFile.DirectoryName), filePath)}");
                    }
                }
                else
                {
                    // SDK-style .csproj: source files are implicitly globbed
                    var projectDir = projectFile.DirectoryName;
                    foreach (var csFile in Directory.GetFiles(projectDir, "*.cs", SearchOption.AllDirectories))
                    {
                        // Skip common output/intermediate directories
                        if (csFile.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                            || csFile.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"))
                        {
                            continue;
                        }

                        allCsFiles.Add(csFile);
                    }
                }
            }

            return allCsFiles.ToArray();
        }

        private static FileInfo[] FindProjectFile(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new FileInfo[0];
            }

            try
            {
                // Start from the directory containing the source file/assembly
                DirectoryInfo directory;

                if (File.Exists(source))
                {
                    directory = new DirectoryInfo(Path.GetDirectoryName(source));
                }
                else if (Directory.Exists(source))
                {
                    directory = new DirectoryInfo(source);
                }
                else
                {
                    return new FileInfo[0];
                }

                // Walk up the directory tree until a project file is found
                while (directory != null)
                {
                    // Search for .csproj (SDK-style) first, then .nfproj (legacy)
                    FileInfo[] projectFiles = directory.GetFiles("*.csproj");

                    if (projectFiles.Length == 0)
                    {
                        projectFiles = directory.GetFiles("*.nfproj");
                    }

                    if (projectFiles.Length > 0)
                    {
                        return projectFiles;
                    }

                    directory = directory.Parent;
                }

                return new FileInfo[0];
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
