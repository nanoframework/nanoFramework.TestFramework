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
        #region Contains

        // TODO: add after adding Linq support to nanoFramework
        ///// <summary>
        ///// Tests whether the specified collection contains the given element.
        ///// </summary>
        ///// <typeparam name="T">The type of the collection items.</typeparam>
        ///// <param name="expected">The expected item.</param>
        ///// <param name="collection">The collection.</param>
        ///// <param name="message">The message to display when the assertion fails.</param>
        ///// <param name="expectedExpression">
        ///// The syntactic expression of expected as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void Contains<T>(T expected, IEnumerable<T> collection, string? message = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "", [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //{
        //    if (!collection.Contains(expected))
        //    {
        //        string userMessage = BuildUserMessageForExpectedExpressionAndCollectionExpression(message, expectedExpression, collectionExpression);
        //        ThrowAssertContainsItemFailed(userMessage);
        //    }
        //}

        /// <summary>
        /// Tests whether the specified collection contains the given element.
        /// </summary>
        /// <param name="expected">The expected item.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="message">The message to display when the assertion fails.</param>
        /// <param name="expectedExpression">
        /// The syntactic expression of expected as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void Contains(
            object? expected,
            IEnumerable collection,
            string? message = "",
            [CallerArgumentExpression(nameof(expected))] string expectedExpression = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        {
            CheckParameterNotNull(collection, "Assert.Contains", "collection");

            foreach (object? item in collection)
            {
                if (object.Equals(item, expected))
                {
                    return;
                }
            }

            string userMessage = BuildUserMessageForExpectedExpressionAndCollectionExpression(message, expectedExpression, collectionExpression);
            ThrowAssertContainsItemFailed(userMessage);
        }

        // TODO: add after adding Linq support to nanoFramework
        ///// <summary>
        ///// Tests whether the specified collection contains the given element.
        ///// </summary>
        ///// <typeparam name="T">The type of the collection items.</typeparam>
        ///// <param name="expected">The expected item.</param>
        ///// <param name="collection">The collection.</param>
        ///// <param name="comparer">An equality comparer to compare values.</param>
        ///// <param name="message">The message to display when the assertion fails.</param>
        ///// <param name="expectedExpression">
        ///// The syntactic expression of expected as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void Contains<T>(T expected, IEnumerable<T> collection, IEqualityComparer<T> comparer, string? message = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "", [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //{
        //    if (!collection.Contains(expected, comparer))
        //    {
        //        string userMessage = BuildUserMessageForExpectedExpressionAndCollectionExpression(message, expectedExpression, collectionExpression);
        //        ThrowAssertContainsItemFailed(userMessage);
        //    }
        //}

        /// <summary>
        /// Tests whether the specified collection contains the given element.
        /// </summary>
        /// <param name="expected">The expected item.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <param name="message">The message to display when the assertion fails.</param>
        /// <param name="expectedExpression">
        /// The syntactic expression of expected as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void Contains(
            object? expected,
            IEnumerable collection,
            IEqualityComparer comparer,
            string? message = "",
            [CallerArgumentExpression(nameof(expected))] string expectedExpression = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        {
            CheckParameterNotNull(collection, "Assert.Contains", "collection");
            CheckParameterNotNull(comparer, "Assert.Contains", "comparer");

            foreach (object? item in collection)
            {
                if (comparer.Equals(item, expected))
                {
                    return;
                }
            }

            string userMessage = BuildUserMessageForExpectedExpressionAndCollectionExpression(message, expectedExpression, collectionExpression);
            ThrowAssertContainsItemFailed(userMessage);
        }

        // TODO: add after adding Linq support to nanoFramework
        ///// <summary>
        ///// Tests whether the specified collection contains the given element.
        ///// </summary>
        ///// <typeparam name="T">The type of the collection items.</typeparam>
        ///// <param name="predicate">A function to test each element for a condition.</param>
        ///// <param name="collection">The collection.</param>
        ///// <param name="message">The message to display when the assertion fails.</param>
        ///// <param name="predicateExpression">
        ///// The syntactic expression of predicate as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void Contains<T>(Func<T, bool> predicate, IEnumerable<T> collection, string? message = "", [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "", [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //{
        //    if (!collection.Any(predicate))
        //    {
        //        string userMessage = BuildUserMessageForPredicateExpressionAndCollectionExpression(message, predicateExpression, collectionExpression);
        //        ThrowAssertContainsPredicateFailed(userMessage);
        //    }
        //}

        /// <summary>
        /// Tests whether the specified collection contains the given element.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="message">The message to display when the assertion fails.</param>
        /// <param name="predicateExpression">
        /// The syntactic expression of predicate as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void Contains(
            Func<object?, bool> predicate,
            IEnumerable collection,
            string? message = "",
            [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        {
            CheckParameterNotNull(collection, "Assert.Contains", "collection");
            CheckParameterNotNull(predicate, "Assert.Contains", "predicate");

            foreach (object? item in collection)
            {
                if (predicate(item))
                {
                    return;
                }
            }

            string userMessage = BuildUserMessageForPredicateExpressionAndCollectionExpression(message, predicateExpression, collectionExpression);
            ThrowAssertContainsPredicateFailed(userMessage);
        }

        /// <summary>
        /// Tests whether the specified string contains the specified substring
        /// and throws an exception if the substring does not occur within the
        /// test string.
        /// </summary>
        /// <param name="substring">
        /// The string expected to occur within <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The string that is expected to contain <paramref name="substring"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="substring"/>
        /// is not in <paramref name="value"/>. The message is shown in
        /// test results.
        /// </param>
        /// <param name="substringExpression">
        /// The syntactic expression of substring as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="value"/> is null, or <paramref name="substring"/> is null,
        /// or <paramref name="value"/> does not contain <paramref name="substring"/>.
        /// </exception>
        public static void Contains(
            string substring,
            string value,
            string? message = "",
            [CallerArgumentExpression(nameof(substring))] string substringExpression = "",
            [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            CheckParameterNotNull(value, "Assert.Contains", "value");
            CheckParameterNotNull(substring, "Assert.Contains", "substring");

            if (!value.Contains(substring))
            {
                string userMessage = BuildUserMessageForSubstringExpressionAndValueExpression(message, substringExpression, valueExpression);
                string finalMessage = string.Format("String '{0}' does not contain string '{1}'. {2}.", value, substring, userMessage);
                ThrowAssertFailed("Assert.Contains", finalMessage);
            }
        }

        #endregion // Contains


        #region DoesNotContain

        // TODO: add after adding Linq support to nanoFramework
        ///// <summary>
        ///// Tests whether the specified collection does not contain the specified item.
        ///// </summary>
        ///// <typeparam name="T">The type of the collection items.</typeparam>
        ///// <param name="notExpected">The unexpected item.</param>
        ///// <param name="collection">The collection.</param>
        ///// <param name="message">The message to display when the assertion fails.</param>
        ///// <param name="notExpectedExpression">
        ///// The syntactic expression of notExpected as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void DoesNotContain<T>(T notExpected, IEnumerable<T> collection, string? message = "", [CallerArgumentExpression(nameof(notExpected))] string notExpectedExpression = "", [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //{
        //    if (collection.Contains(notExpected))
        //    {
        //        string userMessage = BuildUserMessageForNotExpectedExpressionAndCollectionExpression(message, notExpectedExpression, collectionExpression);
        //        ThrowAssertDoesNotContainItemFailed(userMessage);
        //    }
        //}

        /// <summary>
        /// Tests whether the specified collection does not contain the specified item.
        /// </summary>
        /// <param name="notExpected">The unexpected item.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="message">The message to display when the assertion fails.</param>
        /// <param name="notExpectedExpression">
        /// The syntactic expression of notExpected as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void DoesNotContain(
            object? notExpected,
            IEnumerable collection,
            string? message = "",
            [CallerArgumentExpression(nameof(notExpected))] string notExpectedExpression = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        {
            CheckParameterNotNull(collection, "Assert.DoesNotContain", "collection");

            foreach (object? item in collection)
            {
                if (object.Equals(notExpected, item))
                {
                    string userMessage = BuildUserMessageForNotExpectedExpressionAndCollectionExpression(message, notExpectedExpression, collectionExpression);
                    ThrowAssertDoesNotContainItemFailed(userMessage);
                }
            }
        }

        // TODO: add after adding Linq support to nanoFramework
        ///// <summary>
        ///// Tests whether the specified collection does not contain the specified item.
        ///// </summary>
        ///// <typeparam name="T">The type of the collection items.</typeparam>
        ///// <param name="notExpected">The unexpected item.</param>
        ///// <param name="collection">The collection.</param>
        ///// <param name="comparer">An equality comparer to compare values.</param>
        ///// <param name="message">The message to display when the assertion fails.</param>
        ///// <param name="notExpectedExpression">
        ///// The syntactic expression of notExpected as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void DoesNotContain<T>(T notExpected, IEnumerable<T> collection, IEqualityComparer<T> comparer, string? message = "", [CallerArgumentExpression(nameof(notExpected))] string notExpectedExpression = "", [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //{
        //    if (collection.Contains(notExpected, comparer))
        //    {
        //        string userMessage = BuildUserMessageForNotExpectedExpressionAndCollectionExpression(message, notExpectedExpression, collectionExpression);
        //        ThrowAssertDoesNotContainItemFailed(userMessage);
        //    }
        //}

        /// <summary>
        /// Tests whether the specified collection does not contain the specified item.
        /// </summary>
        /// <param name="notExpected">The unexpected item.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <param name="message">The message to display when the assertion fails.</param>
        /// <param name="notExpectedExpression">
        /// The syntactic expression of notExpected as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void DoesNotContain(
            object? notExpected,
            IEnumerable collection,
            IEqualityComparer comparer,
            string? message = "",
            [CallerArgumentExpression(nameof(notExpected))] string notExpectedExpression = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        {
            CheckParameterNotNull(collection, "Assert.DoesNotContain", "collection");
            CheckParameterNotNull(comparer, "Assert.DoesNotContain", "comparer");

            foreach (object? item in collection)
            {
                if (comparer.Equals(item, notExpected))
                {
                    string userMessage = BuildUserMessageForNotExpectedExpressionAndCollectionExpression(message, notExpectedExpression, collectionExpression);
                    ThrowAssertDoesNotContainItemFailed(userMessage);
                }
            }
        }

        // TODO: add after adding Linq support to nanoFramework
        ///// <summary>
        ///// Tests whether the specified collection does not contain the specified item.
        ///// </summary>
        ///// <typeparam name="T">The type of the collection items.</typeparam>
        ///// <param name="predicate">A function to test each element for a condition.</param>
        ///// <param name="collection">The collection.</param>
        ///// <param name="message">The message to display when the assertion fails.</param>
        ///// <param name="predicateExpression">
        ///// The syntactic expression of predicate as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void DoesNotContain<T>(Func<T, bool> predicate, IEnumerable<T> collection, string? message = "", [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "", [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //{
        //    if (collection.Any(predicate))
        //    {
        //        string userMessage = BuildUserMessageForPredicateExpressionAndCollectionExpression(message, predicateExpression, collectionExpression);
        //        ThrowAssertDoesNotContainPredicateFailed(userMessage);
        //    }
        //}

        /// <summary>
        /// Tests whether the specified collection does not contain the specified item.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="message">The message to display when the assertion fails.</param>
        /// <param name="predicateExpression">
        /// The syntactic expression of predicate as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void DoesNotContain(
            Func<object?, bool> predicate,
            IEnumerable collection,
            string? message = "",
            [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        {
            CheckParameterNotNull(collection, "Assert.DoesNotContain", "collection");
            CheckParameterNotNull(predicate, "Assert.DoesNotContain", "predicate");

            foreach (object? item in collection)
            {
                if (predicate(item))
                {
                    string userMessage = BuildUserMessageForPredicateExpressionAndCollectionExpression(message, predicateExpression, collectionExpression);
                    ThrowAssertDoesNotContainPredicateFailed(userMessage);
                }
            }
        }

        /// <summary>
        /// Tests whether the specified string does not contain the specified substring
        /// and throws an exception if the substring occurs within the
        /// test string.
        /// </summary>
        /// <param name="substring">
        /// The string expected to not occur within <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        /// The string that is expected to not contain <paramref name="substring"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="substring"/>
        /// is in <paramref name="value"/>. The message is shown in
        /// test results.
        /// </param>
        /// <param name="substringExpression">
        /// The syntactic expression of substring as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <param name="valueExpression">
        /// The syntactic expression of value as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="value"/> is null, or <paramref name="substring"/> is null,
        /// or <paramref name="value"/> contains <paramref name="substring"/>.
        /// </exception>
        public static void DoesNotContain(
            string substring,
            string value,
            string? message = "",
            [CallerArgumentExpression(nameof(substring))] string substringExpression = "",
            [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            CheckParameterNotNull(value, "Assert.DoesNotContain", "value");
            CheckParameterNotNull(substring, "Assert.DoesNotContain", "substring");

            if (value.Contains(substring))
            {
                string userMessage = BuildUserMessageForSubstringExpressionAndValueExpression(message, substringExpression, valueExpression);
                string finalMessage = string.Format("String '{0}' does contain string '{1}'. {2}.", value, substring, userMessage);
                ThrowAssertFailed("Assert.DoesNotContain", finalMessage);
            }
        }

        #endregion // DoesNotContain

        [DoesNotReturn]
        private static void ThrowAssertContainsItemFailed(string userMessage)
        {
            string finalMessage = string.Format(
                "Expected collection to contain the specified item. {0}",
                userMessage);
            ThrowAssertFailed("Assert.Contains", finalMessage);
        }

        [DoesNotReturn]
        private static void ThrowAssertContainsPredicateFailed(string userMessage)
        {
            string finalMessage = string.Format(
                "Expected at least one item to match the predicate. {0}",
                userMessage);
            ThrowAssertFailed("Assert.Contains", finalMessage);
        }

        [DoesNotReturn]
        private static void ThrowAssertDoesNotContainItemFailed(string userMessage)
        {
            string finalMessage = string.Format(
                "Expected collection to not contain the specified item. {0}",
                userMessage);
            ThrowAssertFailed("Assert.DoesNotContain", finalMessage);
        }

        [DoesNotReturn]
        private static void ThrowAssertDoesNotContainPredicateFailed(string userMessage)
        {
            string finalMessage = string.Format(
                "Expected no items to match the predicate. {0}",
                userMessage);
            ThrowAssertFailed("Assert.DoesNotContain", finalMessage);
        }
    }
}
