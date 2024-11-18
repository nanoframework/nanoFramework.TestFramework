// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

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
