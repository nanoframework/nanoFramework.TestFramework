//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The test initialize attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestInitializeAttribute : Attribute
    {
    }

    /// <summary>
    /// The test cleanup attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestCleanupAttribute : Attribute
    {
    }

    /// <summary>
    /// The class initialize attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ClassInitializeAttribute : Attribute
    {
    }

    /// <summary>
    /// The class cleanup attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ClassCleanupAttribute : Attribute
    {
    }

    /// <summary>
    /// The assembly initialize attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AssemblyInitializeAttribute : Attribute
    {
    }

    /// <summary>
    /// The assembly cleanup attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AssemblyCleanupAttribute : Attribute
    {
    }


    /// <summary>
    /// Timeout attribute; used to specify the timeout of a unit test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TimeoutAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutAttribute"/> class.
        /// </summary>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        public TimeoutAttribute(int timeout)
        {
            this.Timeout = timeout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutAttribute"/> class with a preset timeout
        /// </summary>
        /// <param name="timeout">
        /// The timeout
        /// </param>
        public TimeoutAttribute(TestTimeout timeout)
        {
            this.Timeout = (int)timeout;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the timeout.
        /// </summary>
        public int Timeout { get; }

        #endregion
    }

    /// <summary>
    /// Enumeration for timeouts, that can be used with the <see cref="TimeoutAttribute"/> class.
    /// The type of the enumeration must match
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Compat reasons")]
    public enum TestTimeout
    {
        /// <summary>
        /// The infinite.
        /// </summary>
        Infinite = int.MaxValue
    }
}
