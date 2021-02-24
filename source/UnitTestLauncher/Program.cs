//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Reflection;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// This class os a unit test launcher for nanoFramework
    /// </summary>
    public class UnitTestLauncher
    {
        /// <summary>
        /// Main function
        /// </summary>
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFrmaework UnitTestLauncher!");

            Assembly test = Assembly.Load("Test");
            Type[] allTypes = test.GetTypes();
            foreach (var type in allTypes)
            {
                if (type.IsClass)
                {
                    var typeAttribs = type.GetCustomAttributes(true);
                    foreach (var typeAttrib in typeAttribs)
                    {
                        if (typeof(TestClassAttribute) == typeAttrib.GetType())
                        {
                            var methods = type.GetMethods();
                            // First we look at Setup
                            RunTest(methods, typeof(SetupAttribute));
                            RunTest(methods, typeof(TestMethodAttribute));
                            RunTest(methods, typeof(CleanupAttribute));
                        }
                    }
                }
            }
        }

        private static void RunTest(MethodInfo[] methods, Type attribToRun)
        {
            long dt;
            long totalTicks;
            foreach (var method in methods)
            {
                var attribs = method.GetCustomAttributes(true);
                foreach (var attrib in attribs)
                {
                    if (attribToRun == attrib.GetType())
                    {
                        try
                        {
                            dt = DateTime.UtcNow.Ticks;
                            method.Invoke(null, null);
                            totalTicks = DateTime.UtcNow.Ticks - dt;
                            Debug.WriteLine($"Test passed: {method.Name}, {totalTicks}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Test failed: {method.Name}, {ex.Message}");
                        }

                    }
                }
            }
        }
    }
}
