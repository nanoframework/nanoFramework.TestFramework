using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TestFrameworkShared;

namespace nanoFramework.TestFramework
{
    // ReSharper disable StringCompareIsCultureSpecific.1
    public sealed partial class Assert
    {
        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(bool expected, bool actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(byte expected, byte actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(char expected, char actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(DateTime expected, DateTime actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(double expected, double actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(float expected, float actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(int expected, int actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(long expected, long actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified objects are equal and throws an exception if the two objects are unequal. 
        /// </summary>
        /// <param name="expected">The first objects to compare. This is the objects the tests expects.</param>
        /// <param name="actual">The second objects to compare. This is the objects produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(object expected, object actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(sbyte expected, sbyte actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(short expected, short actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(string expected, string actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (string.Compare(expected, actual) == 0)
            {
                return;
            }

            HandleAreEqualFail(expected, actual, message);
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(uint expected, uint actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(ulong expected, ulong actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        public static void AreEqual(ushort expected, ushort actual, [CallerArgumentExpression(nameof(actual))] string message = "")
        {
            if (!Equals(expected, actual))
            {
                HandleAreEqualFail(expected, actual, message);
            }
        }

        [DoesNotReturn]
        private static void HandleAreEqualFail(object expected, object actual, string message)
        {
            HandleFail("Assert.AreEqual", $"Expected:<{ReplaceNulls(expected)}>. Actual:<{ReplaceNulls(actual)}>. {(message is null ? string.Empty : ReplaceNulls(message))}");
        }
    }
}
