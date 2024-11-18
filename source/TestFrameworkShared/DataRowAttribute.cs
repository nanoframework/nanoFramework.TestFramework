// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Data row attribute. Used for passing multiple parameters into same test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DataRowAttribute : Attribute
    {
        /// <summary>
        /// Array containing all passed parameters
        /// </summary>
        public object[] MethodParameters { get; }

        /// <summary>
        /// Initializes a new instance of the DataRowAttribute class.
        /// </summary>
        /// <param name="methodParameters">Parameters which should be stored for future execution of test method</param>
        /// <exception cref="ArgumentNullException">Thrown when methodParameters is null</exception>
        /// <exception cref="ArgumentException">Thrown when methodParameters is empty</exception>
        public DataRowAttribute(params object[] methodParameters)
        {
            if (methodParameters == null)
            {
                throw new ArgumentNullException($"{nameof(methodParameters)} can not be null");
            }

            if (methodParameters.Length == 0)
            {
                throw new ArgumentException($"{nameof(methodParameters)} can not be empty");
            }

            MethodParameters = methodParameters;
        }
    }
}
