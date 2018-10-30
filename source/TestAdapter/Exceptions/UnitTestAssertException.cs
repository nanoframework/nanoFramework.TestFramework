//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    using System;

    /// <summary>
    /// Base class for Framework Exceptions.
    /// </summary>
    public abstract partial class UnitTestAssertException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestAssertException"/> class.
        /// </summary>
        protected UnitTestAssertException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestAssertException"/> class.
        /// </summary>
        /// <param name="msg"> The message. </param>
        /// <param name="ex"> The exception. </param>
        protected UnitTestAssertException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestAssertException"/> class.
        /// </summary>
        /// <param name="msg"> The message. </param>
        protected UnitTestAssertException(string msg)
            : base(msg)
        {
        }
    }
}
