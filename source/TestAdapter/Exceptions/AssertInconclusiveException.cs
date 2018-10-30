//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    using System;

    /// <summary>
    /// The assert inconclusive exception.
    /// </summary>
    public partial class AssertInconclusiveException : UnitTestAssertException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertInconclusiveException"/> class.
        /// </summary>
        /// <param name="msg"> The message. </param>
        /// <param name="ex"> The exception. </param>
        public AssertInconclusiveException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertInconclusiveException"/> class.
        /// </summary>
        /// <param name="msg"> The message. </param>
        public AssertInconclusiveException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertInconclusiveException"/> class.
        /// </summary>
        public AssertInconclusiveException()
            : base()
        {
        }
    }
}
