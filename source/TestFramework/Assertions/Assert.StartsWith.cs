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
        /// Tests whether the specified string begins with the specified prefix
        /// and throws an exception if the test string does not start with the
        /// prefix.
        /// </summary>
        /// <param name="expectedPrefix">
        /// The string expected to be a prefix of <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The string that is expected to begin with <paramref name="expectedPrefix"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// does not begin with <paramref name="expectedPrefix"/>. The message is
        /// shown in test results.
        /// </param>
        /// <param name="expectedPrefixExpression">
        /// The syntactic expression of expectedPrefix as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="value"/> is null, or <paramref name="expectedPrefix"/> is null,
        /// or <paramref name="value"/> does not start with <paramref name="expectedPrefix"/>.
        /// </exception>
        public static void StartsWith([NotNull] string? expectedPrefix, [NotNull] string? value, string? message = "", [CallerArgumentExpression(nameof(expectedPrefix))] string expectedPrefixExpression = "", [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            CheckParameterNotNull(value, "Assert.StartsWith", "value");
            CheckParameterNotNull(expectedPrefix, "Assert.StartsWith", "expectedPrefix");

            if (!value.StartsWith(expectedPrefix))
            {
                string userMessage = BuildUserMessageForExpectedPrefixExpressionAndValueExpression(message, expectedPrefixExpression, valueExpression);
                string finalMessage = string.Format("String '{0}' does not start with string '{1}'. {2}", value, expectedPrefix, userMessage);

                ThrowAssertFailed("Assert.StartsWith", finalMessage, expectedPrefix, value);
            }
        }

        /// <summary>
        /// Tests whether the specified string does not begin with the specified unexpected prefix
        /// and throws an exception if the test string does start with the prefix.
        /// </summary>
        /// <param name="notExpectedPrefix">
        /// The string not expected to be a prefix of <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The string that is not expected to begin with <paramref name="notExpectedPrefix"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// begins with <paramref name="notExpectedPrefix"/>. The message is
        /// shown in test results.
        /// </param>
        /// <param name="notExpectedPrefixExpression">
        /// The syntactic expression of notExpectedPrefix as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="value"/> is null, or <paramref name="notExpectedPrefix"/> is null,
        /// or <paramref name="value"/> does not start with <paramref name="notExpectedPrefix"/>.
        /// </exception>
        public static void DoesNotStartWith([NotNull] string? notExpectedPrefix, [NotNull] string? value, string? message = "", [CallerArgumentExpression(nameof(notExpectedPrefix))] string notExpectedPrefixExpression = "", [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            CheckParameterNotNull(value, "Assert.DoesNotStartWith", "value");
            CheckParameterNotNull(notExpectedPrefix, "Assert.DoesNotStartWith", "notExpectedPrefix");

            if (value.StartsWith(notExpectedPrefix))
            {
                string userMessage = BuildUserMessageForNotExpectedPrefixExpressionAndValueExpression(message, notExpectedPrefixExpression, valueExpression);
                string finalMessage = string.Format("String '{0}' starts with string '{1}'. {2}", value, notExpectedPrefix, userMessage);

                ThrowAssertFailed("Assert.DoesNotStartWith", finalMessage, notExpectedPrefix, value);
            }
        }
    }
}
