// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Helper class to allow output messages from Unit Tests.
    /// </summary>
    internal static class Guard
    {
        /// <summary>
        ///     Throws an exception if the given value is <c>null</c>.
        /// </summary>
        /// <param name="value">
        ///     The value to check.
        /// </param>
        /// <param name="paramName">
        ///     The name of the parameter to emit in the <see cref="ArgumentNullException"/>
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="value"/> is <c>null</c>.
        /// </exception>
        public static void NotNull(object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
