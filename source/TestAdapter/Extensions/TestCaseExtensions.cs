//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.Extensions
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel;

    /// <summary>
    /// Extension Methods for TestCase Class
    /// </summary>
    internal static class TestCaseExtensions
    {
        /// <summary>
        /// The to unit test element.
        /// </summary>
        /// <param name="testCase"> The test case. </param>
        /// <param name="source"> The source. If deployed this is the full path of the source in the deployment directory. </param>
        /// <returns> The converted <see cref="UnitTestElement"/>. </returns>
        internal static UnitTestElement ToUnitTestElement(this TestCase testCase, string source)
        {
            // TODO currently nanoFramework does not support async
            //var isAsync = (testCase.GetPropertyValue(Constants.AsyncTestProperty) as bool?) ?? false;
            var isAsync = false;

            var testClassName = testCase.GetPropertyValue(TestAdapter.Constants.TestClassNameProperty) as string;

            TestMethod testMethod = new TestMethod(testCase.DisplayName, testClassName, source, isAsync);

            UnitTestElement testElement = new UnitTestElement(testMethod)
                                        {
                                            IsAsync = isAsync,
                                            TestCategory = testCase.GetPropertyValue(TestAdapter.Constants.TestCategoryProperty) as string[],
                                            Priority = testCase.GetPropertyValue(TestAdapter.Constants.PriorityProperty) as int?
                                        };

            return testElement;
        }
    }
}
