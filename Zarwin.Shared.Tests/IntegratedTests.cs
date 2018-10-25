using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        [MemberData(nameof(GetAllScenariosData), DisableDiscoveryEnumeration = false)]
        public void AllScenario(Scenario scenario)
        {
            scenario.Run(CreateSimulator());
        }

        public static object[][] GetAllScenariosData() => new ScenarioLoader()
            .GetAllScenarios()
            .Select(s => new object[] { s }).ToArray();


        public class Scenario : IXunitSerializable
        {
            public string Name { get; private set; }
            public string Version { get; private set; }

            public override string ToString() => Name;

            private ScenarioContent _content;

            public Scenario(string name, string version, ScenarioContent content)
            {
                Name = name;
                Version = version;
                _content = content;
            }

            public Scenario(string name, string version, Parameters input, Result expectedOuput)
                : this(name, version, new ScenarioContent(input, expectedOuput))
            {
            }

            public void Run(IInstantSimulator simulator)
            {
                if (_content == null)
                    throw new InvalidOperationException("Cannot execute a scenario without input or expectedOutput");

                var actualOutput = simulator.Run(_content.Input);

                Assert.Equal(_content.ExpectedOutput, actualOutput);
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
                info.AddValue("content", JsonConvert.SerializeObject(_content));
            }

            public void Deserialize(IXunitSerializationInfo info)
            {
                Name = info.GetValue<string>("name");
                Version = info.GetValue<string>("version");

                var jsonContent = info.GetValue<string>("content");
                
                _content = JsonConvert.DeserializeObject<ScenarioContent>(jsonContent);
            }

            #endregion IXunitSerializable
        }

        public class ScenarioLoader : BaseScenarioLoader<Scenario, ScenarioContent>
        {
            public override Scenario CreateScenario(string scenarioName, string version, ScenarioContent content)
                => new Scenario(scenarioName, version, content);

            protected override string ScenarioFolderName => "CompleteScenarios";
        }
    }
}
