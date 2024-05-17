////
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
////


using System.Collections;
using TestFrameworkShared;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// A collection of helper classes to test various conditions associated
    /// with collections within unit tests. If the condition being tested is not
    /// met, an exception is thrown.
    /// </summary>
    public sealed class CollectionAssert
    {
        private const string CollectionEqualReason = "{0}({1})";
        private const string NumberOfElementsDiff = "Different number of elements.";
        private const string ElementsAtIndexDontMatch = "Element at index {0} do not match. Expected:<{1}>. Actual:<{2}>.";
        private const string BothCollectionsSameReference = "Both collection references point to the same collection object. {0}";
        private const string BothCollectionsSameElements = "Both collection contain same elements.";

        #region collection

        /// <summary>
        /// Tests whether the specified collection is empty.
        /// </summary>
        /// <param name="collection">The collection the test expects to be empty.</param>
        /// <param name="message">The message to include in the exception when the collection is empty. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Raises an exception if the collection is not empty.</exception>
        /// <exception cref=""></exception>
        public static void Empty(ICollection collection, string message = "")
        {
            Assert.EnsureParameterIsNotNull(collection, "CollectionAssert.Empty");

            if (collection.Count != 0)
            {
                Assert.HandleFail("CollectionAssert.Empty", message);
            }
        }

        /// <summary>
        /// Tests whether the specified collection is not empty.
        /// </summary>
        /// <param name="collection">The collection the test expects not to be empty.</param>
        /// <param name="message">The message to include in the exception when the collection is not empty. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Raises an exception if the collection is not empty.</exception>
        public static void NotEmpty(ICollection collection, string message = "")
        {
            Assert.EnsureParameterIsNotNull(collection, "CollectionAssert.NotEmpty");

            if (collection.Count == 0)
            {
                Assert.HandleFail("CollectionAssert.NotEmpty", message);
            }
        }

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception if the two collections are not equal. Equality is defined as having the same elements in the same order and quantity. Different references to the same value are considered equal.
        /// </summary>
        /// <param name="expected">The first collection to compare. This is the collection the tests expects.</param>
        /// <param name="actual"> The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            ICollection expected,
            ICollection actual,
            string message = "")
        {
            string reason = string.Empty;

            if (!AreCollectionsEqual(expected,
                                    actual,
                                    ref reason))
            {
                Assert.HandleFail(
                    "CollectionAssert.AreEqual",
                    string.Format(CollectionEqualReason, new object[2]
                    {
                        message,
                        reason
                    }));
            }
        }

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception if the two collections are equal. Equality is defined as having the same elements in the same order and quantity. Different references to the same value are considered equal.
        /// </summary>
        /// <param name="notExpected">The first collection to compare. This is the collection the tests expects not to match <paramref name="actual"/>.</param>
        /// <param name="actual"> The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when actual is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            ICollection notExpected,
            ICollection actual,
            string message = "")
        {
            string reason = string.Empty;

            if (AreCollectionsEqual(notExpected,
                                     actual,
                                     ref reason))
            {
                Assert.HandleFail(
                    "CollectionAssert.AreNotEqual",
                    string.Format(CollectionEqualReason, new object[2]
                    {
                        message,
                        reason
                    }));
            }
        }

        #endregion region

        private static bool AreCollectionsEqual(
            ICollection expected,
            ICollection actual,
            ref string reason)
        {
            if (expected
                != actual)
            {
                if (expected == null
                    || actual == null)
                {
                    return false;
                }

                if (expected.Count
                    != actual.Count)
                {
                    reason = NumberOfElementsDiff;
                    return false;
                }

                IEnumerator enumerator = expected.GetEnumerator();
                IEnumerator enumerator2 = actual.GetEnumerator();

                int num = 0;

                while (enumerator.MoveNext()
                       && enumerator2.MoveNext())
                {
                    if (!object.Equals(
                        enumerator.Current,
                        enumerator2.Current))
                    {
                        reason = string.Format(
                            ElementsAtIndexDontMatch,
                            new object[3]
                            {
                                num,
                                enumerator.Current,
                                enumerator2.Current
                            });

                        return false;
                    }

                    num++;
                }

                reason = BothCollectionsSameElements;

                return true;
            }

            reason = string.Format(BothCollectionsSameReference,
                                   new object[1] { string.Empty });

            return true;
        }
    }
}
