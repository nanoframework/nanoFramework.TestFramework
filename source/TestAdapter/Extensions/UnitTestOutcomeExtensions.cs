//
// Copyright (c) 2018 The nanoFramework project contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TestPlatform.MSTest.TestAdapter.ObjectModel;

namespace nanoFramework.TestPlatform.MSTest.TestAdapter.Extensions
{
    public static class UnitTestOutcomeExtensions
    {
        /// <summary>
        /// Converts the test framework's UnitTestOutcome object to adapter's UnitTestOutcome object.
        /// </summary>
        /// <param name="frameworkTestOutcome">The test framework's UnitTestOutcome object.</param>
        /// <returns>The adapter's UnitTestOutcome object.</returns>
        public static UnitTestOutcome ToUnitTestOutcome(this UnitTestOutcome frameworkTestOutcome)
        {
            UnitTestOutcome outcome = UnitTestOutcome.Passed;

            switch (frameworkTestOutcome)
            {
                case UnitTestOutcome.Failed:
                    outcome = UnitTestOutcome.Failed;
                    break;

                case UnitTestOutcome.Inconclusive:
                    outcome = UnitTestOutcome.Inconclusive;
                    break;

                case UnitTestOutcome.InProgress:
                    outcome = UnitTestOutcome.InProgress;
                    break;

                case UnitTestOutcome.Passed:
                    outcome = UnitTestOutcome.Passed;
                    break;

                case UnitTestOutcome.Timeout:
                    outcome = UnitTestOutcome.Timeout;
                    break;

                case UnitTestOutcome.NotRunnable:
                    outcome = UnitTestOutcome.NotRunnable;
                    break;

                case UnitTestOutcome.Unknown:
                default:
                    outcome = UnitTestOutcome.Error;
                    break;
            }

            return outcome;
        }

        /// <summary>
        /// Returns more important outcome of two.
        /// </summary>
        /// <param name="outcome1"> First outcome that needs to be compared. </param>
        /// <param name="outcome2"> Second outcome that needs to be compared. </param>
        /// <returns> Outcome which has higher importance.</returns>
        internal static UnitTestOutcome GetMoreImportantOutcome(
            this UnitTestOutcome outcome1, 
            UnitTestOutcome outcome2)
        {
            var unitTestOutcome1 = outcome1.ToUnitTestOutcome();
            var unitTestOutcome2 = outcome2.ToUnitTestOutcome();
            return unitTestOutcome1 < unitTestOutcome2 ? outcome1 : outcome2;
        }
    }
}
