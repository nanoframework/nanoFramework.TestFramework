// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
        /// Tests whether the specified string ends with the specified suffix
        /// and throws an exception if the test string does not end with the
        /// suffix.
        /// </summary>
        /// <param name="expectedSuffix">
        /// The string expected to be a suffix of <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The string that is expected to end with <paramref name="expectedSuffix"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// does not end with <paramref name="expectedSuffix"/>. The message is
        /// shown in test results.
        /// </param>
        /// <param name="expectedSuffixExpression">
        /// The syntactic expression of expectedSuffix as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="value"/> is null, or <paramref name="expectedSuffix"/> is null,
        /// or <paramref name="value"/> does not start with <paramref name="expectedSuffix"/>.
        /// </exception>
        public static void EndsWith([NotNull] string? expectedSuffix, [NotNull] string? value, string? message = "", [CallerArgumentExpression(nameof(expectedSuffix))] string expectedSuffixExpression = "", [CallerArgumentExpression(nameof(value))] string valueExpression = "")
       {
            CheckParameterNotNull(value, "Assert.EndsWith", "value");
            CheckParameterNotNull(expectedSuffix, "Assert.EndsWith", "expectedSuffix");

            if (!value.EndsWith(expectedSuffix))
            {
                string userMessage = BuildUserMessageForExpectedSuffixExpressionAndValueExpression(message, expectedSuffixExpression, valueExpression);
                string finalMessage = string.Format("String '{0}' does not end with string '{1}'. {2}", value, expectedSuffix, userMessage);

                ThrowAssertFailed("Assert.EndsWith", finalMessage, expectedSuffix, value);
            }
        }

        /// <summary>
        /// Tests whether the specified string does not end with the specified unexpected suffix
        /// and throws an exception if the test string ends with the
        /// suffix.
        /// </summary>
        /// <param name="notExpectedSuffix">
        /// The string expected not to be a suffix of <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The string that is expected to not end with <paramref name="notExpectedSuffix"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// ends with <paramref name="notExpectedSuffix"/>. The message is
        /// shown in test results.
        /// </param>
        /// <param name="notExpectedSuffixExpression">
        /// The syntactic expression of notExpectedSuffix as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="value"/> is null, or <paramref name="notExpectedSuffix"/> is null,
        /// or <paramref name="value"/> ends with <paramref name="notExpectedSuffix"/>.
        /// </exception>
        public static void DoesNotEndWith([NotNull] string? notExpectedSuffix, [NotNull] string? value, string? message = "", [CallerArgumentExpression(nameof(notExpectedSuffix))] string notExpectedSuffixExpression = "", [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            CheckParameterNotNull(value, "Assert.DoesNotEndWith", "value");
            CheckParameterNotNull(notExpectedSuffix, "Assert.DoesNotEndWith", "notExpectedSuffix");

            if (value.EndsWith(notExpectedSuffix))
            {
                string userMessage = BuildUserMessageForNotExpectedSuffixExpressionAndValueExpression(message, notExpectedSuffixExpression, valueExpression);
                string finalMessage = string.Format("String '{0}' ends with string '{1}'. {2}", value, notExpectedSuffix, userMessage);

                ThrowAssertFailed("Assert.DoesNotEndWith", finalMessage, notExpectedSuffix, value);
            }
        }
    }
}
