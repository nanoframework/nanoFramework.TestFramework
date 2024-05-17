using System;
using System.Diagnostics.CodeAnalysis;
using TestFrameworkShared;

namespace nanoFramework.TestFramework
{
    // ReSharper disable StringCompareIsCultureSpecific.1
    public sealed partial class Assert
    {
        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(bool notExpected, bool actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(byte notExpected, byte actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(char notExpected, char actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(DateTime notExpected, DateTime actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(double notExpected, double actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(float notExpected, float actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(long notExpected, long actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(int notExpected, int actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified object are unequal and throws an exception if the two object are equal. 
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the object the tests expects.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal to <paramref name="actual"/>.</exception>
        public static void AreNotEqual(object notExpected, object actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(sbyte notExpected, sbyte actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(short notExpected, short actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(string notExpected, string actual, string message = "")
        {
            if (string.Compare(notExpected, actual) == 0)
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(uint notExpected, uint actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(ulong notExpected, ulong actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        public static void AreNotEqual(ushort notExpected, ushort actual, string message = "")
        {
            if (Equals(notExpected, actual))
            {
                HandleAreNotEqualFail(notExpected, actual, message);
            }
        }

        [DoesNotReturn]
        private static void HandleAreNotEqualFail(object expected, object actual, string message)
        {
            HandleFail("Assert.AreNotEqual", $"Expected any value except:<{ReplaceNulls(expected)}>. Actual:<{ReplaceNulls(actual)}>. {(message is null ? string.Empty : ReplaceNulls(message))}");
        }
    }
}
