//
// Copyright (c) 2018 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace nanoFramework.TestPlatform.TestAdapter
{
    public static class LogHelperExtensions
    {
        #region IMessageLogger extensions

        public static void InformationalMessage(this IMessageLogger logger, string message)
        {
            logger.SendMessage(TestMessageLevel.Informational, message);
        }

        public static void ErrorMessage(this IMessageLogger logger, string message)
        {
            logger.SendMessage(TestMessageLevel.Error, message);
        }

        #endregion


        #region IFrameworkHandle extensions

        public static void InformationalMessage(this IFrameworkHandle frameworkHandle, string message)
        {
            frameworkHandle.SendMessage(TestMessageLevel.Informational, message);
        }
        public static void ErrorMessage(this IFrameworkHandle frameworkHandle, string message)
        {
            frameworkHandle.SendMessage(TestMessageLevel.Error, message);
        }

        #endregion
    }
}
