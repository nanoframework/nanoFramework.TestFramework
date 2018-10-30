//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter
{
    /// <summary>
    /// Parallel execution mode.
    /// </summary>
    public enum ExecutionScope
    {
        /// <summary>
        /// Each thread of execution will be handed a TestClass worth of tests to execute.
        /// Within the TestClass, the test methods will execute serially.
        /// </summary>
        ClassLevel = 0,

        /// <summary>
        /// Each thread of execution will be handed TestMethods to execute.
        /// </summary>
        MethodLevel = 1,
    }
}