// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Helper class for keeping test name same in TestAdapter and TestRunner
    /// </summary>
    public static class Helper
    {
        private delegate bool AnyDelegateType(object source);

        /// <summary>
        /// Checks whether a type can be considered for test discovery and execution.
        /// </summary>
        /// <param name="type">Type to inspect.</param>
        /// <returns><see langword="true"/> when the type is a class and not an attribute type.</returns>
        public static bool IsTestClassCandidate(Type type)
        {
            return type.IsClass && !IsAttributeType(type);
        }

        private static bool IsAttributeType(Type type)
        {
            var attributeFullName = typeof(Attribute).FullName;

            Type current = type;
            while (current != null)
            {
                if (current.FullName == attributeFullName)
                {
                    return true;
                }
                try
                {
                    current = current.BaseType;
                }
                catch
                {
                    // Base type assembly not resolvable in this reflection context;
                    // conservatively treat as attribute to avoid calling GetCustomAttributes on it.
                    return true;
                }
            }

            return false;

        }

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
