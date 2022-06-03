//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;

namespace nanoFramework.TestFramework.Test
{
    [TestClass]
    public class TestOfDataRow
    {
        [DataRow(1, 2, 3)]
        [DataRow(5, 6, 11)]
        public void TestAddition(int number1, int number2, int result)
        {
            var additionResult = number1 + number2;

            Assert.Equal(additionResult, result);
        }

        [DataRow("TestString")]
        public void TestString(string testData)
        {
            Assert.Equal(testData, "TestString");
        }
    }
}
