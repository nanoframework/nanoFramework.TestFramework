//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Helper class to allow output messages from Unit Tests.
    /// </summary>
    public static class OutputHelper
    {
        /// <summary>
        /// Writes a message to the test trace output.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public static void Write(string message) => System.Console.Write(message);

        /// <summary>
        /// Writes a message followed by a line terminator to the test trace output.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public static void WriteLine(string message) => System.Console.WriteLine(message);
    }
}
