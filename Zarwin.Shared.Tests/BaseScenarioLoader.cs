using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Zarwin.Shared.Tests
{
    public abstract class BaseScenarioLoader<TScenario, TScenarioContent>
    {
        public IEnumerable<TScenario> GetAllScenarios()
        {
            var assemblyFolder = Path.GetDirectoryName(typeof(IntegratedTests).Assembly.Location);
            var scenariosFolder = Path.Combine(assemblyFolder, ScenarioFolderName);
            return Directory.EnumerateDirectories(scenariosFolder)
                .SelectMany(GetVersionScenarios);
        }

        protected abstract string ScenarioFolderName { get; }

        public IEnumerable<TScenario> GetVersionScenarios(string versionPath)
        {
            return Directory.EnumerateFiles(versionPath)
                .Select(GetScenarioFromFile);
        }

        public TScenario GetScenarioFromFile(string scenarioPath)
        {
            var scenarioName = Path.GetFileNameWithoutExtension(scenarioPath);
            string version = Path.GetFileName(Path.GetDirectoryName(scenarioPath));

            using (var fileStream = new FileStream(scenarioPath, FileMode.Open))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var content = new JsonSerializer().Deserialize<TScenarioContent>(jsonReader);
                return CreateScenario(scenarioName, version, content);
            }
        }

        public abstract TScenario CreateScenario(string scenarioName, string version, TScenarioContent content);
    }
}
