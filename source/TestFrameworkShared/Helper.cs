//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System.Reflection;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Helper class for keeping test name same in TestAdapter and TestRunner
    /// </summary>
    public static class Helper
    {
        private delegate bool AnyDelegateType(object source);

        private static string GetJoinedParams(object[] data)
        {
            var returnString = string.Empty;
            foreach (var item in data)
            {
                returnString += $"{item} | ";
            }

            // In each loop iteration we are appending " | " event at the end
            // To keep return string clean, we are removing last 3 charcters 
            // Lenght starts from 1, substring from 0
            // To remove 3 last characters using this method, we need to add 1
            return returnString.Substring(0, returnString.Length - 4);
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
        /// Generates test display name based on passed <paramref name="method"/>, <paramref name="attribute"/> and <paramref name="attributeIndex"/>. 
        /// </summary>
        /// <returns>Returns method name with attributeIndex if passed attribute is of DataRow type</returns>
        public static string GetTestDisplayName(MethodInfo method, object attribute, int attributeIndex)
        {
            // Comparing via full name, because attribute parameter is from "TestFramework.dll"
            // and current type TestCaseAttribute is in scope of "TestAdapter" due to shared project
            // The same reason - reflection to get value
            if (attribute.GetType().FullName == typeof(DataRowAttribute).FullName)
            {
                return $"{method.Name} (index {attributeIndex})";
            }

            return method.Name;
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
