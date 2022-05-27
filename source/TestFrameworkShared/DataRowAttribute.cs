using System;

namespace nanoFramework.TestFramework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DataRowAttribute : Attribute
    {
        public object[] MethodParameters { get; }

        public DataRowAttribute(params object[] methodParameters)
        {
            if (methodParameters == null)
            {
                throw new ArgumentNullException($"{nameof(methodParameters)} can not be null");
            }

            if (methodParameters.Length == 0)
            {
                throw new ArgumentException($"{nameof(methodParameters)} can not be empty");
            }

            MethodParameters = methodParameters;
        }
    }
}