//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    internal class Constants
    {
        internal const string NFExecutorUriString = "executor://nanoFrameworkTestExecutor/v1";

        internal static readonly Uri ExecutorUri = new Uri(NFExecutorUriString);

        /// <summary>
        /// The assembly name of the dll containing logger APIs(EqtTrace) from the TestPlatform.
        /// </summary>
        /// <remarks>
        /// The reason we have this is because the AssemblyResolver itself logs information during resolution.
        /// If the resolver is called for the assembly containing the logger APIs, we do not log so as to prevent a stack overflow.
        /// </remarks>
        internal const string LoggerAssemblyName = "Microsoft.VisualStudio.TestPlatform.ObjectModel";

        internal const string TargetFrameworkAttributeFullName = "System.Runtime.Versioning.TargetFrameworkAttribute";

        internal const string DotNetnanoFrameWorkStringPrefix = ".NETnanoFramework,Version=";

        internal const string TargetFrameworkName = "TargetFrameworkName";

        internal const string TargetFramework_nanoFramework = ".NETnanoFramework";

        internal const string TestClassAttributeFullName = "Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute";

        internal const string TestMethodAttributeFullName = "Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute";


        internal static readonly TestProperty TestClassNameProperty = TestProperty.Register("TestDiscoverer.TestClassName", TestClassNameLabel, typeof(string), TestPropertyAttributes.Hidden, typeof(TestCase));

#pragma warning disable CS0618 // Type or member is obsolete
        internal static readonly TestProperty TestCategoryProperty = TestProperty.Register("TestDiscoverer.TestCategory", TestCategoryLabel, typeof(string[]), TestPropertyAttributes.Hidden | TestPropertyAttributes.Trait, typeof(TestCase));
#pragma warning restore CS0618 // Type or member is obsolete

        internal static readonly TestProperty PriorityProperty = TestProperty.Register("TestDiscoverer.Priority", PriorityLabel, typeof(int), TestPropertyAttributes.Hidden, typeof(TestCase));

        internal static readonly TestProperty DeploymentItemsProperty = TestProperty.Register("MSTestDiscoverer.DeploymentItems", DeploymentItemsLabel, typeof(KeyValuePair<string, string>[]), TestPropertyAttributes.Hidden, typeof(TestCase));

        internal static readonly TestProperty ExecutionIdProperty = TestProperty.Register("ExecutionId", ExecutionIdLabel, typeof(Guid), TestPropertyAttributes.Hidden, typeof(TestResult));

        internal static readonly TestProperty ParentExecIdProperty = TestProperty.Register("ParentExecId", ParentExecIdLabel, typeof(Guid), TestPropertyAttributes.Hidden, typeof(TestResult));

        internal static readonly TestProperty InnerResultsCountProperty = TestProperty.Register("InnerResultsCount", InnerResultsCountLabel, typeof(int), TestPropertyAttributes.Hidden, typeof(TestResult));

        #region Private Constants

        /// <summary>
        /// These are the Test properties used by the adapter, which essentially correspond
        /// to attributes on tests, and may be available in command line/TeamBuild to filter tests.
        /// These Property names should not be localized.
        /// </summary>
        private const string TestClassNameLabel = "ClassName";
        private const string IsAsyncLabel = "IsAsync";
        private const string TestCategoryLabel = "TestCategory";
        private const string PriorityLabel = "Priority";
        private const string DeploymentItemsLabel = "DeploymentItems";
        private const string DoNotParallelizeLabel = "DoNotParallelize";
        private const string ExecutionIdLabel = "ExecutionId";
        private const string ParentExecIdLabel = "ParentExecId";
        private const string InnerResultsCountLabel = "InnerResultsCount";

        private const string TestRunId = "__Tfs_TestRunId__";
        private const string TestPlanId = "__Tfs_TestPlanId__";
        private const string TestCaseId = "__Tfs_TestCaseId__";
        private const string TestPointId = "__Tfs_TestPointId__";
        private const string TestConfigurationId = "__Tfs_TestConfigurationId__";
        private const string TestConfigurationName = "__Tfs_TestConfigurationName__";
        private const string IsInLabEnvironment = "__Tfs_IsInLabEnvironment__";
        private const string BuildConfigurationId = "__Tfs_BuildConfigurationId__";
        private const string BuildDirectory = "__Tfs_BuildDirectory__";
        private const string BuildFlavor = "__Tfs_BuildFlavor__";
        private const string BuildNumber = "__Tfs_BuildNumber__";
        private const string BuildPlatform = "__Tfs_BuildPlatform__";
        private const string BuildUri = "__Tfs_BuildUri__";
        private const string TfsServerCollectionUrl = "__Tfs_TfsServerCollectionUrl__";
        private const string TfsTeamProject = "__Tfs_TeamProject__";

        #endregion
    }
}
