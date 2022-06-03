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

        /// <summary>
        /// Generates test display name based on passed <paramref name="method"/> and <paramref name="attribute"/>. 
        /// </summary>
        /// <returns>Returns method name with parameters if passed attribute is of DataRow type</returns>
        public static string GetTestDisplayName(MethodInfo method, object attribute)
        {
            // Comparing via full name, because attribute parameter is from "TestFramework.dll"
            // and current type TestCaseAttribute is in scope of "TestAdapter" due to shared project
            // The same reason - reflection to get value
            if (attribute.GetType().FullName == typeof(DataRowAttribute).FullName)
            {
                var methodParameters = (object[])attribute.GetType()
                    .GetMethod($"get_{nameof(DataRowAttribute.MethodParameters)}").Invoke(attribute, null);

                return $"{method.Name} - (params: {GetJoinedParams(methodParameters)})";
            }

            return method.Name;
        }
    }
}
