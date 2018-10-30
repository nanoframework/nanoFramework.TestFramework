//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.PlatformServices
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.Interface;
    using System;
    using System.IO;
    using System.Reflection;


    /// <summary>
    /// This service is responsible for any file based operations.
    /// </summary>
    public class FileOperations : IFileOperations
    {
        /// <summary>
        /// Loads an assembly into the current context.
        /// </summary>
        /// <param name="assemblyFileName">The name of the assembly.</param>
        /// <param name="isReflectionOnly">
        /// Indicates whether this should be a reflection only load.
        /// </param>
        /// <returns>A handle to the loaded assembly.</returns>
        public Assembly LoadAssembly(string assemblyFileName, bool isReflectionOnly)
        {
            //if (isReflectionOnly)
            //{
            //    return Assembly.ReflectionOnlyLoadFrom(assemblyFileName);
            //}
            //else
            //{
            //    return Assembly.LoadFrom(assemblyFileName);
            //}
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assemblyFileName);
            return Assembly.Load(new AssemblyName(fileNameWithoutExtension));

        }

        /// <summary>
        /// Gets the path to the .DLL of the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Path to the .DLL of the assembly.</returns>
        public string GetAssemblyPath(Assembly assembly)
        {
            //return assembly.Location;
            return null; // TODO: what are the options here?
        }

        /// <summary>
        /// Verify if a file exists in the current context.
        /// </summary>
        /// <param name="assemblyFileName"> The assembly file name. </param>
        /// <returns> true if the file exists. </returns>
        public bool DoesFileExist(string assemblyFileName)
        {
            //return (SafeInvoke<bool>(() => File.Exists(assemblyFileName)) as bool?) ?? false;
            var fileExists = true;// false;

            //try
            //{
            //    var fileNameWithoutPath = Path.GetFileName(assemblyFileName);
            //    var searchTask = Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(fileNameWithoutPath).AsTask();
            //    searchTask.Wait();
            //    fileExists = searchTask.Result != null;
            //}
            //catch (Exception)
            //{
            //    // ignore
            //}

            return fileExists;

        }

        /// <summary>
        /// Creates a Navigation session for the source file.
        /// This is used to get file path and line number information for its components.
        /// </summary>
        /// <param name="source"> The source file. </param>
        /// <returns> A Navigation session instance for the current platform. </returns>
        public object CreateNavigationSession(string source)
        {
            //var messageFormatOnException =
            //    string.Join("MSTestDiscoverer:DiaSession: Could not create diaSession for source:", source, ". Reason:{0}");
            //return SafeInvoke<DiaSession>(() => new DiaSession(source), messageFormatOnException) as DiaSession;
            //return DiaSessionOperations.CreateNavigationSession(source);

            return new DiaSession(source);
        }

        /// <summary>
        /// Get's the navigation data for a navigation session.
        /// </summary>
        /// <param name="navigationSession"> The navigation session. </param>
        /// <param name="className"> The class name. </param>
        /// <param name="methodName"> The method name. </param>
        /// <param name="minLineNumber"> The min line number. </param>
        /// <param name="fileName"> The file name. </param>
        public void GetNavigationData(object navigationSession, string className, string methodName, out int minLineNumber, out string fileName)
        {
            //fileName = null;
            //minLineNumber = -1;

            //var diasession = navigationSession as DiaSession;
            //var navigationData = diasession?.GetNavigationData(className, methodName);

            //if (navigationData != null)
            //{
            //    minLineNumber = navigationData.MinLineNumber;
            //    fileName = navigationData.FileName;
            //}
            //DiaSessionOperations.GetNavigationData(navigationSession, className, methodName, out minLineNumber, out fileName);

            DiaSession diaSession = navigationSession as DiaSession;
            var navData = diaSession.GetNavigationData(className, methodName);

            minLineNumber = navData.MinLineNumber;
            fileName = navData.FileName;
        }

        /// <summary>
        /// Dispose's the navigation session instance.
        /// </summary>
        /// <param name="navigationSession"> The navigation session. </param>
        public void DisposeNavigationSession(object navigationSession)
        {
            //var diasession = navigationSession as DiaSession;
            //diasession?.Dispose();
            // DiaSessionOperations.DisposeNavigationSession(navigationSession);


            DiaSession diaSession = navigationSession as DiaSession;
            diaSession.Dispose();
        }

        /// <summary>
        /// Gets the full file path of an assembly file.
        /// </summary>
        /// <param name="assemblyFileName"> The file name. </param>
        /// <returns> The full file path. </returns>
        public string GetFullFilePath(string assemblyFileName)
        {
            //return (SafeInvoke<string>(() => Path.GetFullPath(assemblyFileName)) as string) ?? assemblyFileName;
            return assemblyFileName;

        }

        //private static object SafeInvoke<T>(Func<T> action, string messageFormatOnException = null)
        //{
        //    try
        //    {
        //        return action.Invoke();
        //    }
        //    catch (Exception exception)
        //    {
        //        if (string.IsNullOrEmpty(messageFormatOnException))
        //        {
        //            messageFormatOnException = "{0}";
        //        }

        //        EqtTrace.ErrorIf(EqtTrace.IsErrorEnabled, messageFormatOnException, exception.Message);
        //    }

        //    return null;
        //}
    }
}
