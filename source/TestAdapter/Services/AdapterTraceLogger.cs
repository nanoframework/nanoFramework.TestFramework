//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.PlatformServices
{
    using nanoFramework.TestPlatform.MSTest.TestAdapter.PlatformServices.Interface;

    /// <summary>
    /// The trace logger for .Net Core.
    /// </summary>
    public class AdapterTraceLogger : IAdapterTraceLogger
    {
        /// <summary>
        /// Log an error in a given format.
        /// </summary>
        /// <param name="format"> The format. </param>
        /// <param name="args"> The args. </param>
        /// <exception cref="System.NotImplementedException"> This is currently not implemented. </exception>
        public void LogError(string format, params object[] args)
        {
            // Do Nothing.
        }

        /// <summary>
        /// Log a warning in a given format.
        /// </summary>
        /// <param name="format"> The format. </param>
        /// <param name="args"> The args. </param>
        /// <exception cref="System.NotImplementedException"> This is currently not implemented. </exception>
        public void LogWarning(string format, params object[] args)
        {
            // Do Nothing.
        }

        /// <summary>
        /// Log an information message in a given format.
        /// </summary>
        /// <param name="format"> The format. </param>
        /// <param name="args"> The args. </param>
        /// <exception cref="System.NotImplementedException"> This is currently not implemented. </exception>
        public void LogInfo(string format, params object[] args)
        {
            // Do Nothing.
        }
    }
}
