//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Runtime.Native;
using nanoFramework.TestFramework;
using System;
using System.Diagnostics;
using static nanoFramework.Runtime.Native.SystemInfo;

namespace NFUnitTest
{
    [TestClass]
    class SkipFewTest
    {
        [Setup]
        public void SetupMethodWillPass()
        { }

        [TestMethod]
        public void MethodWillPass1()
        { }

        [TestMethod]
        public void MethodWillPass2()
        { }

        [TestMethod]
        public void MethodWillSkip()
        {
            Assert.SkipTest("This is a good reason: testing the test!");
        }

        [TestMethod]
        public void MethodWillSkip2()
        {
            Debug.WriteLine("For no reason");
            Assert.SkipTest();
        }

        [TestMethod]
        public void MethodWillPass3()
        { }


        [TestMethod]
        public void MethodWillSkippIfFloatingPointSupportNotOK()
        {
            var sysInfoFloat = SystemInfo.FloatingPointSupport;
            if ((sysInfoFloat != FloatingPoint.DoublePrecisionHardware) && (sysInfoFloat != FloatingPoint.DoublePrecisionSoftware))
            {
                Assert.SkipTest("Double floating point not supported, skipping the Assert.Double test");
            }

            double on42 = 42.1;
            double maxDouble = double.MaxValue;
            Assert.Equal(42.1, on42);
            Assert.Equal(double.MaxValue, maxDouble);
        }
    }
}
