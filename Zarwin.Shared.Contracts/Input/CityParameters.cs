using Newtonsoft.Json;

namespace Zarwin.Shared.Contracts.Input
{
    public class CityParameters
    {
        [JsonProperty("wallHealthPoints")]
        public int WallHealthPoints { get; }

        public CityParameters(int wallHealthPoints)
        {
            WallHealthPoints = wallHealthPoints;
        }
    }
}
