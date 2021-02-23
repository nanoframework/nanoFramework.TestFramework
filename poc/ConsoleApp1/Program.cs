using System;
using System.Reflection;
using System.Diagnostics;
using nanoFramework.TestFramework;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var source = @"C:\Repos\nanoFramework\UnitTests\TestOfTestFramework\bin\Debug\Test.dll";
            //var source = @"C:\Repos\nanoFramework\UnitTests\TestAdapter\bin\Debug\net4.6\nanoFramework.TestAdapter.dll";
            var nfprojSources = FindNfprojSources(source);
            if (nfprojSources.Length == 0)
            {
                return;
            }

            var allCsFils = GetAllCsFileNames(nfprojSources);

            Assembly test = Assembly.LoadFile(source);
            AppDomain.CurrentDomain.AssemblyResolve += App_AssemblyResolve;
            AppDomain.CurrentDomain.Load(test.GetName());

            List<TestCase> testCases = new List<TestCase>();

            Type[] allTypes = test.GetTypes();
            foreach (var type in allTypes)
            {
                if (type.IsClass)
                {
                    var typeAttribs = type.GetCustomAttributes(true);
                    foreach (var typeAttrib in typeAttribs)
                    {
                        if (typeof(TestClassAttribute).FullName == typeAttrib.GetType().FullName)
                        {
                            var methods = type.GetMethods();
                            // First we look at Setup
                            foreach (var method in methods)
                            {
                                var attribs = method.GetCustomAttributes(true);

                                foreach (var attrib in attribs)
                                {
                                    if (attrib.GetType().FullName == typeof(SetupAttribute).FullName ||
                                    attrib.GetType().FullName == typeof(TestMethodAttribute).FullName ||
                                    attrib.GetType().FullName == typeof(CleanupAttribute).FullName)
                                    {
                                        TestCase testCase = new TestCase();
                                        var flret = GetFileNameAndLineNumber(allCsFils, type, method);
                                        testCase.CodeFilePath = flret.FileName;
                                        testCase.DisplayName = flret.MethodName;
                                        testCase.LineNumber = flret.LineNumber;
                                        testCase.Source = source;
                                        testCase.ExecutorUri = new Uri("executor://nanoFrameworkTestExecutor");
                                        testCases.Add(testCase);
                                    }
                                }
                            }

                        }
                    }
                }
            }
            Console.WriteLine("next");
        }

        private static void App_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Console.WriteLine();
        }

        private static Assembly App_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Split(new[] { ',' })[0] + ".dll";
            string path = Path.GetDirectoryName(args.RequestingAssembly.Location);
            return Assembly.LoadFrom(Path.Combine(path, dllName));
        }

        // Loads the content of a file to a byte array.
        static byte[] LoadRawFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            return buffer;
        }


        static string[] GetAllCsFileNames(FileInfo[] nfprojSources)
        {
            List<string> allCsFiles = new List<string>();
            foreach (var nfproj in nfprojSources)
            {
                var csFiles = Directory.GetFiles(Path.GetDirectoryName(nfproj.FullName), "*.cs", SearchOption.AllDirectories);
                // Get rid of those in /bin / obj
                var csFilesClean = csFiles.Where(m => !(m.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") || m.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")));
                allCsFiles.AddRange(csFilesClean);
            }

            return allCsFiles.ToArray();
        }

        static FileInfo[] FindNfprojSources(string source)
        {
            if (Path.GetDirectoryName(source) == null)
            {
                return new FileInfo[0];
            }

            // Find all the potential *.cs files present at same level or above a nfproj file,
            // if no nfproj file, then we will skip this source
            var mainDirectory = new DirectoryInfo(Path.GetDirectoryName(source));
            var nfproj = mainDirectory.GetFiles("*.nfproj");
            if (nfproj.Length == 0)
            {
                var ret = FindNfprojSources(mainDirectory.Parent.FullName);
                return ret;
            }

            return nfproj;
        }

        static FileInfoLine GetFileNameAndLineNumber(string[] csFiles, Type className, MethodInfo method)
        {
            var clName = className.Name;
            var methodName = method.Name;
            FileInfoLine flret = new FileInfoLine();
            foreach (var csFile in csFiles)
            {
                StreamReader sr = new StreamReader(csFile);
                var allFile = sr.ReadToEnd();
                if (allFile.Contains($"class {clName}"))
                {
                    if (allFile.Contains($" {methodName}("))
                    {
                        // We found it!
                        int lineNum = 1;
                        foreach (var line in allFile.Split('\r'))
                        {
                            if (line.Contains($" {methodName}("))
                            {
                                flret.FileName = csFile;
                                flret.LineNumber = lineNum;
                                flret.MethodName = method.Name;
                                return flret;
                            }

                            lineNum++;
                        }
                    }
                }
            }

            return flret;
        }
    }
}

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// The test method attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestMethodAttribute : Attribute
    {
    }
}


namespace nanoFramework.TestFramework
{
    /// <summary>
    /// The test class attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestClassAttribute : Attribute
    {
        /// <summary>
        /// Gets a test method attribute that enables running this test.
        /// </summary>
        /// <param name="testMethodAttribute">The test method attribute instance defined on this method.</param>
        /// <returns>The <see cref="TestMethodAttribute"/> to be used to run this test.</returns>
        /// <remarks>Extensions can override this method to customize how all methods in a class are run.</remarks>
        public virtual TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
        {
            // If TestMethod is not extended by derived class then return back the original TestMethodAttribute
            return testMethodAttribute;
        }
    }
}

namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Setup attribute, will always be launched first by the launcher, typically used to setup hardware or classes that has to be used in all the tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SetupAttribute : Attribute
    {
    }
}


namespace nanoFramework.TestFramework
{
    /// <summary>
    /// Clean up attribute typically used to clean up after the tests, it will always been called the last after all the Test Method run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CleanupAttribute : Attribute
    {
    }
}
