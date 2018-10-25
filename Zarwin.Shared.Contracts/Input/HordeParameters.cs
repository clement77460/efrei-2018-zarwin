using Newtonsoft.Json;

namespace Zarwin.Shared.Contracts.Input
{
    public class HordeParameters
    {
        [JsonProperty("waves")]
        public WaveHordeParameters[] Waves { get; }

        [JsonConstructor]
        public HordeParameters(params WaveHordeParameters[] waves)
        {
            Waves = waves;
        }

        public HordeParameters(int size)
            : this(new WaveHordeParameters(size))
        {
        }
    }
}
