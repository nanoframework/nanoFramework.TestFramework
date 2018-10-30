//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nanoFramework.TestPlatform.TestInterface
{
    public class UnitTestDiscoverer
    {
        private nFTestSettings _settings;

        public UnitTestDiscoverer(nFTestSettings settings)
        {
            _settings = settings ?? new nFTestSettings();

        }

        public List<TestCase> DiscoverTests(IEnumerable<string> sources)
        {
            var tests = new List<TestCase>();

            // Retrieve test cases for each provided source
            foreach (var source in sources)
            {
                //LogVerbose($"Source: {source}{Environment.NewLine}");
                //if (!File.Exists(source))
                //{
                //    LogVerbose($"  File not found.{Environment.NewLine}");
                //}
                //else if (CheckSource(source))
                //{
                //    var foundtests = ExtractTestCases(source);
                //    LogVerbose($"  Testcase count: {foundtests.Count}{Environment.NewLine}");
                //    tests.AddRange(foundtests);
                //}
                //else
                //{
                //    LogVerbose($"  Invalid source.{Environment.NewLine}");
                //}
                //LogDebug($"  Accumulated Testcase count: {tests.Count}{Environment.NewLine}");

                tests.Add(new TestCase() { Filename = "filename", Line = 10, Source = "source", Name = "test name" });
            }

            return tests;
        }
    }
}
