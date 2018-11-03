using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Zarwin.Shared.Tests
{
    public abstract class BaseScenarioLoader<TScenario, TScenarioContent>
    {
        protected abstract string ScenarioFolderName { get; }

        private string GetScenarioFolder()
        {
            var assemblyFolder = Path.GetDirectoryName(typeof(IntegratedTests).Assembly.Location);
            return Path.Combine(assemblyFolder, ScenarioFolderName);
        }

        public IEnumerable<TScenario> GetAllScenarios()
        {
            string scenariosFolder = GetScenarioFolder();
            return Directory.EnumerateDirectories(scenariosFolder)
                .SelectMany(GetVersionScenarios);
        }

        public IEnumerable<TScenario> GetVersionScenarios(string versionPath)
        {
            return Directory.EnumerateFiles(versionPath)
                .Select(GetScenarioFromFile);
        }

        public TScenario GetScenario(string version, string scenarioName)
        {
            var scenarioPath = Path.Combine(GetScenarioFolder(), version, scenarioName + ".json");
            return GetScenarioFromFile(scenarioPath);
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
