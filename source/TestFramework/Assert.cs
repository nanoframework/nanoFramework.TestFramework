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
        private const string ObjectAsString = "(object)";
        private const string NullAsString = "(null)";
        
        private const string StringContainsFailMsg = "{2} does not contains {1}. {0}";
        private const string StringDoesNotContainsFailMsg = "{2} should not contain {1}. {0}";
        private const string StringDoesNotEndWithFailMsg = "{2} does not end with {1}. {0}";
        private const string StringDoesNotStartWithFailMsg = "{2} does not start with {1}. {0}";

        /// <summary>
        /// Tests whether the specified objects refer to different objects and throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match actual.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is the same as <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> refers to the same object as <paramref name="actual"/>.</exception>
        public static void AreNotSame(object notExpected, object actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!ReferenceEquals(notExpected, actual))
            {
                return;
            }

            HandleFail("Assert.AreNotSame", ReplaceNulls(message));
        }

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="expected">The first object to compare. This is the value the test expects.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not the same as <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> does not refer to the same object as <paramref name="actual"/>.</exception>
        public static void AreSame(object expected, object actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (ReferenceEquals(expected, actual))
            {
                return;
            }

            if (expected is ValueType || actual is ValueType)
            {
                HandleFail("Assert.AreSame", $"Do not pass value types to AreSame(). Values converted to Object will never be the same. Consider using AreEqual(). {ReplaceNulls(message)}");
                
            }

            HandleFail("Assert.AreSame", ReplaceNulls(message));
        }

        internal static void EnsureParameterIsNotNull(object value, string assertion, [CallerArgumentExpression(nameof(value))] string parameter = null)
        {
            if (value is not null)
            {
                return;
            }

            HandleFail(assertion, $"The parameter '{parameter}' is invalid. The value cannot be null.");
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

        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the inheritance hierarchy of the object.
        /// </summary>
        /// <param name="expected">The expected type of value.</param>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <param name="message">The message to include in the exception when value is not an instance of expected. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="value"/> is <see langword="null"/> or <paramref name="expected"/> is not in the inheritance hierarchy of <paramref name="value"/>.</exception>
        public static void IsInstanceOfType(object value, Type expected, [CallerArgumentExpression(nameof(value))] string message = "")
        {
            EnsureParameterIsNotNull(expected, "Assert.IsInstanceOfType");

            if (value is not null && expected == value.GetType())
            {
                return;
            }

            // ReSharper disable once MergeConditionalExpression
            #pragma warning disable IDE0031 // IDE keeps suggesting I change this to value?.GetType() but since we don't have Nullable<T> this won't work in all cases.
            var actual = value is null ? null : value.GetType();
            #pragma warning restore IDE0031

            HandleFail("Assert.IsInstanceOfType", $"Expected type:<{expected}>. Actual type:<{ReplaceNulls(actual)}>. {ReplaceNulls(message)}");
        }

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        /// <param name="notExpected">The type that value should not be.</param>
        /// <param name="message">The message to include in the exception when value is an instance of notExpected. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="value"/> is not <see langword="null"/> and <paramref name="notExpected"/> is in the inheritance hierarchy of <paramref name="value"/>.</exception>
        public static void IsNotInstanceOfType(object value, Type notExpected, [CallerArgumentExpression(nameof(value))] string message = "")
        {
            EnsureParameterIsNotNull(notExpected, "Assert.IsNotInstanceOfType");

            if (value is null || notExpected != value.GetType())
            {
                return;
            }

            HandleFail("Assert.IsNotInstanceOfType", $"Wrong type:<{notExpected}>. Actual type:<{value.GetType()}>. {ReplaceNulls(message)}");
        }

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <param name="value">The object the test expects not to be null.</param>
        /// <param name="message">The message to include in the exception when value is null. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if value is null.</exception>
        public static void IsNotNull([NotNull] object value, [CallerArgumentExpression(nameof(value))] string message = "")
        {
            if (value is not null)
            {
            }

            HandleFail("Assert.IsNotNull", message);
        }

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The object the test expects to be null.</param>
        /// <param name="message">The message to include in the exception when value is not null. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if value is not null.</exception>
        public static void IsNull(object value, [CallerArgumentExpression(nameof(value))] string message = "")
        {
            if (value is null)
            {
                return;
            }

            HandleFail("Assert.IsNull", message);
        }

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

        [DoesNotReturn]
        public static void SkipTest(string message = null)
        {
            throw new SkipTestException(message);
        }

        /// <summary>
        /// Tests whether the code specified by delegate action throws exact given exception
        /// of type <paramref name="exception"/> (and not of derived type) and throws <see cref="AssertFailedException"/> if code
        /// does not throw exception or throws exception of type other than <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">Type of exception expected to be thrown.</param>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <param name="message">The message to include in the exception when action does not throw exception of type <paramref name="exception"/>.</param>
        /// <exception cref="AssertFailedException">Thrown if action does not throw exception of type <paramref name="exception"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <see langword="null"/>.</exception>
        public static void ThrowsException(Type exception, Action action, [CallerArgumentExpression(nameof(action))] string message = "")
        {
            EnsureParameterIsNotNull(action, "Assert.ThrowsException");
            EnsureParameterIsNotNull(exception, "Assert.ThrowsException");

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
                if (exception == ex.GetType())
                {
                    return;
                }
                
                HandleFail("Assert.ThrowsException", $"Threw exception {ex.GetType().Name}, but exception {exception.Name} was expected. {ReplaceNulls(message)}\r\nException Message: {ex.Message}");
            }

            HandleFail("Assert.ThrowsException", $"No exception thrown. {exception.Name} exception was expected. {ReplaceNulls(message)}");
        }

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
            EnsureParameterIsNotNull(expected, "Assert.Contains");
            EnsureParameterIsNotNull(other, "Assert.Contains");

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
            EnsureParameterIsNotNull(expected, "Assert.Contains");
            EnsureParameterIsNotNull(other, "Assert.Contains");

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
            EnsureParameterIsNotNull(expected, "Assert.Contains");
            EnsureParameterIsNotNull(other, "Assert.Contains");

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
            EnsureParameterIsNotNull(expected, "Assert.Contains");
            EnsureParameterIsNotNull(other, "Assert.Contains");

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

        [DoesNotReturn]
        internal static void HandleFail(string assertion, string message)
        {
            var text = string.Empty;

            if (!string.IsNullOrEmpty(message))
            {
                text = ReplaceNulls(message);
            }

            throw new AssertFailedException(string.Format(AssertionFailed, assertion, text));
        }

        internal static string ReplaceNulls(object input)
        {
            if (input == null)
            {
                return NullAsString;
            }

            string text = null;

            try
            {
                text = input.ToString();
            }
            catch (NotImplementedException)
            {
                // Move along
            }

            text ??= ObjectAsString;

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
    }
}
