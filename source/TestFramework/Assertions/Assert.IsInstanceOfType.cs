// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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
        /// Tests whether the specified object is an instance of the expected
        /// type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">
        /// The object the test expects to be of the specified type.
        /// </param>
        /// <param name="expectedType">
        /// The expected type of <paramref name="value"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is not an instance of <paramref name="expectedType"/>. The message is
        /// shown in test results.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> is null or
        /// <paramref name="expectedType"/> is not in the inheritance hierarchy
        /// of <paramref name="value"/>.
        /// </exception>
        public static void IsInstanceOfType([NotNull] object? value, [NotNull] Type? expectedType, string? message = "", [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            if (IsInstanceOfTypeFailing(value, expectedType))
            {
                ThrowAssertIsInstanceOfTypeFailed(value, expectedType, BuildUserMessageForValueExpression(message, valueExpression));
            }
        }

        /// <summary>
        /// Tests whether the specified object is an instance of the generic
        /// type and throws an exception if the generic type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <typeparam name="T">The expected type of <paramref name="value"/>.</typeparam>
        public static T IsInstanceOfType<T>([NotNull] object? value, string? message = "", [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            IsInstanceOfType(value, typeof(T), message, valueExpression);
            return (T)value!;
        }

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong
        /// type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">
        /// The object the test expects not to be of the specified type.
        /// </param>
        /// <param name="wrongType">
        /// The type that <paramref name="value"/> should not be.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is an instance of <paramref name="wrongType"/>. The message is shown
        /// in test results.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="value"/> is not null and
        /// <paramref name="wrongType"/> is in the inheritance hierarchy
        /// of <paramref name="value"/>.
        /// </exception>
        public static void IsNotInstanceOfType(object? value, [NotNull] Type? wrongType, string? message = "", [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            if (IsNotInstanceOfTypeFailing(value, wrongType))
            {
                ThrowAssertIsNotInstanceOfTypeFailed(value, wrongType, BuildUserMessageForValueExpression(message, valueExpression));
            }
        }

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong generic
        /// type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <typeparam name="T">The type that <paramref name="value"/> should not be.</typeparam>
        public static void IsNotInstanceOfType<T>(object? value, string? message = "", [CallerArgumentExpression(nameof(value))] string valueExpression = "")
            => IsNotInstanceOfType(value, typeof(T), message, valueExpression);

        private static bool IsNotInstanceOfTypeFailing(object? value, [NotNullWhen(false)] Type? wrongType)
            => wrongType is null ||
                // Null is not an instance of any type.
                (value is not null && wrongType.IsInstanceOfType(value));

        [DoesNotReturn]
        private static void ThrowAssertIsNotInstanceOfTypeFailed(object? value, Type? wrongType, string userMessage)
        {
            string finalMessage = userMessage;
            if (wrongType is not null)
            {
                finalMessage = string.Format(
                    "Wrong Type:&lt;{1}&gt;. Actual type:&lt;{2}&gt;. {0}",
                    userMessage,
                    wrongType.ToString(),
                    value!.GetType().ToString());
            }

            ThrowAssertFailed("Assert.IsNotInstanceOfType", finalMessage);
        }

        private static bool IsInstanceOfTypeFailing([NotNullWhen(false)] object? value, [NotNullWhen(false)] Type? expectedType)
            => expectedType == null || value == null || !expectedType.IsInstanceOfType(value);

        [DoesNotReturn]
        private static void ThrowAssertIsInstanceOfTypeFailed(object? value, Type? expectedType, string userMessage)
        {
            string finalMessage = userMessage;
            if (expectedType is not null && value is not null)
            {
                finalMessage = string.Format(
                    "{0} Expected type:&lt;{1}&gt;. Actual type:&lt;{2}&gt;.",
                    userMessage,
                    expectedType.ToString(),
                    value.GetType().ToString());
            }

            ThrowAssertFailed("Assert.IsInstanceOfType", finalMessage);
        }
    }
}
