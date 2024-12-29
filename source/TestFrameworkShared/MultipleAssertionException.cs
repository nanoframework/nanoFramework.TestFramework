// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace TestFrameworkShared
{
    /// <summary>
    /// Represents an exception that encapsulates multiple assertion failures.
    /// </summary>
    public class MultipleAssertionException : Exception
    {
        private readonly Exception[] _failures;
        private readonly int _failureCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAssertionException"/> class.
        /// </summary>
        /// <param name="failures">The array of exceptions representing assertion failures.</param>
        /// <param name="failureCount">The number of failures in the <paramref name="failures"/> array.</param>
        public MultipleAssertionException(Exception[] failures, int failureCount)
            : base("One or more assertions failed.")
        {
            _failures = failures;
            _failureCount = failureCount;

            Console.WriteLine(ToString());
        }

        /// <summary>
        /// Returns a string representation of the exception, including all assertion failures.
        /// </summary>
        /// <returns>A string describing the exception and all failures.</returns>
        public override string ToString()
        {
            string message = base.ToString();

            for (int i = 0; i < _failureCount; i++)
            {
                if(_failures[i] == null)
                {
                    continue;
                }

                message += "\n" + _failures[i].ToString();
            }

            return message;
        }
    }
}
