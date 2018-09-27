using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;
using static Zarwin.Shared.Tests.IntegratedTests;

namespace Zarwin.Shared.Tests.Infrastructure
{
    public class ScenarioTheoryDiscoverer : TheoryDiscoverer
    {
        public ScenarioTheoryDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }

        protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute, object[] dataRow)
        {
            var cases = base.CreateTestCasesForDataRow(discoveryOptions, testMethod, theoryAttribute, dataRow);

            foreach (var testCase in cases)
            {
                ApplyScenarioInfo(testCase, dataRow);
            }
            return cases;
        }

        private void ApplyScenarioInfo(IXunitTestCase testCase, object[] dataRow)
        {
            if (dataRow.Length < 1)
                return;

            var scenario = dataRow[0] as Scenario;
            if (scenario == null)
                return;

            testCase.Traits.Add("grading", new List<string>() { scenario.Version });
        }
    }
}
