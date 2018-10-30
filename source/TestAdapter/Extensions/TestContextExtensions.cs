//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TestPlatform.MSTest.TestAdapter.Interface;

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.Extensions
{
    internal static class TestContextExtensions
    {
        /// <summary>
        /// Returns diagnostic messages written to test context and clears from this instance.
        /// </summary>
        /// <param name="testContext">The test context instance.</param>
        /// <returns>The diagnostic messages.</returns>
        internal static string GetAndClearDiagnosticMessages(this ITestContext testContext)
        {
            var messages = testContext.GetDiagnosticMessages();

            testContext.ClearDiagnosticMessages();

            return messages;
        }
    }
}
