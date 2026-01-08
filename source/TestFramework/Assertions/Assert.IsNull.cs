// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TestFrameworkShared;

#nullable enable

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// A collection of helper classes to test various conditions within unit tests. If the condition being tested is not met, an exception is thrown.
    /// </summary>
    public sealed partial class Assert
    {
        /// <summary>
        /// Tests whether the specified object is null and throws an exception
        /// if it is not.
        /// </summary>
        /// <param name="value">
        /// The object the test expects to be null.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is not null. The message is shown in test results.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> is not null.
        /// </exception>
        public static void IsNull(
            object? value,
            string? message = "",
            [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            if (IsNullFailing(value))
            {
                ThrowAssertIsNullFailed(BuildUserMessageForValueExpression(message, valueExpression));
            }
        }

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception
        /// if it is null.
        /// </summary>
        /// <param name="value">
        /// The object the test expects not to be null.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is null. The message is shown in test results.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> is null.
        /// </exception>
        public static void IsNotNull(
            [NotNull] object? value,
            string? message = "",
            [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            if (IsNotNullFailing(value))
            {
                ThrowAssertIsNotNullFailed(BuildUserMessageForValueExpression(message, valueExpression));
            }
        }

        private static bool IsNullFailing(object? value) => value is not null;

        private static void ThrowAssertIsNullFailed(string? message)
            => ThrowAssertFailed("Assert.IsNull", message);
        private static bool IsNotNullFailing([NotNullWhen(false)] object? value) => value is null;

        [DoesNotReturn]
        private static void ThrowAssertIsNotNullFailed(string? message)
            => ThrowAssertFailed("Assert.IsNotNull", message);
    }
}
