using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zarwin.Shared.Grader
{
    public class Badger : IDisposable
    {
        private const string _mainBranch = "prod";

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _branchName;

        public Badger(string branchName)
        {
            _branchName = branchName;
        }

        public void Dispose()
            => _httpClient.Dispose();

        public Task MakeGradeBadge(double grade)
            => MakeBadge("note", Math.Round(grade, 2, MidpointRounding.AwayFromZero) + "/20", "red");

        public Task MakeCompletionBadge(double grade)
            => MakeBadge("completude", Math.Round(grade, 2, MidpointRounding.AwayFromZero) + "/10", "orange");

        public Task MakeDeliveryBadge(double grade)
            => MakeBadge("livraison", Math.Round(grade, 2, MidpointRounding.AwayFromZero) + "/10", "yellow");

        public Task MakeQualityBadge(double ratio)
            => MakeBadge("qualite", Math.Round(ratio * 100) + "%", "blue");

        public Task MakeCoverageBadge(double ratio)
            => MakeBadge("couverture", Math.Round(ratio * 100) + "%", "green");

        public async Task MakeBadge(string subject, string status, string color)
        {
            var response = await _httpClient.GetAsync(CreateUrl(subject, status, color));
            response.EnsureSuccessStatusCode();

            var downloadStream = await response.Content.ReadAsStreamAsync();
            using (var fileStream = new FileStream(subject + ".svg", FileMode.Create))
            {
                await downloadStream.CopyToAsync(fileStream);
            }
        }

        private const string urlFormat = "https://img.shields.io/badge/{0}-{1}-{2}.svg";
        private string CreateUrl(string subject, string status, string color)
        {
            var colorCodes = _branchName == "prod" 
                ? _brightColors
                : _darkColors;
            var colorCode = colorCodes[color];

            return string.Format(
                urlFormat,
                UrlEncode(subject + " " + _branchName),
                UrlEncode(status),
                colorCode);
        }

        // The service we use interprets "+" litterally.
        private string UrlEncode(string text)
            => WebUtility.UrlEncode(text).Replace("+", "%20");

        private static Dictionary<string, string> _brightColors = new Dictionary<string, string>()
        {
            ["red"] = "e05d44",
            ["blue"] = "007ec6",
            ["green"] = "97CA00",
            ["orange"] = "fe7d37",
            ["yellow"] = "dfb317",
        };

        private static Dictionary<string, string> _darkColors = new Dictionary<string, string>()
        {
            ["red"] = "a3311b",
            ["blue"] = "003d60",
            ["green"] = "4b6400",
            ["orange"] = "ce4901",
            ["yellow"] = "83690d",
        };
    }
}
