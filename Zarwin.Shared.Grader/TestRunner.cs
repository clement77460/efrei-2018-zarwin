using System;
using System.Collections.Generic;

namespace Zarwin.Shared.Grader
{
    public class TestRunner
    {
        private readonly string _solutionDirectory;
        private readonly bool _noBuild;

        public TestRunner(string solutionDirectory, bool noBuild)
        {
            _solutionDirectory = solutionDirectory;
            _noBuild = noBuild;
        }

        public TestResult RunTests(TestParameter parameter)
        {
            var results = new TestRunOperation(_solutionDirectory, _noBuild, parameter).RunTests();
            return InterpretResults(results)
                ?? TestResult.Inconclusive;
        }

        private TestResult InterpretResults( List<TestResult> results)
        {
            if (results.Count == 1)
            {
                return results[0];
            }
            else if (results.Count == 0)
            {
                Console.Error.WriteLine("No test were implemented. Assuming 0.");
                return null;
            }
            else
            {
                Console.Error.WriteLine("Tests were implemented multiple times. Assuming 0. Test results :");
                foreach (var result in results)
                    Console.Error.WriteLine(result.FileName);
                return null;
            }
        }
    }
}
