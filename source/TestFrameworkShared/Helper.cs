// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Helper class for keeping test name same in TestAdapter and TestRunner
    /// </summary>
    public static class Helper
    {
        private delegate bool AnyDelegateType(object source);

        private static bool Any(this object[] array, AnyDelegateType predicate)
        {
            foreach (var item in array)
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The IEnumerable to check for emptiness.</param>
        /// <returns>true if the source sequence contains any elements; otherwise, false.</returns>
        public static bool Any<T>(this IEnumerable<T> source)
        {
            IEnumerator<T> enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }

        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The IEnumerable that contains the elements to be counted.</param>
        /// <returns>The number of elements in the input sequence.</returns>
        public static int Count<T>(this IEnumerable<T> source)
        {
            int count = 0;
            IEnumerator<T> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                count++;
            }

            return count;
        }

        /// <summary>
        /// Removes "TestMethod" attribute from array if "DataRow" attribute exists in the same array
        /// </summary>
        /// <param name="attribs">Array of attributes to check</param>
        /// <returns>New array without TestMethod if DataRow exists, if not the same array</returns>
        public static object[] RemoveTestMethodIfDataRowExists(object[] attribs)
        {
            //If method attribute contains TestMethod and DataRow - add only DataRow
            if (attribs.Any(x => x.GetType().FullName == typeof(TestMethodAttribute).FullName) &&
                attribs.Any(x => x.GetType().FullName == typeof(DataRowAttribute).FullName))
            {
                var newAttribs = new object[attribs.Length - 1];

                var newAttribsIndex = 0;
                for (int i = 0; i < attribs.Length; i++)
                {
                    var attrib = attribs[i];
                    if (attrib.GetType().FullName == typeof(TestMethodAttribute).FullName)
                    {
                        continue;
                    }

                    newAttribs[newAttribsIndex] = attrib;
                    newAttribsIndex++;
                }

                return newAttribs;
            }

            return attribs;
        }
    }
}
