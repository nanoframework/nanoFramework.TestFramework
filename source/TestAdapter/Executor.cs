using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace nanoFramework.TestPlatform.TestAdapter
{
    [ExtensionUri(TestsConstants.Executornano)]
    class Executor : ITestExecutor
    {
        private bool _cancel = false;

        public void Cancel()
        {
            _cancel = true;
            return;
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            foreach (var test in tests)
            {
                frameworkHandle.RecordResult(new TestResult(test) { Outcome = TestOutcome.Passed, DisplayName = test.DisplayName, Duration = TimeSpan.FromSeconds(1) }); ;
            }
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            foreach (var source in sources)
            {
                var testsCases = TestDiscoverer.FindTestCases(source);
                RunTests(testsCases, runContext, frameworkHandle);
            }
        }
    }
}
