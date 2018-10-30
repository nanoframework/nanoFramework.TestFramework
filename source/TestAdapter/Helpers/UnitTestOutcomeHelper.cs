//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.Helpers
{
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel;

    internal static class UnitTestOutcomeHelper
    {
        /// <summary>
        /// Converts the parameter unitTestOutcome to testOutcome
        /// </summary>
        /// <param name="unitTestOutcome"> The unit Test Outcome. </param>
        /// <param name="mapInconclusiveToFailed">Should map inconclusive to failed.</param>
        /// <returns>The Test platforms outcome.</returns>
        internal static TestOutcome ToTestOutcome(UnitTestOutcome unitTestOutcome, bool mapInconclusiveToFailed)
        {
            switch (unitTestOutcome)
            {
                case UnitTestOutcome.Passed:
                    return TestOutcome.Passed;

                case UnitTestOutcome.Failed:
                case UnitTestOutcome.Error:
                case UnitTestOutcome.Timeout:
                    return TestOutcome.Failed;

                case UnitTestOutcome.NotRunnable:
                    return TestOutcome.None;

                case UnitTestOutcome.Ignored:
                    return TestOutcome.Skipped;

                case UnitTestOutcome.Inconclusive:
                    {
                        if (mapInconclusiveToFailed)
                        {
                            return TestOutcome.Failed;
                        }

                        return TestOutcome.Skipped;
                    }

                case UnitTestOutcome.NotFound:
                    return TestOutcome.NotFound;

                default:
                    return TestOutcome.None;
            }
        }
    }
}
