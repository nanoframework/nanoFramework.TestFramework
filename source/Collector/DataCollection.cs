using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace nanoFramework.Collector
{
    [DataCollectorTypeUri(CollectorConstants.DataCollectorUri)]
    [DataCollectorFriendlyName(CollectorConstants.FriendlyName)]

    public class nanoCollector : DataCollector, ITestExecutionEnvironmentSpecifier
    {
        int i = 0;
        private DataCollectionSink _dataSink;
        private DataCollectionEnvironmentContext _context;
        private DataCollectionLogger _logger;
        private string _tempDirectoryPath = Path.GetTempPath();

        public override void Initialize(
            XmlElement configurationElement,
            DataCollectionEvents events,
            DataCollectionSink dataSink,
            DataCollectionLogger logger,
            DataCollectionEnvironmentContext environmentContext)
        {
            _dataSink = dataSink;
            _context = environmentContext;
            _logger = logger;
            events.TestHostLaunched += TestHostLaunched;
            events.SessionStart += SessionStarted;
            events.SessionEnd += SessionEnded;
            events.TestCaseStart += TestCaseStart;
            events.TestCaseEnd += TestCaseEnd;
        }

        private void TestCaseEnd(object sender, TestCaseEndEventArgs e)
        {
            _logger.LogWarning(_context.SessionDataCollectionContext, "[nanoCollector] TestCaseEnded " + e.TestCaseName);
        }

        private void TestCaseStart(object sender, TestCaseStartEventArgs e)
        {
            _logger.LogWarning(_context.SessionDataCollectionContext, "[nanoCollector] TestCaseStarted " + e.TestCaseName);
            _logger.LogWarning(_context.SessionDataCollectionContext, "[nanoCollector]TestCaseStarted " + e.TestElement.FullyQualifiedName);
            var filename = Path.Combine(_tempDirectoryPath, "testcasefilename" + i++ + ".txt");
            File.WriteAllText(filename, "nanoSuperTest");
            _dataSink.SendFileAsync(e.Context, filename, true);
        }

        private void SessionStarted(object sender, SessionStartEventArgs args)
        {
            var filename = Path.Combine(_tempDirectoryPath, "filename.txt");
            File.WriteAllText(filename, string.Empty);
            _dataSink.SendFileAsync(_context.SessionDataCollectionContext, filename, true);
            _logger.LogWarning(_context.SessionDataCollectionContext, "SessionStarted");
        }

        private void SessionEnded(object sender, SessionEndEventArgs args)
        {
            _logger.LogError(_context.SessionDataCollectionContext, new Exception("my exception"));
            _logger.LogWarning(_context.SessionDataCollectionContext, "my warning");
            _logger.LogException(_context.SessionDataCollectionContext, new Exception("abc"), DataCollectorMessageLevel.Error);

            _logger.LogWarning(_context.SessionDataCollectionContext, "[nanoCollector] SessionEnded");
        }

        private void TestHostLaunched(object sender, TestHostLaunchedEventArgs e)
        {
            _logger.LogWarning(_context.SessionDataCollectionContext, $"[nanoCollector] TestHostLaunched {e.TestHostProcessId}");
        }

        public IEnumerable<KeyValuePair<string, string>> GetTestExecutionEnvironmentVariables()
        {
            return new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("key", "value") };
        }

        protected override void Dispose(bool disposing)
        {
            _logger.LogWarning(_context.SessionDataCollectionContext, "Dispose called.");
        }
    }
}
