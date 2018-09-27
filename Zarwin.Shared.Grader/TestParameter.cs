using System.Collections.Generic;
using System.Linq;

namespace Zarwin.Shared.Grader
{
    public class TestParameter
    {
        public string GradeName { get; }
        public string[] TraitValues { get; }

        public TestParameter(string gradeName, params string[] traitValues)
        {
            GradeName = gradeName;
            TraitValues = traitValues;
        }

        public string IncludeFilterArg => $"--filter=\"{string.Join("|", GetFilterComponents("="))}\"";
        public string ExcludeFilterArg => $"--filter=\"{string.Join("&", GetFilterComponents("!="))}\"";

        public IEnumerable<string> GetFilterComponents(string @operator)
            => TraitValues.Select(value => "grading" + @operator + value);
    }
}
