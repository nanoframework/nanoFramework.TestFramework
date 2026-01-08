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
        /// Tests whether the specified condition is true and throws an exception
        /// if the condition is false.
        /// </summary>
        /// <param name="condition">
        /// The condition the test expects to be true.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="condition"/>
        /// is false. The message is shown in test results.
        /// </param>
        /// <param name="conditionExpression">
        /// The syntactic expression of condition as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="condition"/> is false.
        /// </exception>
        public static void IsTrue(
            [DoesNotReturnIf(false)] bool? condition,
            string? message = "",
            [CallerArgumentExpression(nameof(condition))] string conditionExpression = "")
        {
            if (IsTrueFailing(condition))
            {
                ThrowAssertIsTrueFailed(BuildUserMessageForConditionExpression(message, conditionExpression));
            }
        }

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception
        /// if the condition is true.
        /// </summary>
        /// <param name="condition">
        /// The condition the test expects to be false.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="condition"/>
        /// is true. The message is shown in test results.
        /// </param>
        /// <param name="conditionExpression">
        /// The syntactic expression of condition as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="condition"/> is true.
        /// </exception>
        public static void IsFalse(
            [DoesNotReturnIf(true)] bool? condition,
            string? message = "",
            [CallerArgumentExpression(nameof(condition))] string conditionExpression = "")
        {
            if (IsFalseFailing(condition))
            {
                ThrowAssertIsFalseFailed(BuildUserMessageForConditionExpression(message, conditionExpression));
            }
        }

        private static bool IsFalseFailing(bool? condition)
            => condition is true or null;

        [DoesNotReturn]
        private static void ThrowAssertIsFalseFailed(string userMessage)
            => ThrowAssertFailed("Assert.IsFalse", userMessage);

        private static bool IsTrueFailing(bool? condition) => condition is false or null;

        private static void ThrowAssertIsTrueFailed(string? message)
            => ThrowAssertFailed("Assert.IsTrue", message);
    }
}
