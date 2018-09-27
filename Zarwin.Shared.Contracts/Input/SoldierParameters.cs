using Newtonsoft.Json;

namespace Zarwin.Shared.Contracts.Input
{
    public class SoldierParameters
    {
        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("level")]
        public int Level { get; }

        public SoldierParameters(int id, int level)
        {
            Id = id;
            Level = level;
        }
    }
}
