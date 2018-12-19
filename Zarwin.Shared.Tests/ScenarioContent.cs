using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;

namespace Zarwin.Shared.Tests
{
    public class ScenarioContent
    {
        [JsonProperty("input")]
        public Parameters Input { get; }

        [JsonProperty("expectedOutput")]
        public Result ExpectedOutput { get; }

        public ScenarioContent(Parameters input, Result expectedOutput)
        {
            Input = input;
            ExpectedOutput = expectedOutput;
        }
    }
}
