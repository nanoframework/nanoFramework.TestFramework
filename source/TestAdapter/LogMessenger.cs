//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using nanoFramework.TestPlatform.TestAdapter;

namespace nanoFramework.TestAdapter
{
    internal class LogMessenger
    {
        private IFrameworkHandle _frameworkHandle = null;
        private IMessageLogger _logger = null;
        private Settings _settings = null;

        /// <summary>
        /// A log messenger to log during the discovery and executor process
        /// </summary>
        /// <param name="frameworkHandle">The framework handle</param>
        /// <param name="provider">The settings provider</param>
        public LogMessenger(
            IFrameworkHandle frameworkHandle,
            SettingsProvider provider)
        {
            _frameworkHandle = frameworkHandle;
            if (provider != null)
            {
                _settings = provider.Settings;
            }
        }
        /// <summary>
        /// A log messenger to log during the discovery and executor process
        /// </summary>
        /// <param name="logger">A platform logger</param>
        /// <param name="provider">The settings provider</param>

        public LogMessenger(
            IMessageLogger logger,
            SettingsProvider provider)
        {
            _logger = logger;

            if (provider != null)
            {
                _settings = provider.Settings;
            }
        }

        /// <summary>
        /// Log a panic message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogPanicMessage(
            string message)
        {
            LogMessage(
                message,
                Settings.LoggingLevel.Error,
                true);
        }

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="logLevel">The log level</param>
        /// <param name="panicMessage">Is it a panic message</param>
        public void LogMessage(
            string message,
            Settings.LoggingLevel logLevel,
            bool panicMessage = false)
        {
            if (logLevel >= _settings?.Logging)
            {
                _frameworkHandle?.SendMessage(
                    TestMessageLevel.Informational,
                    $"[nanoTestAdapter]: {message}");

                _logger?.SendMessage(
                    TestMessageLevel.Informational,
                    $"[nanoTestAdapter]: {message}");
            }
            else if (panicMessage)
            {
                _frameworkHandle?.SendMessage(
                    TestMessageLevel.Error,
                    $"[nanoTestAdapter] **PANIC**: {message}");
                _logger?.SendMessage(
                    TestMessageLevel.Error,
                    $"[nanoTestAdapter] **PANIC**: {message}");
            }
        }
    }
}
