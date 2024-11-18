// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// To skip a test, raise this exception through the Assert.SkipTest("some message");
    /// </summary>
    public class SkipTestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the SkipTestException class with a specified error message
        /// and a reference to the inner SkipTestException that is the cause of this exception. 
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException"></param>
        public SkipTestException(string message = null, Exception innerException = null) : base(message, innerException)
        { }
    }
}
