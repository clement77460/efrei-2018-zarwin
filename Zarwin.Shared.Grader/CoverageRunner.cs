using System;
using System.IO;
using System.Xml;

namespace Zarwin.Shared.Grader
{
    public class CoverageRunner
    {
        private readonly string _solutionDirectory;
        private readonly bool _noBuild;

        public CoverageRunner(string solutionDirectory, bool noBuild)
        {
            _solutionDirectory = solutionDirectory;
            _noBuild = noBuild;
        }

        public double Run(TestParameter parameter)
        {
            CleanupFiles();
            ExecuteProcess(parameter);
            var result = ReadResult();

            CleanupFiles();
            return result;
        }

        private void CleanupFiles()
        {
            File.Delete("coverage.json");
            File.Delete("coverage.cobertura.xml");
        }

        private void ExecuteProcess(TestParameter parameter)
        {
            File.WriteAllText("coverage.json", "{}");
            var process = new TestProcess(
                _solutionDirectory,
                _noBuild,
                parameter.ExcludeFilterArg,
                "/p:CollectCoverage=true",
                "/p:CoverletOutput=../",
                "/p:MergeWith=\"../coverage.json\"",
                "/p:CoverletOutputFormat=\\\"json,cobertura\\\"",
                "/p:Exclude=\"[Zarwin.Shared.*]*\"");

            process.Run();
        }

        private double ReadResult()
        {
            try
            {
                var xmlResults = new XmlDocument();
                xmlResults.Load("coverage.cobertura.xml");
                var counterResult = xmlResults["coverage"];

                return double.Parse(counterResult.Attributes["branch-rate"].Value);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unexpected error when computing coverage : {e}; assuming 0");
                return 0;
            }
        }
    }
}
