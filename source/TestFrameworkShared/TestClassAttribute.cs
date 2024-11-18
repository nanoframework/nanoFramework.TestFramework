// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// The test class attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestClassAttribute : Attribute
    {
        /// <summary>
        /// Gets a test method attribute that enables running this test.
        /// </summary>
        /// <param name="testMethodAttribute">The test method attribute instance defined on this method.</param>
        /// <returns>The <see cref="TestMethodAttribute"/> to be used to run this test.</returns>
        /// <remarks>Extensions can override this method to customize how all methods in a class are run.</remarks>
        public virtual TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
        {
            // If TestMethod is not extended by derived class then return back the original TestMethodAttribute
            return testMethodAttribute;
        }
    }
}
