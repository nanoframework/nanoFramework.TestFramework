using System.Reflection;

namespace nanoFramework.TestFramework
{
    public static class Helper
    {
        private static string GetJoinedParams(object[] data)
        {
            var returnString = string.Empty;
            foreach (var item in data)
            {
                returnString += $"{item} | ";
            }

            return returnString.Substring(0, returnString.Length - 4);
        }

        public static string GetDisplayName(MethodInfo method, object attribute)
        {
            /*Comparing via full name, because attribute parameter is from "TestFramework.dll"
            and current type TestCaseAttribute is in scope of "TestAdapter" due to shared project
            The same reason - reflection to get value
            */
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