using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Zarwin.Shared.Grader
{
    class Program
    {
        private static int _firstVersion = 2;
        private static int _currentVersion = 1;

        private static readonly string[] _defaultQualityCriteria = new[]
        {
            "Nommages variables",
            "Nommages et types de membres",
            "Structure fonctions",
            "Structure classes",
            "Pas de duplications de code",
            "Séparation de responsabilités",
            "Encapsulation",
            "Tests unitaires"
        };

        static async Task Main(string[] args)
        {
            var branchName = args.Length > 0 ? args[0] : "local";
            var program = new Program(".", true, _currentVersion);

            var grade = ReadGrade() ?? new CompleteGrade();
            program.UpdateGrade(grade);
            WriteGrade(grade);

            using (var badger = new Badger(branchName))
            {
                await badger.MakeGradeBadge(grade.Grade);
                await badger.MakeCompletionBadge(grade.CompletionGrade);
                await badger.MakeDeliveryBadge(grade.DeliveryGrade);
                await badger.MakeQualityBadge(grade.QualityRate);
                await badger.MakeCoverageBadge(grade.TestCoverage);
            }
        }

        private static CompleteGrade ReadGrade()
        {
            if (!File.Exists("grade.json"))
                return null;

            return JsonConvert.DeserializeObject<CompleteGrade>(File.ReadAllText("grade.json"));
        }

        private static void WriteGrade(CompleteGrade grade)
        {
            File.WriteAllText("grade.json", JsonConvert.SerializeObject(grade, Formatting.Indented));
        }

        private readonly TestRunner _testRunner;
        private readonly CoverageRunner _coverageRunner;

        private readonly TestParameter _currentVersionTests;
        private readonly TestParameter _completudeTests;

        public Program(string solutionDirectory, bool noBuild, int version)
        {
            _testRunner = new TestRunner(solutionDirectory, noBuild);
            _coverageRunner = new CoverageRunner(solutionDirectory, noBuild);

            _currentVersionTests = new TestParameter("Livraison V" + version, "v" + version);
            _completudeTests = new TestParameter("Complétude",
                GenerateCompletionTests(version).ToArray());
        }

        private static IEnumerable<string> GenerateCompletionTests(int version)
        {
            return Enumerable.Range(1, version).Select(v => "v" + v)
                .Concat(new[] { "final" });
        }

        public void UpdateGrade(CompleteGrade grade)
        {
            InitializeGradeIfEmpty(grade);

            if (_currentVersion >= _firstVersion)
            {
                grade.TestResults[_completudeTests.GradeName] = _testRunner.RunTests(_completudeTests);
                grade.TestResults[_currentVersionTests.GradeName] = _testRunner.RunTests(_currentVersionTests);
            }
            grade.TestCoverage = _coverageRunner.Run(_completudeTests);
        }

        public void InitializeGradeIfEmpty(CompleteGrade grade)
        {
            if (grade.QualityGrades == null)
            {
                grade.QualityGrades = _defaultQualityCriteria.ToDictionary(
                    criteria => criteria,
                    criteria => (double)1);
            }

            if (grade.TestResults == null)
                grade.TestResults = new Dictionary<string, TestResult>();

            if (!grade.TestResults.ContainsKey(_completudeTests.GradeName))
                grade.TestResults[_completudeTests.GradeName] = TestResult.Maximum;
        }
    }
}
