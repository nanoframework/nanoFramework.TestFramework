// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Reflection;

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// This class is a unit test launcher for nanoFramework
    /// </summary>
    public class UnitTestLauncher
    {
        /// <summary>
        /// Main function
        /// </summary>
        public static void Main()
        {
            Assembly test = Assembly.Load("NFUnitTest");

            Type[] allTypes = test.GetTypes();

            foreach (var type in allTypes)
            {
                if (!Helper.IsTestClassCandidate(type))
                {
                    continue;
                }

                var typeAttribs = type.GetCustomAttributes(true);

                foreach (var typeAttrib in typeAttribs)
                {
                    if (typeof(TestClassAttribute) != typeAttrib.GetType())
                    {
                        continue;
                    }

                    var methods = type.GetMethods();

                    // one instance per test class, created when the first instance method has to run
                    object instance = null;

                    // First we look at Setup
                    var continueTests = RunTest(type, methods, typeof(SetupAttribute), ref instance);

                    if (continueTests)
                    {
                        // then we run the tests
                        RunTest(type, methods, typeof(TestMethodAttribute), ref instance);
                        RunTest(type, methods, typeof(DataRowAttribute), ref instance);

                        // last we handle Cleanup
                        RunTest(type, methods, typeof(CleanupAttribute), ref instance);
                    }
                }
            }
        }

        private static bool RunTest(
            Type type,
            MethodInfo[] methods,
            Type attribToRun,
            ref object instance)
        {
            long dt;
            long totalTicks;
            bool isSetupMethod = attribToRun == typeof(SetupAttribute);

            foreach (var method in methods)
            {
                var attribs = method.GetCustomAttributes(true);
                attribs = Helper.RemoveTestMethodIfDataRowExists(attribs);

                for (int i = 0; i < attribs.Length; i++)
                {
                    var attrib = attribs[i];
                    var methodName = $"{method.DeclaringType.FullName}.{method.Name}.{i}";

                    if (attribToRun != attrib.GetType())
                    {
                        continue;
                    }

                    try
                    {
                        dt = DateTime.UtcNow.Ticks;
                        object[] parameters = GetParameters(attrib);

                        object target = null;

                        if (!method.IsStatic)
                        {
                            if (instance == null)
                            {
                                instance = CreateInstance(type);
                            }

                            target = instance;
                        }

                        method.Invoke(target, parameters);
                        totalTicks = DateTime.UtcNow.Ticks - dt;

                        // Keep result markers at the beginning of a line even if a test used Write() without trailing newline.
                        // On change this pattern it has to be updated at Executor.CheckAllTests.
                        Console.WriteLine($"\r\nTest passed,{methodName},{totalTicks}");
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(SkipTestException))
                        {
                            // Keep result markers at the beginning of a line even if a test used Write() without trailing newline.
                            // On change this pattern it has to be updated at Executor.CheckAllTests.
                            Console.WriteLine($"\r\nTest skipped,{methodName},{ex.Message}");

                            if (isSetupMethod)
                            {
                                // In case the Setup attribute test is skipped, we will skip
                                // All the other tests
                                return false;
                            }
                        }
                        else
                        {
                            // Keep result markers at the beginning of a line even if a test used Write() without trailing newline.
                            // On change this pattern it has to be updated at Executor.CheckAllTests.
                            Console.WriteLine($"\r\nTest failed,{methodName},{ex.Message}");
                        }
                    }
                }
            }

            return true;
        }

        private static object CreateInstance(Type type)
        {
            ConstructorInfo constructor = type.GetConstructor(new Type[0]);

            if (constructor == null)
            {
                throw new InvalidOperationException($"{type.FullName} has no parameterless constructor");
            }

            return constructor.Invoke(null);
        }

        private static object[] GetParameters(object attribute)
        {
            if (attribute.GetType() != typeof(DataRowAttribute))
            {
                return null;
            }

            var testCaseAttribute = (DataRowAttribute)attribute;
            return testCaseAttribute.MethodParameters;
        }
    }
}
