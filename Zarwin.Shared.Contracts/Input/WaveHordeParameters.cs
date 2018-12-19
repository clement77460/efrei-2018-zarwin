using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zarwin.Shared.Contracts.Input
{
    public class WaveHordeParameters
    {
        [JsonProperty("zombieParameters")]
        public ZombieParameter[] ZombieParameters { get; }

        [JsonIgnore]
        [Obsolete("Use " + nameof(ZombieParameters) + " instead")]
        public ZombieParameter[] ZombieTypes => ZombieParameters;

        [JsonConstructor]
        public WaveHordeParameters(params ZombieParameter[] zombieParameters)
        {
            ZombieParameters = zombieParameters;
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
