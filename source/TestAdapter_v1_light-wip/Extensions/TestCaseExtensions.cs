//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using VSTestCase = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase;

namespace nanoFramework.TestPlatform.TestAdapter
{
    public static class TestCaseExtensions
    {
        public static VSTestCase ToVSTestCase(this TestInterface.TestCase testCase)
        {
            var vsTestCase = new TestCase(testCase.Name, TestExecutor.ExecutorUri, testCase.Source);
            vsTestCase.CodeFilePath = testCase.Filename;
            vsTestCase.LineNumber = testCase.Line;

            foreach (var tag in testCase.Tags)
            {
                vsTestCase.Traits.Add(new Trait("Tag", tag));
            }

            return vsTestCase;
        }
    }
}
