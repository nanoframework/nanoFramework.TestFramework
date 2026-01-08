// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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
        /// Tests whether the specified objects both refer to the same object and
        /// throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="expected">
        /// The first object to compare. This is the value the test expects.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not the same as <paramref name="expected"/>. The message is shown
        /// in test results.
        /// </param>
        /// <param name="expectedExpression">
        /// The syntactic expression of expected as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="actualExpression">
        /// The syntactic expression of actual as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> does not refer to the same object
        /// as <paramref name="actual"/>.
        /// </exception>
        public static void AreSame<T>(
            T? expected,
            T? actual,
            string? message = "",
            [CallerArgumentExpression(nameof(expected))] string expectedExpression = "",
            [CallerArgumentExpression(nameof(actual))] string actualExpression = "")
        {
            if (!IsAreSameFailing(expected, actual))
            {
                return;
            }

            string userMessage = BuildUserMessageForExpectedExpressionAndActualExpression(message, expectedExpression, actualExpression);

            ThrowAssertAreSameFailed(expected, actual, userMessage);
        }

        private static bool IsAreSameFailing<T>(T? expected, T? actual)
            => !object.ReferenceEquals(expected, actual);

        [DoesNotReturn]
        private static void ThrowAssertAreSameFailed<T>(T? expected, T? actual, string userMessage)
        {
            string finalMessage = userMessage;
            if (expected is ValueType && actual is ValueType)
            {
                finalMessage = string.Format(
                    "Do not pass value types to AreSame(). Values converted to Object will never be the same. Consider using AreEqual(). {0}",
                    userMessage);
            }

            ThrowAssertFailed("Assert.AreSame", finalMessage);
        }

        /// <summary>
        /// Tests whether the specified objects refer to different objects and
        /// throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="notExpected">
        /// The first object to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is the same as <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <param name="notExpectedExpression">
        /// The syntactic expression of notExpected as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="actualExpression">
        /// The syntactic expression of actual as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> refers to the same object
        /// as <paramref name="actual"/>.
        /// </exception>
        public static void AreNotSame<T>(
            T? notExpected,
            T? actual,
            string? message = "",
            [CallerArgumentExpression(nameof(notExpected))] string notExpectedExpression = "",
            [CallerArgumentExpression(nameof(actual))] string actualExpression = "")
        {
            if (IsAreNotSameFailing(notExpected, actual))
            {
                ThrowAssertAreNotSameFailed(BuildUserMessageForNotExpectedExpressionAndActualExpression(message, notExpectedExpression, actualExpression));
            }
        }

        private static bool IsAreNotSameFailing<T>(T? notExpected, T? actual)
            => object.ReferenceEquals(notExpected, actual);

        [DoesNotReturn]
        private static void ThrowAssertAreNotSameFailed(string userMessage)
            => ThrowAssertFailed("Assert.AreNotSame", userMessage);
    }
}
