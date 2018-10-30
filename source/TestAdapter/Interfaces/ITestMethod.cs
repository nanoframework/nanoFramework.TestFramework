//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    using System;
    using System.Reflection;

    /// <summary>
    /// TestMethod for execution.
    /// </summary>
    public partial interface ITestMethod
    {
        /// <summary>
        /// Gets the name of test method.
        /// </summary>
        string TestMethodName { get; }

        /// <summary>
        /// Gets the name of test class.
        /// </summary>
        string TestClassName { get; }

        /// <summary>
        /// Gets the return type of test method.
        /// </summary>
        Type ReturnType { get; }

        /// <summary>
        /// Gets the arguments with which test method is invoked.
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// Gets the parameters of test method.
        /// </summary>
        ParameterInfo[] ParameterTypes { get; }

        /// <summary>
        /// Gets the methodInfo for test method.
        /// </summary>
        /// <remarks>
        /// This is just to retrieve additional information about the method.
        /// Do not directly invoke the method using MethodInfo. Use ITestMethod.Invoke instead.
        /// </remarks>
        MethodInfo MethodInfo { get; }

        /// <summary>
        /// Get all attributes of the test method.
        /// </summary>
        /// <param name="inherit">
        /// Whether attribute defined in parent class is valid.
        /// </param>
        /// <returns>
        /// All attributes.
        /// </returns>
        Attribute[] GetAllAttributes(bool inherit);

        /// <summary>
        /// Get attribute of specific type.
        /// </summary>
        /// <typeparam name="AttributeType"> System.Attribute type. </typeparam>
        /// <param name="inherit">
        /// Whether attribute defined in parent class is valid.
        /// </param>
        /// <returns>
        /// The attributes of the specified type.
        /// </returns>
        AttributeType[] GetAttributes<AttributeType>(bool inherit)
            where AttributeType : Attribute;
    }

}
