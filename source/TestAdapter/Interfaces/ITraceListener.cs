//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.Interface
{
    using System.IO;

    /// <summary>
    /// Operations on the TraceListener object that is implemented differently for each platform.
    /// </summary>
    public interface ITraceListener
    {
        /// <summary>
        /// Gets the text writer that receives the tracing or debugging output.
        /// </summary>
        /// <returns>The writer instance.</returns>
        TextWriter GetWriter();

        /// <summary>
        ///  Disposes this TraceListener object.
        /// </summary>
        void Dispose();
    }
}
