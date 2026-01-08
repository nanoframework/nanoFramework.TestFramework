// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TestFrameworkShared;

#nullable enable

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// A collection of helper classes to test various conditions within unit tests. If the condition being tested is not met, an exception is thrown.
    /// </summary>
    public sealed partial class Assert
    {
        private Assert()
        {
        }

        /// <summary>
        /// Helper function that creates and throws an AssertionFailedException.
        /// </summary>
        /// <param name="assertionName">
        /// name of the assertion throwing an exception.
        /// </param>
        /// <param name="message">
        /// The assertion failure message.
        /// </param>
        [DoesNotReturn]
        [StackTraceHidden]
        internal static void ThrowAssertFailed(string assertionName, string? message)
            => throw new AssertFailedException(
                string.Format("{0} failed. {1}", assertionName, message));

        /// <summary>
        /// Helper function that creates and throws an AssertionFailedException with expected and actual values.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the expected and actual values.
        /// </typeparam>
        /// <param name="assertionName">
        /// name of the assertion throwing an exception.
        /// </param>
        /// <param name="message">
        /// The assertion failure message.
        /// </param>
        /// <param name="expected">
        /// Expected value to store in exception data.
        /// </param>
        /// <param name="actual">
        /// Actual value to store in exception data.
        /// </param>
        [DoesNotReturn]
        [StackTraceHidden]
        internal static void ThrowAssertFailed<T>(string assertionName, string? message, T? expected = default, T? actual = default)
        {
            AssertFailedException exception = new(
                string.Format("{0} failed. {1}", assertionName, message));

            // Store expected and actual values in exception Data for types with known good ToString implementations
            if (HasKnownGoodToString(expected))
            {
                exception.AssertExpected = expected;
            }

            if (HasKnownGoodToString(actual))
            {
                exception.AssertActual = actual;
            }

            throw exception;
        }

        private static bool HasKnownGoodToString<T>([NotNullWhen(true)] T? value)
        {
            if (value is null)
            {
                return false;
            }

            Type type = typeof(T);

            // Unwrap nullable value types
            type = Nullable.GetUnderlyingType(type) ?? type;

            // Primitive types and string
            // TODO: add this after implementing System.Type.IsPrimitive in nanoFramework
            // if (type.IsPrimitive || type == typeof(string))
            if (type == typeof(string))
            {
                return true;
            }

            // Common types with good ToString implementations
            return type == typeof(DateTime)
                || type == typeof(TimeSpan)
                || type == typeof(Guid);
        }

        /// <summary>
        /// Builds the formatted message using the given user format message and parameters.
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <returns>
        /// The formatted string based on format and parameters.
        /// </returns>
        internal static string BuildUserMessage(string? format)
            => format ?? string.Empty;

        private static string BuildUserMessageForSingleExpression(string? format, string callerArgExpression, string parameterName)
        {
            string userMessage = BuildUserMessage(format);
            if (string.IsNullOrEmpty(callerArgExpression))
            {
                return userMessage;
            }

            string callerArgMessagePart = string.Format("'{0}' expression: '{1}'.", parameterName, callerArgExpression);
            return string.IsNullOrEmpty(userMessage)
                ? callerArgMessagePart
                : $"{callerArgMessagePart} {userMessage}";
        }

        private static string BuildUserMessageForActionExpression(
            string? format,
            string actionExpression)
            => BuildUserMessageForSingleExpression(format, actionExpression, "action");

        private static string BuildUserMessageForCollectionExpression(
            string? format,
            string collectionExpression)
            => BuildUserMessageForSingleExpression(format, collectionExpression, "collection");

        private static string BuildUserMessageForTwoExpressions(
            string? format,
            string callerArgExpression1,
            string parameterName1,
            string callerArgExpression2,
            string parameterName2)
        {
            string userMessage = BuildUserMessage(format);
            if (string.IsNullOrEmpty(callerArgExpression1) || string.IsNullOrEmpty(callerArgExpression2))
            {
                return userMessage;
            }

            string callerArgMessagePart = string.Format(
                "{0}' expression: '{1}', '{2}' expression: '{3}", parameterName1, callerArgExpression1, parameterName2,
                callerArgExpression2);

            return string.IsNullOrEmpty(userMessage)
                ? callerArgMessagePart
                : $"{callerArgMessagePart} {userMessage}";
        }

        private static string BuildUserMessageForConditionExpression(string? format, string conditionExpression)
            => BuildUserMessageForSingleExpression(format, conditionExpression, "condition");

        private static string BuildUserMessageForExpectedExpressionAndCollectionExpression(
            string? format,
            string expectedExpression,
            string collectionExpression)
            => BuildUserMessageForTwoExpressions(format, expectedExpression, "expected", collectionExpression, "collection");

        private static string BuildUserMessageForExpectedExpressionAndActualExpression(
            string? format,
            string expectedExpression,
            string actualExpression) => BuildUserMessageForTwoExpressions(format, expectedExpression, "expected", actualExpression, "actual");


        private static string BuildUserMessageForExpectedSuffixExpressionAndValueExpression(
            string? format,
            string expectedSuffixExpression,
            string valueExpression)
            => BuildUserMessageForTwoExpressions(format, expectedSuffixExpression, "expectedSuffix", valueExpression, "value");

        private static string BuildUserMessageForExpectedPrefixExpressionAndValueExpression(
            string? format,
            string expectedPrefixExpression,
            string valueExpression)
            => BuildUserMessageForTwoExpressions(format, expectedPrefixExpression, "expectedPrefix", valueExpression, "value");

        private static string BuildUserMessageForNotExpectedPrefixExpressionAndValueExpression(
            string? format,
            string notExpectedPrefixExpression,
            string valueExpression)
            => BuildUserMessageForTwoExpressions(format, notExpectedPrefixExpression, "notExpectedPrefix", valueExpression, "value");

        private static string BuildUserMessageForNotExpectedExpressionAndActualExpression(
            string? format,
            string notExpectedExpression,
            string actualExpression) => BuildUserMessageForTwoExpressions(format, notExpectedExpression, "notExpected", actualExpression, "actual");

        private static string BuildUserMessageForNotExpectedExpressionAndCollectionExpression(
            string? format,
            string notExpectedExpression,
            string collectionExpression)
            => BuildUserMessageForTwoExpressions(format, notExpectedExpression, "notExpected", collectionExpression, "collection");

        private static string BuildUserMessageForNotExpectedSuffixExpressionAndValueExpression(
            string? format,
            string notExpectedSuffixExpression,
            string valueExpression)
            => BuildUserMessageForTwoExpressions(format, notExpectedSuffixExpression, "notExpectedSuffix", valueExpression, "value");

        private static string BuildUserMessageForPredicateExpressionAndCollectionExpression(
            string? format,
            string predicateExpression,
            string collectionExpression)
            => BuildUserMessageForTwoExpressions(format, predicateExpression, "predicate", collectionExpression, "collection");

        private static string BuildUserMessageForSubstringExpressionAndValueExpression(
            string? format,
            string substringExpression,
            string valueExpression) => BuildUserMessageForTwoExpressions(format, substringExpression, "substring", valueExpression, "value");

        private static string BuildUserMessageForValueExpression(
            string? format,
            string valueExpression)
            => BuildUserMessageForSingleExpression(format, valueExpression, "value");

        /// <summary>
        /// Checks the parameter for valid conditions.
        /// </summary>
        /// <param name="param">
        /// The parameter.
        /// </param>
        /// <param name="assertionName">
        /// The assertion Name.
        /// </param>
        /// <param name="parameterName">
        /// parameter name.
        /// </param>
        internal static void CheckParameterNotNull([NotNull] object? param, string assertionName, string parameterName)
        {
            if (param == null)
            {
                string finalMessage = string.Format("The parameter '{0}' is invalid. The value cannot be null.", parameterName);
                ThrowAssertFailed(assertionName, finalMessage);
            }
        }
    }
}
