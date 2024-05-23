using System;
using System.Collections;
using TestFrameworkShared;

namespace nanoFramework.TestFramework
{
    public sealed partial class Assert
    {
        /// <summary>
        /// Tests whether the specified collection is empty.
        /// </summary>
        /// <param name="collection">The collection the test expects to be empty.</param>
        /// <param name="message">The message to include in the exception when the collection is empty. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Raises an exception if the collection is not empty.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the method with the same name in CollectionAssert class.")]
        public static void Empty(ICollection collection, string message = "")
        {
            if (collection.Count != 0)
            {
                HandleFail("Assert.Empty", message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(Array expected, Array actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(bool expected, bool actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(byte expected, byte actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(char expected, char actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(DateTime expected, DateTime actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(double expected, double actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(float expected, float actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(int expected, int actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(long expected, long actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(sbyte expected, sbyte actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(short expected, short actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(string expected, string actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(uint expected, uint actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(ulong expected, ulong actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are unequal. 
        /// </summary>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is not equal to <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> is not equal to <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreEqual.")]
        public static void Equal(ushort expected, ushort actual, string message = "") => AreEqual(expected, actual, message);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="condition">The condition the test expects to be false.</param>
        /// <param name="message">The message to include in the exception when condition is true. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if condition is <see langword="true"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method IsFalse.")]
        public static void False(bool condition, string message = "") => IsFalse(condition, message);

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        /// <param name="wrongType">The type that value should not be.</param>
        /// <param name="message">The message to include in the exception when value is an instance of wrongType. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="value"/> is not null and <paramref name="wrongType"/> is in the inheritance hierarchy of value.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method IsNotInstanceOfType.")]
        public static void IsNotType(Type wrongType, object value, string message = "") => IsNotInstanceOfType(value, wrongType, message);

        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the inheritance hierarchy of the object.
        /// </summary>
        /// <param name="expectedType">The expected type of value.</param>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <param name="message">The message to include in the exception when value is not an instance of expectedType. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="value"/> is <see langword="null"/> or <paramref name="expectedType"/> is not in the inheritance hierarchy of <paramref name="value"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method IsInstanceOfType.")]
        public static void IsType(Type expectedType, object value, string message = "") => IsInstanceOfType(value, expectedType, message);

        /// <summary>
        /// Tests whether the specified collection is not empty.
        /// </summary>
        /// <param name="collection">The collection the test expects not to be empty.</param>
        /// <param name="message">The message to include in the exception when the collection is not empty. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Raises an exception if the collection is not empty.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the method with the same name in CollectionAssert class.")]
        public static void NotEmpty(ICollection collection, string message = "")
        {
            if (collection.Count == 0)
            {
                HandleFail("Assert.NotEmpty", message);
            }
        }

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(Array notExpected, Array actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(bool notExpected, bool actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(byte notExpected, byte actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(char notExpected, char actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/>.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(DateTime notExpected, DateTime actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(double notExpected, double actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(float notExpected, float actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(int notExpected, int actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(long notExpected, long actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(sbyte notExpected, sbyte actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(short notExpected, short actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(string notExpected, string actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(uint notExpected, uint actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(ulong notExpected, ulong actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. 
        /// </summary>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual"/></param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="actual"/> is equal to <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> is equal <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotEqual.")]
        public static void NotEqual(ushort notExpected, ushort actual, string message = "") => AreNotEqual(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <param name="value">The object the test expects not to be null.</param>
        /// <param name="message">The message to include in the exception when value is null. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if value is null.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method IsNotNull.")]
        public static void NotNull(object value, string message = "") => IsNotNull(value, message);

        /// <summary>
        /// Tests whether the specified objects refer to different objects and throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match actual.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is the same as <paramref name="notExpected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="notExpected"/> refers to the same object as <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreNotSame.")]
        public static void NotSame(object notExpected, object actual, string message = "") => AreNotSame(notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The object the test expects to be null.</param>
        /// <param name="message">The message to include in the exception when value is not null. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if value is not null.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method IsNull.")]
        public static void Null(object value, string message = "") => IsNull(value, message);

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="expected">The first object to compare. This is the value the test expects.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not the same as <paramref name="expected"/>. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if <paramref name="expected"/> does not refer to the same object as <paramref name="actual"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method AreSame.")]
        public static void Same(object expected, object actual, string message = "") => AreSame(expected, actual, message);

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
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method ThrowsException.")]
        public static void Throws(Type exceptionType, Action action, string message = "") => ThrowsException(exceptionType, action, message);

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="condition">The condition the test expects to be true.</param>
        /// <param name="message">The message to include in the exception when condition is false. The message is shown in test results.</param>
        /// <exception cref="AssertFailedException">Thrown if condition is <see langword="false"/>.</exception>
        [Obsolete("This method is deprecated and will be removed in a future version. Use the new method IsTrue.")]
        public static void True(bool condition, string message = "") => IsTrue(condition, message);
    }
}
