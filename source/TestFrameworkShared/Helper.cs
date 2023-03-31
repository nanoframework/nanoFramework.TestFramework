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
