using System;

namespace nanoFramework.TestFramework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DataRowAttribute : Attribute
    {
        public DataRowAttribute(object[] args)
        {
            Arguments = args;
        }

        public object[] Arguments { get; }
    }
}
