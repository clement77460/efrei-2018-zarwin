using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Zarwin.Shared.Grader
{
    public class CompleteGrade
    {    
        [JsonProperty("Evaluation de qualité")]
        public Dictionary<string, double> QualityGrades { get; set; }

        [JsonIgnore]
        public double TestCoverage
        {
            get => QualityGrades["Tests unitaires"];
            set => QualityGrades["Tests unitaires"] = value;
        }

        [JsonProperty("Coefficient de qualité")]
        public double QualityRate 
            => 0.5 + 0.5 * QualityGrades.Values.Average();

        [JsonProperty("Résultats des tests")]
        public Dictionary<string, TestResult> TestResults { get; set; }

        [JsonProperty("Note de livraison")]
        public double DeliveryGrade
        {
            get
            {
                var deliveryRate = TestResults
                    .Where(pair => pair.Key.StartsWith("Livraison "))
                    .Average(result => (double?)result.Value.SuccessRate)
                    ?? 1;

                return deliveryRate * 10;
            }
        }

        [JsonProperty("Note de complétude")]
        public double CompletionGrade
        {
            get
            {
                return TestResults["Complétude"].SuccessRate * 10;
            }
        }

        [JsonProperty("Note totale")]
        public double Grade
        {
            get
            {
                var deliveryRates = TestResults
                    .Where(pair => pair.Key.StartsWith("Livraison "))
                    .Average(result => (double?)result.Value.SuccessRate)
                    ?? 1;

                var completeRate = TestResults["Complétude"].SuccessRate;

                return (DeliveryGrade + CompletionGrade) * QualityRate;
            }
        }
    }
}
