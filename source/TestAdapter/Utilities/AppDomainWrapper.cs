//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.Utilities
{
    using System;
    using System.Security.Policy;

    /// <summary>
    /// Abstraction over the AppDomain APIs.
    /// </summary>
    internal class AppDomainWrapper : IAppDomain
    {
        public AppDomain CreateDomain(string friendlyName, Evidence securityInfo, AppDomainSetup info)
        {
            return AppDomain.CreateDomain(friendlyName, securityInfo, info);
        }

        public void Unload(AppDomain appDomain)
        {
            AppDomain.Unload(appDomain);
        }
    }
}
