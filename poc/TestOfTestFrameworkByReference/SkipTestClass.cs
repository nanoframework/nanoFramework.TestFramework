//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TestFramework;
using System;
using System.Diagnostics;

namespace NFUnitTest
{
    [TestClass]
    class SkipTestClass
    {
        [Setup]
        public void SetupMethodToSkip()
        {
            Debug.WriteLine("Skippîng all the other methods");
            Assert.SkipTest("None of the other methods should be tested, they should all be skipped.");
        }

        [TestMethod]
        public void TestMothdWhichShouldSkip()
        { }

        [TestMethod]
        public void TestMothdWhichShouldSkip2()
        { }

        [TestMethod]
        public void TestMothdWhichShouldSkip3()
        { }

        [TestMethod]
        public void TestMothdWhichShouldSkip4()
        { }

        [TestMethod]
        public void TestMothdWhichShouldSkip5()
        { }

        [TestMethod]
        public void TestMothdWhichShouldSkip6()
        { }

        [Cleanup]
        public void CleanUpMethodSkip()
        { }
    }
}
