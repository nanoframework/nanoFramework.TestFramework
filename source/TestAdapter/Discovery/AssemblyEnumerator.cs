//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.Discovery
{
    using nanoFramework.TestPlatform.MSTest.TestAdapter.Helpers;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.Resources;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Text;


    /// <summary>
    /// Enumerates through all types in the assembly in search of valid test methods.
    /// </summary>
    internal class AssemblyEnumerator : MarshalByRefObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyEnumerator"/> class.
        /// </summary>
        public AssemblyEnumerator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyEnumerator"/> class.
        /// </summary>
        /// <param name="settings">The settings for the session.</param>
        /// <remarks>Use this constructor when creating this object in a new app domain so the settings for this app domain are set.</remarks>
        public AssemblyEnumerator(TestSettings settings)
        {
            // Populate the settings into the domain(Desktop workflow) performing discovery.
            // This would just be resettings the settings to itself in non desktop workflows.
            TestSettings.PopulateSettings(settings);
        }

        /// <summary>
        /// Returns object to be used for controlling lifetime, null means infinite lifetime.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        [SecurityCritical]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Enumerates through all types in the assembly in search of valid test methods.
        /// </summary>
        /// <param name="assemblyFileName"> The assembly file name. </param>
        /// <param name="warnings"> Contains warnings if any, that need to be passed back to the caller. </param>
        /// <returns> A collection of Test Elements. </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Catching a generic exception since it is a requirement to not abort discovery in case of any errors.")]
        internal ICollection<UnitTestElement> EnumerateAssembly(
            string assemblyFileName, 
            out ICollection<string> warnings)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(assemblyFileName), "Invalid assembly file name.");

            var warningMessages = new List<string>();
            var tests = new List<UnitTestElement>();

            Type testClassAttrib = null;
            Type testMethodAttrib = null;


            // load test attributes from TestFramework nanoFramework assembly
            var testFrameworkAssembly = PlatformServiceProvider.Instance.FileOperations.LoadAssembly(
                    Path.GetDirectoryName(assemblyFileName) + "\\nanoFramework.TestPlatform.TestFramework.dll",
                    isReflectionOnly: true);

            if(testFrameworkAssembly == null)
            {
                // If we fail to load the TestFramework assembly abort the discovery because that won't be possible
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    "Fail to load nanoFramework.TestPlatform.TestFramework assembly.");

                warningMessages.Add(message);

                PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("Fail to load nanoFramework.TestPlatform.TestFramework assembly.");

                warningMessages.Add(message);
            }

            var testFrameworkTypes = this.GetTypes(testFrameworkAssembly, assemblyFileName, null);

            foreach (var type in testFrameworkTypes)
            {
                if (type == null)
                {
                    continue;
                }

                string typeFullName = null;

                try
                {
                    typeFullName = type.FullName;

                    if(typeFullName == Constants.TestClassAttributeFullName)
                    {
                        testClassAttrib = type;
                        continue;
                    }
                    else if (typeFullName == Constants.TestMethodAttributeFullName)
                    {
                        testMethodAttrib = type;
                        continue;
                    }
                }
                catch (Exception exception)
                {
                    // If we fail to discover the test attributes abort the discovery because that won't be possible
                    string message = string.Format(
                        CultureInfo.CurrentCulture,
                        "Exception occurred when searching test attributes in nanoFramework.TestPlatform.TestFramework assembly. {0}",
                        exception.Message);

                    warningMessages.Add(message);

                    PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("Could not find the test attributes in nanoFramework.TestPlatform.TestFramework assembly. {0}", exception);

                    warningMessages.Add(message);
                }
            }

            if (testClassAttrib != null && testMethodAttrib != null)
            {
                // we have test attributes so we are good to go
                // always need to load the source assembly in reflection only context
                Assembly assembly;
                assembly = PlatformServiceProvider.Instance.FileOperations.LoadAssembly(
                    assemblyFileName,
                    isReflectionOnly: true);

                var types = this.GetTypes(assembly, assemblyFileName, warningMessages);

                foreach (var type in types)
                {
                    if (type == null)
                    {
                        continue;
                    }

                    string typeFullName = null;

                    try
                    {
                        ICollection<string> warningsFromTypeEnumerator;

                        typeFullName = type.FullName;
                        var unitTestCases = this.GetTypeEnumerator(type, assemblyFileName, testClassAttrib, testMethodAttrib).Enumerate(out warningsFromTypeEnumerator);

                        if (warningsFromTypeEnumerator != null)
                        {
                            warningMessages.AddRange(warningsFromTypeEnumerator);
                        }

                        if (unitTestCases != null)
                        {
                            tests.AddRange(unitTestCases);
                        }
                    }
                    catch (Exception exception)
                    {
                        // If we fail to discover type from a class, then don't abort the discovery
                        // Move to the next type.
                        string message = string.Format(
                            CultureInfo.CurrentCulture,
                            Resource.CouldNotInspectTypeDuringDiscovery,
                            typeFullName,
                            assemblyFileName,
                            exception.Message);
                        warningMessages.Add(message);

                        PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo(
                            "AssemblyEnumerator: Exception occurred while enumerating type {0}. {1}",
                            typeFullName,
                            exception);
                    }
                }
            }
            else
            {
                // If we fail to discover the test attributes abort the discovery because that won't be possible
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    "Fail to find test attributes in nanoFramework.TestPlatform.TestFramework assembly.");

                warningMessages.Add(message);

                PlatformServiceProvider.Instance.AdapterTraceLogger.LogInfo("Fail to find test attributes in nanoFramework.TestPlatform.TestFramework assembly.");

                warningMessages.Add(message);

            }

            warnings = warningMessages;
            return tests;
        }

        /// <summary>
        /// Gets the types defined in an assembly.
        /// </summary>
        /// <param name="assembly">The reflected assembly.</param>
        /// <param name="assemblyFileName">The file name of the assembly.</param>
        /// <param name="warningMessages">Contains warnings if any, that need to be passed back to the caller.</param>
        /// <returns>Gets the types defined in the provided assembly.</returns>
        internal Type[] GetTypes(
            Assembly assembly, 
            string assemblyFileName, 
            ICollection<string> warningMessages)
        {
            var types = new List<Type>();
            try
            {
                types.AddRange(assembly.DefinedTypes.Select(typeinfo => typeinfo.AsType()));
            }
            catch (ReflectionTypeLoadException ex)
            {
                PlatformServiceProvider.Instance.AdapterTraceLogger.LogWarning(
                    "TestExecutor.TryGetTests: Failed to discover tests from {0}. Reason:{1}",
                    assemblyFileName,
                    ex);
                PlatformServiceProvider.Instance.AdapterTraceLogger.LogWarning("Exceptions thrown from the Loader :");

                if (ex.LoaderExceptions != null)
                {
                    // If not able to load all type, log a warning and continue with loaded types.
                    var message = string.Format(
                        CultureInfo.CurrentCulture,
                        Resource.TypeLoadFailed,
                        assemblyFileName,
                        this.GetLoadExceptionDetails(ex));

                    warningMessages?.Add(message);

                    foreach (var loaderEx in ex.LoaderExceptions)
                    {
                        PlatformServiceProvider.Instance.AdapterTraceLogger.LogWarning("{0}", loaderEx);
                    }
                }

                return ex.Types;
            }

            return types.ToArray();
        }

        /// <summary>
        /// Formats load exception as multiline string, each line contains load error message.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>Returns loader exceptions as a multiline string.</returns>
        internal string GetLoadExceptionDetails(ReflectionTypeLoadException ex)
        {
            Debug.Assert(ex != null, "exception should not be null.");

            var map = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase); // Exception -> null.
            var errorDetails = new StringBuilder();

            if (ex.LoaderExceptions != null)
            {
                // Loader exceptions can contain duplicates, leave only unique exceptions.
                foreach (var loaderException in ex.LoaderExceptions)
                {
                    Debug.Assert(loaderException != null, "loader exception should not be null.");
                    var line = string.Format(CultureInfo.CurrentCulture, Resource.EnumeratorLoadTypeErrorFormat, loaderException.GetType(), loaderException.Message);
                    if (!map.ContainsKey(line))
                    {
                        map.Add(line, null);
                        errorDetails.AppendLine(line);
                    }
                }
            }
            else
            {
                errorDetails.AppendLine(ex.Message);
            }

            return errorDetails.ToString();
        }

        /// <summary>
        /// Returns an instance of the <see cref="TypeEnumerator"/> class.
        /// </summary>
        /// <param name="type">The type to enumerate.</param>
        /// <param name="assemblyFileName">The reflected assembly name.</param>
        /// <returns>a TypeEnumerator instance.</returns>
        internal virtual TypeEnumerator GetTypeEnumerator(
            Type type, 
            string assemblyFileName, 
            Type testClassAttrib, 
            Type testMethodAttrib)
        {
            var reflectHelper = new ReflectHelper();
            var typevalidator = new TypeValidator(reflectHelper, testClassAttrib);
            var testMethodValidator = new TestMethodValidator(reflectHelper, testMethodAttrib);

            return new TypeEnumerator(type, assemblyFileName, reflectHelper, typevalidator, testMethodValidator);
        }
    }
}
