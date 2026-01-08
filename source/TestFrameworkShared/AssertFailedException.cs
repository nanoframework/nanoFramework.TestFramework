// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using nanoFramework.TestFramework;

namespace TestFrameworkShared
{
    /// <summary>
    /// <see cref="AssertFailedException"/> class. Used to indicate failure for a test case.
    /// </summary>
    public class AssertFailedException : Exception
    {
        internal object AssertExpected;
        internal object AssertActual;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="ex">The exception.</param>
        public AssertFailedException(string msg, Exception ex)
               : base(msg, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        /// <param name="msg">The message.</param>
        public AssertFailedException(string msg)
            : base(msg)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        public AssertFailedException()
        {
        }
    }
}
