//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// A static Assert class as a helper for tests
    /// </summary>
    public class Assert
    {

        #region true/false

        /// <summary>
        /// Check if a condition is true
        /// </summary>
        /// <param name="condition">The condition to check</param>
        /// <exception cref="Exception">Raises an exception if the condition is not true</exception>
        public static void True(bool condition, string message = "")
        {
            if (condition)
            {
                return;
            }

            throw new Exception($"{condition} is not true. {message}");
        }

        /// <summary>
        /// Check if a condition is false
        /// </summary>
        /// <param name="condition">The condition to check</param>
        /// <exception cref="Exception">Raises an exception if the condition is not false</exception>
        public static void False(bool condition, string message = "")
        {
            if (!condition)
            {
                return;
            }

            throw new Exception($"{condition} is not false. {message}");
        }

        #endregion

        #region Equal

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(bool a, bool b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(int a, int b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(Array a, Array b, string message = "")
        {
            if (a.SequenceEqual(b))
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(uint a, uint b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(short a, short b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(ushort a, ushort b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(long a, long b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(ulong a, ulong b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(byte a, byte b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(char a, char b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(sbyte a, sbyte b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(double a, double b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(float a, float b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void Equal(string a, string b, string message = "")
        {
            if (a == b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        #endregion

        #region NotEqual

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(bool a, bool b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(int a, int b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(Array a, Array b, string message = "")
        {
            if (!a.SequenceEqual(b))
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are not equal</exception>
        public static void NotEqual(uint a, uint b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(short a, short b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(ushort a, ushort b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(long a, long b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(ulong a, ulong b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(byte a, byte b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(char a, char b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(sbyte a, sbyte b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(double a, double b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(float a, float b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        /// <summary>
        /// Check if both elements are not equal
        /// </summary>
        /// <param name="a">First element</param>
        /// <param name="b">Second element</param>
        /// <exception cref="Exception">Raises an exception if both elements are equal</exception>
        public static void NotEqual(string a, string b, string message = "")
        {
            if (a != b)
            {
                return;
            }

            throw new Exception($"{a} is not equal to {b}. {message}");
        }

        #endregion

        #region string

        /// <summary>
        /// Check if a string is included in another string
        /// </summary>
        /// <param name="expected">The expected string</param>
        /// <param name="actual">The actual string to check</param>
        /// <exception cref="Exception">Raises an exception if the expected string is not included in the actual</exception>
        public static void Contains(string expected, string actual, string message = "")
        {
            if (actual.IndexOf(expected) >= 0)
            {
                return;
            }

            throw new Exception($"{actual} does not contains {expected}. {message}");
        }

        /// <summary>
        /// Check if a string does not contains another string
        /// </summary>
        /// <param name="expected">The expected string</param>
        /// <param name="actual">The actual string to check</param>
        /// <exception cref="Exception">Raises an exception if the expected string is included in the actual</exception>
        public static void DoesNotContains(string expected, string actual, string message = "")
        {
            if (actual == null)
            {
                return;
            }

            if (actual.IndexOf(expected) < 0)
            {
                return;
            }

            throw new Exception($"{actual} does not contains {expected}. {message}");
        }

        /// <summary>
        /// Check is a string ends with another string
        /// </summary>
        /// <param name="expected">The expected string</param>
        /// <param name="actual">The actual string to check</param>
        /// <exception cref="Exception">Raises an exception if the expected string is not at the end of the actual string</exception>
        public static void EndsWith(string expected, string actual, string message = "")
        {
            // We have to take the last index as the text can contains multiple times the same word
            if (actual.LastIndexOf(expected) == actual.Length - expected.Length)
            {
                return;
            }

            throw new Exception($"{actual} does not ends with {expected}. {message}");
        }

        /// <summary>
        /// Check is a string starts with another string
        /// </summary>
        /// <param name="expected">The expected string</param>
        /// <param name="actual">The actual string to check</param>
        /// <exception cref="Exception">Raises an exception if the expected string is not at the end of the actual string</exception>
        public static void StartsWith(string expected, string actual, string message = "")
        {
            if (actual.IndexOf(expected) == 0)
            {
                return;
            }

            throw new Exception($"{actual} does not starts with {expected}. {message}");
        }

        #endregion

        #region collection

        /// <summary>
        /// Check if a collection is empty
        /// </summary>
        /// <param name="collection">The collection to check</param>
        /// <exception cref="Exception">Raises an exception if the collection is not empty</exception>
        public static void Empty(ICollection collection, string message = "")
        {
            if (collection.Count == 0)
            {
                return;
            }

            throw new Exception($"{collection} is not empty. {message}");
        }

        /// <summary>
        /// Check if a collection is empty
        /// </summary>
        /// <param name="collection">The collection to check</param>
        /// <exception cref="Exception">Raises an exception if the collection is not empty</exception>
        public static void NotEmpty(ICollection collection, string message = "")
        {
            if (collection.Count > 0)
            {
                return;
            }

            throw new Exception($"{collection} is not empty. {message}");
        }

        #endregion region

        #region types, objects

        /// <summary>
        /// Check if an object is equal to a type
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <param name="obj">The object to check</param>
        /// <exception cref="Exception">Raises an exception if the types are not equal</exception>
        public static void IsType(Type type, object obj, string message = "")
        {
            if (type == obj.GetType())
            {
                return;
            }

            throw new Exception($"{obj} is not type of {type}. {message}");
        }

        /// <summary>
        /// Check if an object is not equal to a type
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <param name="obj">The object to check</param>
        /// <exception cref="Exception">Raises an exception if the types are equal</exception>
        public static void IsNotType(Type type, object obj, string message = "")
        {
            if (type != obj.GetType())
            {
                return;
            }

            throw new Exception($"{obj} is not type of {type}. {message}");
        }

        /// <summary>
        /// Check if an object is the same as another
        /// </summary>
        /// <param name="a">The first object</param>
        /// <param name="b">The second object</param>
        /// <exception cref="Exception">Raises an exception if the objects are not the same</exception>
        public static void Same(object a, object b, string message = "")
        {
            if (a.Equals(b))
            {
                return;
            }

            throw new Exception($"{a} is not the same as {b}. {message}");
        }

        /// <summary>
        /// Check if an object is not the same as another
        /// </summary>
        /// <param name="a">The first object</param>
        /// <param name="b">The second object</param>
        /// <exception cref="Exception">Raises an exception if the objects are the same</exception>
        public static void NotSame(object a, object b, string message = "")
        {
            if (!a.Equals(b))
            {
                return;
            }

            throw new Exception($"{a} is the same as {b}. {message}");
        }

        /// <summary>
        /// Check if an object is null
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <exception cref="Exception">Raises an exception if the object is not null</exception>
        public static void Null(object obj, string message = "")
        {
            if (obj == null)
            {
                return;
            }

            throw new Exception($"{obj} is null. {message}");
        }

        /// <summary>
        /// Check if an object is not null
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <exception cref="Exception">Raises an exception if the object is null/exception>
        public static void NotNull(object obj, string message = "")
        {
            if (obj != null)
            {
                return;
            }

            throw new Exception($"{obj} is not null. {message}");
        }

        /// <summary>
        /// Check if an exception is raised and matching a type
        /// </summary>
        /// <param name="exceptionType">The exception to be raised</param>
        /// <param name="action">The method to execute</param>
        public static void Trows(Type exceptionType, Action action, string message = "")
        {
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

                throw new Exception($"An exception {ex.GetType()} has been thrown but is not type {exceptionType}. {message}");
            }

            throw new Exception($"No exception has been thrown. {message}");
        }

        #endregion
    }
}
