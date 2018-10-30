//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    using System;

    /// <summary>
    /// AssertFailedException class. Used to indicate failure for a test case
    /// </summary>
    public partial class AssertFailedException : UnitTestAssertException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        /// <param name="msg"> The message. </param>
        /// <param name="ex"> The exception. </param>
        public AssertFailedException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        /// <param name="msg"> The message. </param>
        public AssertFailedException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailedException"/> class.
        /// </summary>
        public AssertFailedException()
            : base()
        {
        }
    }
}
