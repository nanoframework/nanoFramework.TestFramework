// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
        #region IsNotEmpty

        /// <summary>
        /// Tests that the collection is not empty.
        /// </summary>
        /// <typeparam name="T">The type of the collection items.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="message">The message format to display when the assertion fails.</param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void IsNotEmpty<T>(
            IEnumerable<T> collection,
            string? message = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        {
            if (collection.Any())
            {
                return;
            }

            string userMessage = BuildUserMessageForCollectionExpression(message, collectionExpression);
            ThrowAssertIsNotEmptyFailed(userMessage);
        }

        // TODO: Enable when IEnumerable.Cast<T> is available in nanoFramework
        ///// <summary>
        ///// Tests that the collection is not empty.
        ///// </summary>
        ///// <param name="collection">The collection.</param>
        ///// <param name="message">The message format to display when the assertion fails.</param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void IsNotEmpty(
        //    IEnumerable collection,
        //    string? message = "",
        //    [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //{
        //    if (collection.Cast<object>().Any())
        //    {
        //        return;
        //    }

        //    string userMessage = BuildUserMessageForCollectionExpression(message, collectionExpression);
        //    ThrowAssertIsNotEmptyFailed(userMessage);
        //}

        #endregion // IsNotEmpty

        #region HasCount

        /// <summary>
        /// Tests whether the collection has the expected count/length.
        /// </summary>
        /// <typeparam name="T">The type of the collection items.</typeparam>
        /// <param name="expected">The expected count.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="message">The message to display when the assertion fails.</param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void HasCount<T>(
            int expected,
            IEnumerable<T> collection,
            string? message = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
            => HasCount("HasCount", expected, collection, message, collectionExpression);

        // TODO: Enable when IEnumerable.Cast<T> is available in nanoFramework
        ///// <summary>
        ///// Tests whether the collection has the expected count/length.
        ///// </summary>
        ///// <param name="expected">The expected count.</param>
        ///// <param name="collection">The collection.</param>
        ///// <param name="message">The message to display when the assertion fails.</param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void HasCount(
        //    int expected,
        //    IEnumerable collection,
        //    string? message = "",
        //    [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //    => HasCount("HasCount", expected, collection, message, collectionExpression);

        #endregion // HasCount

        #region IsEmpty

        /// <summary>
        /// Tests that the collection is empty.
        /// </summary>
        /// <typeparam name="T">The type of the collection items.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="message">The message to display when the assertion fails.</param>
        /// <param name="collectionExpression">
        /// The syntactic expression of collection as given by the compiler via caller argument expression.
        /// Users shouldn't pass a value for this parameter.
        /// </param>
        public static void IsEmpty<T>(
            IEnumerable<T> collection,
            string? message = "",
            [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
            => HasCount("IsEmpty", 0, collection, message, collectionExpression);

        // TODO: Enable when IEnumerable.Cast<T> is available in nanoFramework
        ///// <summary>
        ///// Tests that the collection is empty.
        ///// </summary>
        ///// <param name="collection">The collection.</param>
        ///// <param name="message">The message to display when the assertion fails.</param>
        ///// <param name="collectionExpression">
        ///// The syntactic expression of collection as given by the compiler via caller argument expression.
        ///// Users shouldn't pass a value for this parameter.
        ///// </param>
        //public static void IsEmpty(
        //    IEnumerable collection,
        //    string? message = "",
        //    [CallerArgumentExpression(nameof(collection))] string collectionExpression = "")
        //    => HasCount("IsEmpty", 0, collection, message, collectionExpression);

        #endregion // IsEmpty


        private static void HasCount<T>(string assertionName, int expected, IEnumerable<T> collection, string? message, string collectionExpression)
        {
            int actualCount = collection.Count();
            if (actualCount == expected)
            {
                return;
            }

            string userMessage = BuildUserMessageForCollectionExpression(message, collectionExpression);

            ThrowAssertCountFailed(assertionName, expected, actualCount, userMessage);
        }

        // TODO: Enable when IEnumerable.Cast<T> is available in nanoFramework
        //private static void HasCount(string assertionName, int expected, IEnumerable collection, string? message, string collectionExpression)
        //    => HasCount(assertionName, expected, collection.Cast<object>(), message, collectionExpression);

        [DoesNotReturn]
        private static void ThrowAssertCountFailed(string assertionName, int expectedCount, int actualCount, string userMessage)
        {
            string finalMessage = string.Format(
                "Expected collection of size {1}. Actual: {2}. {0}",
                userMessage,
                expectedCount,
                actualCount);
            ThrowAssertFailed($"Assert.{assertionName}", finalMessage, expectedCount, actualCount);
        }

        [DoesNotReturn]
        private static void ThrowAssertIsNotEmptyFailed(string userMessage)
        {
            string finalMessage = string.Format(
                "Expected collection to contain any item but it is empty. {0}",
                userMessage);
            ThrowAssertFailed("Assert.IsNotEmpty", finalMessage);
        }
    }
}
