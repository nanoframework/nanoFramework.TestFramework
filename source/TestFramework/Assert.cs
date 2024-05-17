//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TestFrameworkShared;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// A collection of helper classes to test various conditions within unit tests. If the condition being tested is not met, an exception is thrown.
    /// </summary>
    public sealed partial class Assert
    {
        private const string AssertionFailed = "{0} failed. {1}";
        private const string Common_NullInMessages = "(null)";
        private const string Common_ObjectString = "(object)";
        private const string WrongExceptionThrown = "Threw exception {2}, but exception {1} was expected. {0}\r\nException Message: {3}";
        private const string NoExceptionThrown = "No exception thrown. {1} exception was expected. {0}";
        private const string AreSameGivenValues = "Do not pass value types to AreSame(). Values converted to Object will never be the same. Consider using AreEqual(). {0}";
        private const string IsNotInstanceOfFailMsg = "Wrong Type:<{1}>. Actual type:<{2}>. {0}";
        private const string IsInstanceOfFailMsg = "{0} Expected type:<{1}>. Actual type:<{2}>.";
        private const string StringContainsFailMsg = "{2} does not contains {1}. {0}";
        private const string StringDoesNotContainsFailMsg = "{2} should not contain {1}. {0}";
        private const string StringDoesNotEndWithFailMsg = "{2} does not end with {1}. {0}";
        private const string StringDoesNotStartWithFailMsg = "{2} does not start with {1}. {0}";
        private const string AreEqualFailMsg = "Expected:<{1}>. Actual:<{2}>. {0}";
        private const string AreNotEqualFailMsg = "Expected any value except:<{1}>. Actual:<{2}>. {0}";
        private const string NullParameterToAssert = "The parameter '{0}' is invalid. The value cannot be null. {1}.";

        #region SkipTest

        public static void SkipTest(string message = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new SkipTestException();
            }

            throw new SkipTestException(message);
        }

        #endregion

        #region true/false

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="condition">The condition the test expects to be true.</param>
        /// <param name="message">The message to include in the exception when condition is false. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if condition is <see langword="false"/>.</exception>
        public static void IsTrue(bool condition, [CallerArgumentExpression(nameof(condition))] string message = "")
        {
            if (!condition)
            {
                HandleFail("Assert.IsTrue", message);
            }
        }

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="condition">The condition the test expects to be false.</param>
        /// <param name="message">The message to include in the exception when condition is true. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if condition is <see langword="true"/>.</exception>
        public static void IsFalse(bool condition, [CallerArgumentExpression(nameof(condition))] string message = "")
        {
            if (condition)
            {
                HandleFail("Assert.IsFalse", message);
            }
        }
        #endregion

        #region Equal

        /// <summary>
        /// Tests whether the specified objects are equal and throws an exception if the two objects are unequal. 
        /// </summary>
        /// <param name="expected">The first objects to compare. This is the objects the tests expects.</param>
        /// <param name="actual">The second objects to compare. This is the objects produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            object expected,
            object actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            bool expected,
            bool actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            int expected,
            int actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            Array expected,
            Array actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            uint expected,
            uint actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            short expected,
            short actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            ushort expected,
            ushort actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            long expected,
            long actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            ulong expected,
            ulong actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            byte expected,
            byte actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            char expected,
            char actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            sbyte expected,
            sbyte actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            double expected,
            double actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            float expected,
            float actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            string expected,
            string actual,
            string message = "")
        {
            if (string.Compare(expected, actual) == 0)
            {
                return;
            }

            HandleAreEqualFail(
                expected,
                actual,
                message);
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(
            DateTime expected,
            DateTime actual,
            string message = "")
        {
            if (!object.Equals(expected, actual))
            {
                HandleAreEqualFail(
                    expected,
                    actual,
                    message);
            }
        }
        #endregion

        #region NotEqual

        /// <summary>
        /// Tests whether the specified object are unequal and throws an exception if the two object are equal. 
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the object the tests expects.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            object notExpected,
            object actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            bool notExpected,
            bool actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            int notExpected,
            int actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            Array notExpected,
            Array actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            uint notExpected,
            uint actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            short notExpected,
            short actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            ushort notExpected,
            ushort actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            long notExpected,
            long actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }


        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            ulong notExpected,
            ulong actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            byte notExpected,
            byte actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            char notExpected,
            char actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            sbyte notExpected,
            sbyte actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            double notExpected,
            double actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            float notExpected,
            float actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            string notExpected,
            string actual,
            string message = "")
        {
            if (string.Compare(notExpected, actual) == 0)
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(
            DateTime notExpected,
            DateTime actual,
            string message = "")
        {
            if (object.Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(
                    notExpected,
                    actual,
                    message);
            }
        }
        #endregion

        #region string

        /// <summary>
        /// Tests whether a string contains another string.
        /// </summary>
        /// <param name="expected">The string that is expected to be found on the <paramref name="other"/> string.</param>
        /// <param name="other">The string to check for the <paramref name="expected"/> string.</param>
        /// <param name="message">The message to include in the exception when the <paramref name="expected"/> string is not contained in the <paramref name="other"/> string. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if the <paramref name="other"/> string contains the <paramref name="expected"/> string.</exception>
        public static void Contains(
            string expected,
            string other,
            string message = "")
        {
            Assert.CheckParameterNotNull(expected, "Assert.Contains", "expected", string.Empty);
            Assert.CheckParameterNotNull(other, "Assert.Contains", "other", string.Empty);

            if (other.IndexOf(expected) < 0)
            {
                string message2 = string.Format(StringContainsFailMsg, new object[3]
                {
                    (message == null) ? string.Empty : ReplaceNulls(message),
                    ReplaceNulls(expected),
                    ReplaceNulls(other)
                });

                HandleFail("Assert.Contains", message2);
            }
        }

        /// <summary>
        /// Tests whether a string doesn't contain another string.
        /// </summary>
        /// <param name="expected">The string that is not expected to be found on the <paramref name="other"/> string.</param>
        /// <param name="other">The string to check for the <paramref name="expected"/> string.</param>
        /// <param name="message">The message to include in the exception when the <paramref name="expected"/> string is contained in the <paramref name="other"/> string. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if the <paramref name="other"/> string does not contain the <paramref name="expected"/> string.</exception>
        public static void DoesNotContains(
            string expected,
            string other,
            string message = "")
        {
            Assert.CheckParameterNotNull(expected, "Assert.Contains", "expected", string.Empty);
            Assert.CheckParameterNotNull(other, "Assert.Contains", "other", string.Empty);

            if (other.IndexOf(expected) >= 0)
            {
                string message2 = string.Format(StringDoesNotContainsFailMsg, new object[3]
                {
                    (message == null) ? string.Empty : ReplaceNulls(message),
                    ReplaceNulls(expected),
                    ReplaceNulls(other)
                });

                HandleFail("Assert.DoesNotContains", message2);
            }
        }

        /// <summary>
        /// Tests whether a string ends with another string.
        /// </summary>
        /// <param name="expected">The string that is expected to be found at the end of the <paramref name="other"/> string.</param>
        /// <param name="other">The string to check for the <paramref name="expected"/> string.</param>
        /// <param name="message">The message to include in the exception when the <paramref name="expected"/> string is not found at the end of the <paramref name="other"/> string. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if the <paramref name="other"/> string does not end with the <paramref name="expected"/> string.</exception>
        public static void EndsWith(
            string expected,
            string other,
            string message = "")
        {
            Assert.CheckParameterNotNull(expected, "Assert.Contains", "expected", string.Empty);
            Assert.CheckParameterNotNull(other, "Assert.Contains", "other", string.Empty);

            // We have to take the last index as the text can contains multiple times the same word
            if (other.LastIndexOf(expected) == other.Length - expected.Length)
            {
                return;
            }

            string message2 = string.Format(StringDoesNotEndWithFailMsg, new object[3]
            {
                (message == null) ? string.Empty : ReplaceNulls(message),
                ReplaceNulls(expected),
                ReplaceNulls(other)
            });

            HandleFail("Assert.EndsWith", message2);
        }

        /// <summary>
        /// Tests whether a string starts with another string.
        /// </summary>
        /// <param name="expected">The string that is expected to be found at the beginning of the <paramref name="other"/> string.</param>
        /// <param name="other">The string to check for the <paramref name="expected"/> string.</param>
        /// <param name="message">The message to include in the exception when the <paramref name="expected"/> string is not found at the beginning of the <paramref name="other"/> string. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if the <paramref name="other"/> string does not start with the <paramref name="expected"/> string.</exception>
        public static void StartsWith(
            string expected,
            string other,
            string message = "")
        {
            Assert.CheckParameterNotNull(expected, "Assert.Contains", "expected", string.Empty);
            Assert.CheckParameterNotNull(other, "Assert.Contains", "other", string.Empty);

            if (other.IndexOf(expected) == 0)
            {
                return;
            }

            string message2 = string.Format(StringDoesNotStartWithFailMsg, new object[3]
            {
                (message == null) ? string.Empty : ReplaceNulls(message),
                ReplaceNulls(expected),
                ReplaceNulls(other)
            });

            HandleFail("Assert.StartsWith", message2);
        }

        #endregion

        #region types, objects

        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the inheritance hierarchy of the object.
        /// </summary>
        /// <param name="expectedType">The expected type of value.</param>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <param name="message">The message to include in the exception when value is not an instance of expectedType. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="value"/> is <see langword="null"/> or <paramref name="expectedType"/> is not in the inheritance hierarchy of <paramref name="value"/>.</exception>
        public static void IsInstanceOfType(
            object value,
            Type expectedType,
            string message = "")
        {
            if (expectedType == null || value == null)
            {
                HandleFail("Assert.IsInstanceOfType", message);
            }

            if (expectedType != value.GetType())
            {
                string message2 = string.Format(IsInstanceOfFailMsg, new object[3]
                {
                    (message == null) ? string.Empty : ReplaceNulls(message),
                    expectedType,
                    value.GetType()
                });

                HandleFail("Assert.IsInstanceOfType", message2);
            }
        }

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        /// <param name="wrongType">The type that value should not be.</param>
        /// <param name="message">The message to include in the exception when value is an instance of wrongType. The message is shown in test results./param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="value"/> is not <see langword="null"/> and <paramref name="wrongType"/> is in the inheritance hierarchy of <paramref name="value"/>.</exception>
        public static void IsNotInstanceOfType(
            object value,
            Type wrongType,
            string message = "")
        {
            if ((object)wrongType == null)
            {
                HandleFail("Assert.IsNotInstanceOfType", message);
            }

            if (value != null)
            {
                if (wrongType != value.GetType())
                {
                    string message2 = string.Format(IsNotInstanceOfFailMsg, new object[3]
                    {
                        (message == null) ? string.Empty : ReplaceNulls(message),
                        wrongType,
                        value.GetType()
                    });

                    HandleFail("Assert.IsNotInstanceOfType", message2);
                }
            }
        }

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="expected">The first object to compare. This is the value the test expects.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not the same as <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> does not refer to the same object as <paramref name="actual"/>.</exception>
        public static void AreSame(
            object expected,
            object actual,
            string message = "")
        {
            if (expected != actual)
            {
                string message2 = message;

                if (expected is ValueType && actual is ValueType)
                {
                    message2 = string.Format(AreSameGivenValues, new object[1] { (message == null) ? string.Empty : ReplaceNulls(message) });
                }

                HandleFail("Assert.AreSame", message2);
            }
        }

        /// <summary>
        /// Tests whether the specified objects refer to different objects and throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match actual.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is the same as <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> refers to the same object as <paramref name="actual"/>.</exception>
        public static void AreNotSame(
            object notExpected,
            object actual,
            string message = "")
        {
            if (notExpected == actual)
            {
                HandleFail("Assert.AreNotSame", message);
            }
        }

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The object the test expects to be null.</param>
        /// <param name="message">The message to include in the exception when value is not null. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if value is not null.</exception>
        public static void IsNull(object value, [CallerArgumentExpression(nameof(value))] string message = "")
        {
            if (value is not null)
            {
                HandleFail("Assert.IsNull", message);
            }
        }

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <param name="value">The object the test expects not to be null.</param>
        /// <param name="message">The message to include in the exception when value is null. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if value is null.</exception>
        public static void IsNotNull([NotNull] object value, [CallerArgumentExpression(nameof(value))] string message = "")
        {
            if (value is null)
            {
                HandleFail("Assert.IsNotNull", message);
            }
        }

        /// <summary>
        /// Tests whether the code specified by delegate action throws exact given exception
        /// of type <paramref name="exceptionType"/> (and not of derived type) and throws <see cref="AssertFailedException"/> if code
        /// does not throw exception or throws exception of type other than <paramref name="exceptionType"/>.
        /// </summary>
        /// <param name="exceptionType">Type of exception expected to be thrown.</param>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <param name="message">The message to include in the exception when action does not throw exception of type <paramref name="exceptionType"/>.</param>
        /// <exception cref="AssertFailedException">Thrown if action does not throw exception of type <paramref name="exceptionType"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <see langword="null"/>.</exception>
        public static void ThrowsException(
            Type exceptionType,
            Action action,
            string message = "")
        {
            string empty = string.Empty;

            if (action == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == exceptionType)
                {
                    return;
                }

                empty = string.Format(WrongExceptionThrown, ReplaceNulls(message), exceptionType.Name, ex.GetType().Name, ex.Message);
                HandleFail("Assert.ThrowsException", empty);
            }

            empty = string.Format(NoExceptionThrown, new object[2]
            {
                ReplaceNulls(message),
                exceptionType.Name
            });

            HandleFail("Assert.ThrowsException", empty);
        }
        #endregion

        internal static void HandleFail(string assertionName, string message)
        {
            string text = string.Empty;

            if (!string.IsNullOrEmpty(message))
            {
                text = ReplaceNulls(message);
            }

            throw new AssertFailedException(string.Format(AssertionFailed, new object[2] { assertionName, text }));
        }

        internal static string ReplaceNulls(object input)
        {
            if (input == null)
            {
                return Common_NullInMessages;
            }

            string text = input.ToString();
            if (text == null)
            {
                return Common_ObjectString;
            }

            return ReplaceNullChars(text);
        }

        internal static string ReplaceNullChars(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string replacedString = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\0')
                {
                    replacedString += "\\0";
                }
                else
                {
                    replacedString += input[i];
                }
            }

            return replacedString.ToString();
        }

        private static void HandleAreEqualFail(
            object expected,
            object actual,
            string message)
        {
            string message2 = string.Format(AreEqualFailMsg, new object[3]
            {
                (message == null) ? string.Empty : ReplaceNulls(message),
                ReplaceNulls(expected),
                ReplaceNulls(actual)
            });

            HandleFail("Assert.AreEqual", message2);
        }

        private static void HandleAreNotEqualFail(
            object expected,
            object actual,
            string message)
        {
            string message2 = string.Format(AreNotEqualFailMsg, new object[3]
            {
                (message == null) ? string.Empty : ReplaceNulls(message),
                ReplaceNulls(expected),
                ReplaceNulls(actual)
            });

            HandleFail("Assert.AreNotEqual", message2);
        }

        internal static void CheckParameterNotNull(
            object param,
            string assertionName,
            string parameterName,
            string message)
        {
            if (param == null)
            {
                HandleFail(
                    assertionName,
                    string.Format(NullParameterToAssert, new object[2]
                    {
                        parameterName,
                        message
                    }));
            }
        }
    }
}
