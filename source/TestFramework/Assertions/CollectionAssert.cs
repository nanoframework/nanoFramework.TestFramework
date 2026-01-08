// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
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
        #region Membership

        /// <summary>
        /// Tests whether the specified collection contains the specified element
        /// and throws an exception if the element is not in the collection.
        /// </summary>
        /// <param name="collection">
        /// The collection in which to search for the element.
        /// </param>
        /// <param name="element">
        /// The element that is expected to be in the collection.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="collection"/> is null, or <paramref name="collection"/> does not contain
        /// element <paramref name="element"/>.
        /// </exception>
        public static void Contains([NotNull] ICollection? collection, object? element)
            => Contains(collection, element, string.Empty);

        /// <summary>
        /// Tests whether the specified collection contains the specified element
        /// and throws an exception if the element is not in the collection.
        /// </summary>
        /// <param name="collection">
        /// The collection in which to search for the element.
        /// </param>
        /// <param name="element">
        /// The element that is expected to be in the collection.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="element"/>
        /// is not in <paramref name="collection"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="collection"/> is null, or <paramref name="collection"/> does not contain
        /// element <paramref name="element"/>.
        /// </exception>
        public static void Contains([NotNull] ICollection? collection, object? element, string? message)
        {
            CheckParameterNotNull(collection, "CollectionAssert.Contains", "collection");

            foreach (object? current in collection)
            {
                if (Equals(current, element))
                {
                    return;
                }
            }

            ThrowAssertFailed("CollectionAssert.Contains", BuildUserMessage(message));
        }

        /// <summary>
        /// Tests whether the specified collection does not contain the specified
        /// element and throws an exception if the element is in the collection.
        /// </summary>
        /// <param name="collection">
        /// The collection in which to search for the element.
        /// </param>
        /// <param name="element">
        /// The element that is expected not to be in the collection.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="collection"/> is null, or <paramref name="collection"/> contains
        /// element <paramref name="element"/>.
        /// </exception>
        public static void DoesNotContain([NotNull] ICollection? collection, object? element)
            => DoesNotContain(collection, element, string.Empty);

        /// <summary>
        /// Tests whether the specified collection does not contain the specified
        /// element and throws an exception if the element is in the collection.
        /// </summary>
        /// <param name="collection">
        /// The collection in which to search for the element.
        /// </param>
        /// <param name="element">
        /// The element that is expected not to be in the collection.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="element"/>
        /// is in <paramref name="collection"/>. The message is shown in test
        /// results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="collection"/> is null, or <paramref name="collection"/> contains
        /// element <paramref name="element"/>.
        /// </exception>
        public static void DoesNotContain([NotNull] ICollection? collection, object? element, string? message)
        {
            CheckParameterNotNull(collection, "CollectionAssert.DoesNotContain", "collection");

            foreach (object? current in collection)
            {
                if (Equals(current, element))
                {
                    ThrowAssertFailed("CollectionAssert.DoesNotContain", BuildUserMessage(message));
                }
            }
        }

        /// <summary>
        /// Tests whether all items in the specified collection are non-null and throws
        /// an exception if any element is null.
        /// </summary>
        /// <param name="collection">
        /// The collection in which to search for null elements.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="collection"/> is null, or <paramref name="collection"/> contains a null element.
        /// </exception>
        public static void AllItemsAreNotNull([NotNull] ICollection? collection)
            => AllItemsAreNotNull(collection, string.Empty);

        /// <summary>
        /// Tests whether all items in the specified collection are non-null and throws
        /// an exception if any element is null.
        /// </summary>
        /// <param name="collection">
        /// The collection in which to search for null elements.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="collection"/>
        /// contains a null element. The message is shown in test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// <paramref name="collection"/> is null, or <paramref name="collection"/> contains a null element.
        /// </exception>
        public static void AllItemsAreNotNull([NotNull] ICollection? collection, string? message)
        {
            CheckParameterNotNull(collection, "CollectionAssert.AllItemsAreNotNull", "collection");
            foreach (object? current in collection)
            {
                if (current == null)
                {
                    ThrowAssertFailed("CollectionAssert.AllItemsAreNotNull", BuildUserMessage(message));
                }
            }
        }

        // TODO: Enable when Dictionary is available in nanoFramework
        ///// <summary>
        ///// Tests whether all items in the specified collection are unique or not and
        ///// throws if any two elements in the collection are equal.
        ///// </summary>
        ///// <param name="collection">
        ///// The collection in which to search for duplicate elements.
        ///// </param>
        ///// <exception cref="AssertFailedException">
        ///// <paramref name="collection"/> is null, or <paramref name="collection"/> contains at least one duplicate
        ///// element.
        ///// </exception>
        //public static void AllItemsAreUnique([NotNull] ICollection? collection)
        //    => AllItemsAreUnique(collection, string.Empty);

        // TODO: Enable when Dictionary is available in nanoFramework
        ///// <summary>
        ///// Tests whether all items in the specified collection are unique or not and
        ///// throws if any two elements in the collection are equal.
        ///// </summary>
        ///// <param name="collection">
        ///// The collection in which to search for duplicate elements.
        ///// </param>
        ///// <param name="message">
        ///// The message to include in the exception when <paramref name="collection"/>
        ///// contains at least one duplicate element. The message is shown in
        ///// test results.
        ///// </param>
        ///// <exception cref="AssertFailedException">
        ///// <paramref name="collection"/> is null, or <paramref name="collection"/> contains at least one duplicate
        ///// element.
        ///// </exception>
        //public static void AllItemsAreUnique([NotNull] ICollection? collection, string? message)
        //{
        //    Assert.CheckParameterNotNull(collection, "CollectionAssert.AllItemsAreUnique", "collection");

        //    message = Assert.ReplaceNulls(message);

        //    bool foundNull = false;
        //    Dictionary<object, bool> table = [];
        //    foreach (object? current in collection)
        //    {
        //        if (current == null)
        //        {
        //            if (!foundNull)
        //            {
        //                foundNull = true;
        //            }
        //            else
        //            {
        //                // Found a second occurrence of null.
        //                string userMessage = Assert.BuildUserMessage(message);
        //                string finalMessage = string.Format(
        //                    CultureInfo.CurrentCulture,
        //                    FrameworkMessages.AllItemsAreUniqueFailMsg,
        //                    userMessage,
        //                    FrameworkMessages.Common_NullInMessages);

        //                Assert.ThrowAssertFailed("CollectionAssert.AllItemsAreUnique", finalMessage);
        //            }
        //        }
        //        else
        //        {
        //            if (!table.TryAdd(current, true))
        //            {
        //                string userMessage = Assert.BuildUserMessage(message);
        //                string finalMessage = string.Format(
        //                    CultureInfo.CurrentCulture,
        //                    FrameworkMessages.AllItemsAreUniqueFailMsg,
        //                    userMessage,
        //                    Assert.ReplaceNulls(current));

        //                Assert.ThrowAssertFailed("CollectionAssert.AllItemsAreUnique", finalMessage);
        //            }
        //        }
        //    }
        //}

        #endregion

        #region AreEqual

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception
        /// if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity. Whether two elements are the same
        /// is checked using <see cref="object.Equals(object, object)" /> method.
        /// Different references to the same value are considered equal.
        /// </summary>
        /// <param name="expected">
        /// The first collection to compare. This is the collection the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> is not equal to
        /// <paramref name="actual"/>.
        /// </exception>
        public static void AreEqual(
            ICollection? expected,
            ICollection? actual)
            => AreEqual(expected, actual, string.Empty);

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception
        /// if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity. Whether two elements are the same
        /// is checked using <see cref="object.Equals(object, object)" /> method.
        /// Different references to the same value are considered equal.
        /// </summary>
        /// <param name="expected">
        /// The first collection to compare. This is the collection the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not equal to <paramref name="expected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> is not equal to
        /// <paramref name="actual"/>.
        /// </exception>
        public static void AreEqual(
            ICollection? expected,
            ICollection? actual,
            string? message)
        {
            string reason = string.Empty;
            if (!AreCollectionsEqual(expected, actual, new ObjectComparer(), ref reason))
            {
                string finalMessage = ConstructFinalMessage(reason, message);
                ThrowAssertFailed("CollectionAssert.AreEqual", finalMessage);
            }
        }

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception
        /// if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity. Whether two elements are the same
        /// is checked using <see cref="object.Equals(object, object)" /> method.
        /// Different references to the same value are considered equal.
        /// </summary>
        /// <param name="notExpected">
        /// The first collection to compare. This is the collection the tests expects
        /// not to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static void AreNotEqual(
            ICollection? notExpected,
            ICollection? actual)
            => AreNotEqual(notExpected, actual, string.Empty);

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception
        /// if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity. Whether two elements are the same
        /// is checked using <see cref="object.Equals(object, object)" /> method.
        /// Different references to the same value are considered equal.
        /// </summary>
        /// <param name="notExpected">
        /// The first collection to compare. This is the collection the tests expects
        /// not to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static void AreNotEqual(
            ICollection? notExpected,
            ICollection? actual,
            string? message)
        {
            string reason = string.Empty;
            if (AreCollectionsEqual(notExpected, actual, new ObjectComparer(), ref reason))
            {
                string finalMessage = ConstructFinalMessage(reason, message);
                ThrowAssertFailed("CollectionAssert.AreNotEqual", finalMessage);
            }
        }

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception
        /// if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity. Different references to the same
        /// value are considered equal.
        /// </summary>
        /// <param name="expected">
        /// The first collection to compare. This is the collection the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="comparer">
        /// The compare implementation to use when comparing elements of the collection.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> is not equal to
        /// <paramref name="actual"/>.
        /// </exception>
        public static void AreEqual(
            ICollection? expected,
            ICollection? actual,
            [NotNull] IComparer? comparer)
            => AreEqual(expected, actual, comparer, string.Empty);

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception
        /// if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity. Different references to the same
        /// value are considered equal.
        /// </summary>
        /// <param name="expected">
        /// The first collection to compare. This is the collection the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="comparer">
        /// The compare implementation to use when comparing elements of the collection.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not equal to <paramref name="expected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="expected"/> is not equal to
        /// <paramref name="actual"/>.
        /// </exception>
        public static void AreEqual(
            ICollection? expected,
            ICollection? actual,
            [NotNull] IComparer? comparer,
            string? message)
        {
            string reason = string.Empty;
            if (!AreCollectionsEqual(expected, actual, comparer, ref reason))
            {
                string finalMessage = ConstructFinalMessage(reason, message);
                ThrowAssertFailed("CollectionAssert.AreEqual", finalMessage);
            }
        }

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception
        /// if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity. Different references to the same
        /// value are considered equal.
        /// </summary>
        /// <param name="notExpected">
        /// The first collection to compare. This is the collection the tests expects
        /// not to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="comparer">
        /// The compare implementation to use when comparing elements of the collection.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static void AreNotEqual(
            ICollection? notExpected,
            ICollection? actual,
            [NotNull] IComparer? comparer)
            => AreNotEqual(notExpected, actual, comparer, string.Empty);

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception
        /// if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity. Different references to the same
        /// value are considered equal.
        /// </summary>
        /// <param name="notExpected">
        /// The first collection to compare. This is the collection the tests expects
        /// not to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second collection to compare. This is the collection produced by the
        /// code under test.
        /// </param>
        /// <param name="comparer">
        /// The compare implementation to use when comparing elements of the collection.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <exception cref="AssertFailedException">
        /// Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.
        /// </exception>
        public static void AreNotEqual(ICollection? notExpected, ICollection? actual, [NotNull] IComparer? comparer, string? message)
        {
            string reason = string.Empty;
            if (AreCollectionsEqual(notExpected, actual, comparer, ref reason))
            {
                string finalMessage = ConstructFinalMessage(reason, message);
                ThrowAssertFailed("CollectionAssert.AreNotEqual", finalMessage);
            }
        }

        #endregion

        private static string ConstructFinalMessage(
            string reason,
            string? message)
        {
            string userMessage = BuildUserMessage(message);
            return userMessage.Length == 0
                ? reason
                : string.Format("{0}. {1}", userMessage, reason);
        }

        /// <summary>
        /// compares the objects using object.Equals.
        /// </summary>
        private sealed class ObjectComparer : IComparer
        {
            int IComparer.Compare(object? x, object? y) => Equals(x, y) ? 0 : -1;
        }

        private sealed class EnumeratorState
        {
            public IEnumerator ExpectedEnumerator;
            public IEnumerator ActualEnumerator;
            public int Position;

            public EnumeratorState(IEnumerator expectedEnumerator, IEnumerator actualEnumerator, int position)
            {
                ExpectedEnumerator = expectedEnumerator;
                ActualEnumerator = actualEnumerator;
                Position = position;
            }
        }

        private sealed class SimpleStack
        {
            private EnumeratorState[] _items;
            private int _count;

            public SimpleStack()
            {
                _items = new EnumeratorState[4];
                _count = 0;
            }

            public int Count => _count;

            public void Push(EnumeratorState item)
            {
                if (_count == _items.Length)
                {
                    var newItems = new EnumeratorState[_items.Length * 2];
                    for (int i = 0; i < _items.Length; i++)
                    {
                        newItems[i] = _items[i];
                    }
                    _items = newItems;
                }
                _items[_count++] = item;
            }

            public EnumeratorState Pop()
            {
                return _items[--_count];
            }
        }

        private static bool AreCollectionsEqual(ICollection? expected, ICollection? actual, [NotNull] IComparer? comparer,
            ref string reason)
        {
            CheckParameterNotNull(comparer, "Assert.AreCollectionsEqual", "comparer");
            if (ReferenceEquals(expected, actual))
            {
                reason = string.Format("Both collection references point to the same collection object. {0}", string.Empty);
                return true;
            }

            return CompareIEnumerable(expected, actual, comparer, ref reason);
        }

        private static bool CompareIEnumerable(IEnumerable? expected, IEnumerable? actual, IComparer comparer, ref string reason)
        {
            if ((expected == null) || (actual == null))
            {
                return false;
            }

            var stack = new SimpleStack();
            stack.Push(new EnumeratorState(expected.GetEnumerator(), actual.GetEnumerator(), 0));

            while (stack.Count > 0)
            {
                EnumeratorState cur = stack.Pop();
                IEnumerator expectedEnum = cur.ExpectedEnumerator;
                IEnumerator actualEnum = cur.ActualEnumerator;
                int position = cur.Position;

                while (expectedEnum.MoveNext())
                {
                    if (!actualEnum.MoveNext())
                    {
                        reason = "Different number of elements.";
                        return false;
                    }

                    object? curExpected = expectedEnum.Current;
                    object? curActual = actualEnum.Current;
                    if (comparer.Compare(curExpected, curActual) == 0)
                    {
                        position++;
                    }
                    else if (curExpected is IEnumerable curExpectedEnum && curActual is IEnumerable curActualEnum)
                    {
                        stack.Push(new EnumeratorState(expectedEnum, actualEnum, position + 1));
                        stack.Push(new EnumeratorState(curExpectedEnum.GetEnumerator(), curActualEnum.GetEnumerator(), 0));
                    }
                    else if (comparer.Compare(curExpected, curActual) != 0)
                    {
                        reason = string.Format(
                            "Element at index {0} do not match.\r\nExpected: {1}\r\nActual: {2}",
                            position,
                            ReplaceNulls(curExpected),
                            ReplaceNulls(curActual));
                        return false;
                    }
                }

                if (actualEnum.MoveNext() && !expectedEnum.MoveNext())
                {
                    reason = "Different number of elements.";
                    return false;
                }
            }

            reason = "Both collection contain same elements.";
            return true;
        }
    }
}
