using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

namespace Zarwin.Shared.Grader
{
    public class TestRunOperation
    {
        private static readonly Regex _resultLine = new Regex("Results File: (?<fileName>.*\\.trx)$");

        private readonly string _solutionDirectory;
        private readonly bool _noBuild;
        private readonly TestParameter _parameter;

        private readonly List<TestResult> _results = new List<TestResult>();

        public TestRunOperation(string solutionDirectory, bool noBuild, TestParameter parameter)
        {
            _solutionDirectory = solutionDirectory;
            _noBuild = noBuild;
            _parameter = parameter;
        }

        public List<TestResult> RunTests()
        {
            ExecuteProcess();
            return _results;
        }

        private void ExecuteProcess()
        {
            var process = new TestProcess(_solutionDirectory, _noBuild, "--logger trx", _parameter.IncludeFilterArg);
            process.ForwardDataAndError = true;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.Run();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
                return;

            var match = _resultLine.Match(e.Data);

            if (match.Success)
            {
                ProcessTestResults(match.Groups["fileName"].Value);
            }
        }

        private readonly static string[] _expectedZeroCounters = new[]
        {
            "timeout",
            "aborted",
            "inconclusive",
            "passedButRunAborted",
            "notRunnable",
            "notExecuted",
            "disconnected",
            "warning",
            "completed",
            "inProgress",
            "pending",
        };

        private void ProcessTestResults(string resultFileName)
        {
            try
            {
                var result = ReadResult(resultFileName);
                if (result != null)
                {
                    _results.Add(result);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unexpected error when running integration tests : {e}");
            }
        }

        private TestResult ReadResult(string resultFileName)
        {
            var xmlResults = new XmlDocument();
            xmlResults.Load(resultFileName);
            var counterResult = xmlResults["TestRun"]["ResultSummary"]["Counters"];

            var total = ReadCounterValue(counterResult, "total");
            var passed = ReadCounterValue(counterResult, "passed");
            var failed = ReadCounterValue(counterResult, "failed");
            var error = ReadCounterValue(counterResult, "error");

            ExpectCounterValueEqual(counterResult, "executed", total);
            ExpectCounterValueEqual(counterResult, "executed", passed + failed + error);
            foreach (var expectedZeroCounter in _expectedZeroCounters)
            {
                ExpectCounterValueEqual(counterResult, expectedZeroCounter, 0);
            }

            if (passed + failed + error != total)
                throw new Exception($"Inconsistent result : {passed} + {failed} + {error} != {total}");

            return total > 0
                ? new TestResult(resultFileName, total, passed)
                : null;
        }

        private static int ReadCounterValue(XmlElement counterResult, string attributeName)
        {
            return int.Parse(counterResult.Attributes[attributeName].Value);
        }

        private static void ExpectCounterValueEqual(XmlElement counterResult, string attributeName, int expectedValue)
        {
            int actualValue = ReadCounterValue(counterResult, attributeName);
            if (actualValue != expectedValue)
                throw new Exception($"Expected {attributeName} to be {expectedValue}, obtained {actualValue}");
        }
    }
}
