//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.Interface;

    /// <summary>
    ///  A fascade service for options passed to a test method.
    /// </summary>
    internal class TestMethodOptions
    {
        /// <summary>
        /// Gets or sets the timeout specified for a test method.
        /// </summary>
        internal int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the ExpectedException attribute adorned on a test method.
        /// </summary>
        internal ExpectedExceptionBaseAttribute ExpectedException { get; set; }

        /// <summary>
        /// Gets or sets the testcontext passed into the test method.
        /// </summary>
        internal ITestContext TestContext { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether debug traces should be captured when running the test.
        /// </summary>
        internal bool CaptureDebugTraces { get; set; }

        /// <summary>
        /// Gets or sets the test method executor that invokes the test.
        /// </summary>
        internal TestMethodAttribute Executor { get; set; }
    }
}
