using Newtonsoft.Json;

namespace Zarwin.Shared.Grader
{
    public class TestResult
    {
        [JsonIgnore]
        public string FileName { get; }

        [JsonProperty("Total")]
        public int Total { get; }

        [JsonProperty("Succès")]
        public int Passed { get; }

        [JsonProperty("Taux")]
        public double SuccessRate => Total > 0
            ? Passed * 1.0 / Total
            : 0;

        public TestResult(string fileName, int total, int passed)
        { 
            FileName = fileName;
            Total = total;
            Passed = passed;
        }

        public static TestResult Inconclusive
            => new TestResult(fileName: null, total: 0, passed: 0);

        public static TestResult Maximum
            => new TestResult(fileName: null, total: 1, passed: 1);
    }
}
