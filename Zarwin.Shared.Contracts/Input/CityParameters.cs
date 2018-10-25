using Newtonsoft.Json;

namespace Zarwin.Shared.Contracts.Input
{
    public class CityParameters
    {
        [JsonProperty("initialMoney")]
        public int InitialMoney { get; }

        [JsonProperty("wallHealthPoints")]
        public int WallHealthPoints { get; }

        public CityParameters(int wallHealthPoints, int initialMoney = 0)
        {
            WallHealthPoints = wallHealthPoints;
            InitialMoney = initialMoney;
        }
    }
}
