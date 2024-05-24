//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using nanoFramework.TestFramework;
using NFUnitTest.Mock;
using TestFrameworkShared;

namespace NFUnitTest
{
    [TestClass]
    public class TestOfTest
    {
        [Setup]
        public void RunSetup()
        {
            Console.WriteLine("Methods with [Setup] will run before tests.");
        }

        [Cleanup]
        public void Cleanup()
        {
            Console.WriteLine("Methods with [Cleanup] will run after tests.");
        }

        [TestMethod]
        public void TestAreEqual()
        {
            Console.WriteLine("Test will check that all the AreEqual are actually equal and that AreNotEqual fails");

            // Arrange
            const bool boolA = true; const bool boolB = true;
            const byte byteA = 42; const byte byteB = 42;
            const char charA = (char)42; const char charB = (char)42;
            var dateTimeA = new DateTime(2024, 4, 20);
            var dateTimeB = new DateTime(2024, 4, 20);
            const float floatA = 42; const float floatB = 42;
            const int intA = 42; const int intB = 42;
            var intArrayA = new[] { 1, 2, 3, 4, 5 };
            var intArrayB = new[] { 1, 2, 3, 4, 5 };
            const long longA = 42; const long longB = 42;
            var objA = new object(); var objB = objA;
            const sbyte sbyteA = 42; const sbyte sbyteB = 42;
            const short shortA = 42; const short shortB = 42;
            const string stringA = "42"; const string stringB = "42";
            const uint uintA = 42; const uint uintB = 42;
            const ulong ulongA = 42; const ulong ulongB = 42;
            const ushort ushortA = 42; const ushort ushortB = 42;

            // Assert
            Assert.AreEqual(boolA, boolB);
            Assert.AreEqual(byteA, byteB);
            Assert.AreEqual(charA, charB);
            Assert.AreEqual(dateTimeA, dateTimeB);
            Assert.AreEqual(floatA, floatB);
            Assert.AreEqual(intA, intB);
            Assert.AreEqual(longA, longB);
            Assert.AreEqual(objA, objB);
            Assert.AreEqual(sbyteA, sbyteB);
            Assert.AreEqual(shortA, shortB);
            Assert.AreEqual(stringA, stringB);
            Assert.AreEqual(uintA, uintB);
            Assert.AreEqual(ulongA, ulongB);
            Assert.AreEqual(ushortA, ushortB);
            Assert.AreSame(objA, objB);
            CollectionAssert.AreEqual(intArrayA, intArrayB);

            CatchAssertException(() => Assert.AreNotEqual(boolA, boolB));
            CatchAssertException(() => Assert.AreNotEqual(byteA, byteB));
            CatchAssertException(() => Assert.AreNotEqual(charA, charB));
            CatchAssertException(() => Assert.AreNotEqual(dateTimeA, dateTimeB));
            CatchAssertException(() => Assert.AreNotEqual(floatA, floatB));
            CatchAssertException(() => Assert.AreNotEqual(intA, intB));
            CatchAssertException(() => Assert.AreNotEqual(longA, longB));
            CatchAssertException(() => Assert.AreNotEqual(objA, objB));
            CatchAssertException(() => Assert.AreNotEqual(sbyteA, sbyteB));
            CatchAssertException(() => Assert.AreNotEqual(shortA, shortB));
            CatchAssertException(() => Assert.AreNotEqual(stringA, stringB));
            CatchAssertException(() => Assert.AreNotEqual(uintA, uintB));
            CatchAssertException(() => Assert.AreNotEqual(ulongA, ulongB));
            CatchAssertException(() => Assert.AreNotEqual(ushortA, ushortB));
            CatchAssertException(() => Assert.AreNotSame(objA, objB));
            CatchAssertException(() => CollectionAssert.AreNotEqual(intArrayA, intArrayB));
        }

        [TestMethod]
        public void TestAreNotEqual()
        {
            Console.WriteLine("Test will check that all the AreNotEqual are actually not equal and AreEqual fails");

            // Arrange
            const bool boolA = true; const bool boolB = false;
            const byte byteA = 42; const byte byteB = 43;
            const char charA = (char)42; const char charB = (char)43;
            var dateTimeA = new DateTime(2024, 4, 20);
            var dateTimeB = new DateTime(2024, 4, 21);
            const float floatA = 42; const float floatB = 43;
            const int intA = 42; const int intB = 43;
            var intArrayA = new[] { 1, 2, 3, 4, 5 };
            var intArrayB = new[] { 5, 4, 3, 2, 1 };
            const long longA = 42; const long longB = 43;
            var objA = new object(); var objB = new object();
            const sbyte sbyteA = 42; const sbyte sbyteB = 43;
            const short shortA = 42; const short shortB = 43;
            const string stringA = "42"; const string stringB = "43";
            const uint uintA = 42; const uint uintB = 43;
            const ulong ulongA = 42; const ulong ulongB = 43;
            const ushort ushortA = 42; const ushort ushortB = 43;

            // Assert
            Assert.AreNotEqual(boolA, boolB);
            Assert.AreNotEqual(byteA, byteB);
            Assert.AreNotEqual(charA, charB);
            Assert.AreNotEqual(dateTimeA, dateTimeB);
            Assert.AreNotEqual(floatA, floatB);
            Assert.AreNotEqual(intA, intB);
            Assert.AreNotEqual(longA, longB);
            Assert.AreNotEqual(objA, objB);
            Assert.AreNotEqual(sbyteA, sbyteB);
            Assert.AreNotEqual(shortA, shortB);
            Assert.AreNotEqual(stringA, stringB);
            Assert.AreNotEqual(uintA, uintB);
            Assert.AreNotEqual(ulongA, ulongB);
            Assert.AreNotEqual(ushortA, ushortB);
            Assert.AreNotSame(objA, objB);
            CollectionAssert.AreNotEqual(intArrayA, intArrayB);

            CatchAssertException(() => Assert.AreEqual(boolA, boolB));
            CatchAssertException(() => Assert.AreEqual(byteA, byteB));
            CatchAssertException(() => Assert.AreEqual(charA, charB));
            CatchAssertException(() => Assert.AreEqual(dateTimeA, dateTimeB));
            CatchAssertException(() => Assert.AreEqual(floatA, floatB));
            CatchAssertException(() => Assert.AreEqual(intA, intB));
            CatchAssertException(() => Assert.AreEqual(longA, longB));
            CatchAssertException(() => Assert.AreEqual(objA, objB));
            CatchAssertException(() => Assert.AreEqual(sbyteA, sbyteB));
            CatchAssertException(() => Assert.AreEqual(shortA, shortB));
            CatchAssertException(() => Assert.AreEqual(stringA, stringB));
            CatchAssertException(() => Assert.AreEqual(uintA, uintB));
            CatchAssertException(() => Assert.AreEqual(ulongA, ulongB));
            CatchAssertException(() => Assert.AreEqual(ushortA, ushortB));
            CatchAssertException(() => Assert.AreSame(objA, objB));
            CatchAssertException(() => CollectionAssert.AreEqual(intArrayA, intArrayB));
        }

        [TestMethod]
        public void TestInstanceOfType()
        {
            var mockObject = new MockObject();
            var notMockObject = new object();

            Assert.IsInstanceOfType(mockObject, typeof(MockObject));
            Assert.IsNotInstanceOfType(notMockObject, typeof(MockObject));

            CatchAssertException(() => Assert.IsInstanceOfType(notMockObject, typeof(MockObject)));
            CatchAssertException(() => Assert.IsNotInstanceOfType(mockObject, typeof(MockObject)));
        }

        [TestMethod]
        public void TestNullNotNull()
        {
            Console.WriteLine("Test null, not null");

            // Arrange
            var nullObject = (object) null;
            var notNullObject = new object();

            // Assert
            Assert.IsNull(nullObject);
            Assert.IsNotNull(notNullObject);

            CatchAssertException(() => Assert.IsNull(notNullObject));
            CatchAssertException(() => Assert.IsNotNull(nullObject));
        }

        [TestMethod]
        public void TestStringComparison()
        {
            Console.WriteLine("Test string, Contains, EndsWith, StartWith");

            // Arrange
            const string contains = "contains";
            const string endsWithContains = "this text contains and end with contains";
            const string startsWithContains = "contains start this text";
            const string doesNotContain = "this is totally something else";
            var empty = string.Empty;

            // Assert
            Assert.Contains(contains, endsWithContains);
            Assert.EndsWith(contains, endsWithContains);
            Assert.StartsWith(contains, startsWithContains);
            Assert.DoesNotContains(contains, doesNotContain);
            Assert.DoesNotContains(contains, empty);

            CatchAssertException(() => Assert.Contains(contains, doesNotContain));
            CatchAssertException(() => Assert.EndsWith(contains, doesNotContain));
            CatchAssertException(() => Assert.StartsWith(contains, doesNotContain));
            CatchAssertException(() => Assert.DoesNotContains(contains, startsWithContains));
        }

        [TestMethod]
        public void TestThrowsException()
        {
            Console.WriteLine("Test will raise exception");

            Assert.ThrowsException(typeof(Exception), ThrowsException);
            Assert.ThrowsException(typeof(ArgumentOutOfRangeException), () =>
            {
                Console.WriteLine("To see another way of doing this");
                // This should throw an ArgumentException
                Thread.Sleep(-2);
            });

            try
            {
                Assert.ThrowsException(typeof(Exception), () => { Console.WriteLine("Nothing will be thrown"); });
            }
            catch (AssertFailedException)
            {
                Console.WriteLine("AssertFailedException raised because no exception was thrown, perfect");
            }

            try
            {
                Assert.ThrowsException(typeof(ArgumentNullException), ThrowsException);
            }
            catch (AssertFailedException)
            {
                Console.WriteLine("AssertFailedException raised because wrong exception was thrown, perfect");
            }

        }

        [TestMethod]
        public void TestTrueFalse()
        {
            Assert.IsTrue(true);
            Assert.IsFalse(false);

            CatchAssertException(() => Assert.IsTrue(false));
            CatchAssertException(() => Assert.IsFalse(true));
        }

        private static void CatchAssertException(Action action)
        {
            Assert.ThrowsException(typeof(AssertFailedException), action);
        }

        public void Nothing()
        {
            Console.WriteLine("Nothing and should not be called");
        }

        private static void ThrowsException()
        {
            throw new Exception("Test failed and it's a shame");
        }
    }

    public class SomethingElse
    {
        public void NothingReally(object value, [CallerArgumentExpression(nameof(value))] string parameter = null)
        {
            Console.WriteLine("Only classes marked with [TestClass] will run tests.");
        }
    }
}
