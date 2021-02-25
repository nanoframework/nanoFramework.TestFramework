using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using nanoFramework.TestPlatform.TestAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nanoFramework.TestAdapter
{
    internal class LogMessenger
    {
        private IFrameworkHandle _frameworkHandle = null;
        private IMessageLogger _logger = null;
        private Settings _settings = null;

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

        public void LogPanicMessage(
            string message)
        {
            LogMessage(
                message,
                Settings.LoggingLevel.Error,
                true);
        }

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
