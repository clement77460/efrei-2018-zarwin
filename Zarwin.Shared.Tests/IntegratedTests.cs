using Newtonsoft.Json;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Zarwin.Shared.Contracts;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using Zarwin.Shared.Tests.Infrastructure;

namespace Zarwin.Shared.Tests
{
    public abstract partial class IntegratedTests
    {
        public abstract IInstantSimulator CreateSimulator();

        [ScenarioTheory]
        [MemberData(nameof(ScenarioData), DisableDiscoveryEnumeration = false)]
        public void AllScenario(Scenario scenario)
        {
            scenario.Run(CreateSimulator());
        }

        public static object[][] ScenarioData => Scenarios.Select(s => new object[] { s }).ToArray();

        public class Scenario : IXunitSerializable
        {
            public string Name { get; private set; }
            public string Version { get; private set; }

            public override string ToString() => Name;

            private Parameters _input;
            private Result _expectedOuput;

            public Scenario(string name, string version, Parameters input, Result expectedOuput)
            {
                Name = name;
                Version = version;
                _input = input;
                _expectedOuput = expectedOuput;
            }

            public void Run(IInstantSimulator simulator)
            {
                if (_input == null || _expectedOuput == null)
                    throw new InvalidOperationException("Cannot execute a scenario without input or expectedOutput");

                var actualOutput = simulator.Run(_input);

                Assert.Equal(_expectedOuput, actualOutput);
            }

            #region IXunitSerializable

            [Obsolete("Shouldn't call this directly", true)]
            public Scenario()
            {
            }

            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue("name", Name);
                info.AddValue("version", Version);
                info.AddValue("input", JsonConvert.SerializeObject(_input));
                info.AddValue("expectedOutput", JsonConvert.SerializeObject(_expectedOuput));
            }

            public void Deserialize(IXunitSerializationInfo info)
            {
                Name = info.GetValue<string>("name");
                Version = info.GetValue<string>("version");

                var jsonInput = info.GetValue<string>("input");
                var jsonExpectedOutput = info.GetValue<string>("expectedOutput");
                
                _input = JsonConvert.DeserializeObject<Parameters>(jsonInput);
                _expectedOuput = JsonConvert.DeserializeObject<Result>(jsonExpectedOutput);
            }

            #endregion IXunitSerializable
        }
    }
}
