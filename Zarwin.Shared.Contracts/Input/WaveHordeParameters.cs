using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Zarwin.Shared.Contracts.Input
{
    public class WaveHordeParameters
    {
        [JsonProperty("zombieTypes")]
        public ZombieParameter[] ZombieTypes { get; }
        
        [JsonConstructor]
        public WaveHordeParameters(params ZombieParameter[] zombieTypes)
        {
            ZombieTypes = zombieTypes;
        }

        public WaveHordeParameters(int size)
            : this(CreateStalkerWave(size))
        {
        }

        private static ZombieParameter[] CreateStalkerWave(int size)
        {
            return new[]
            {
                new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, size)
            };
        }
    }
}
