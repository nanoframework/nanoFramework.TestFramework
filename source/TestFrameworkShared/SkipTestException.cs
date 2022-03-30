//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// To skip a test, raise this exception thru the Assert.SkipTest("some message");
    /// </summary>
    public class SkipTestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the SkipTestException class.
        /// </summary>
        public SkipTestException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the SkipTestException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SkipTestException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the SkipTestException class with a specified error message
        /// and a reference to the inner SkipTestException that is the cause of this exception. 
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException"></param>
        public SkipTestException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
