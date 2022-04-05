using System;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Data test method attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DataTestMethodAttribute : Attribute
    {
    }
}
