using Newtonsoft.Json;
using System;
using Zarwin.Shared.Contracts.Core;

namespace Zarwin.Shared.Contracts.Input
{
    public class Parameters
    {
        [JsonProperty("wavesToRun")]
        public int WavesToRun { get; }

        [JsonIgnore]
        public IDamageDispatcher DamageDispatcher { get; }

        [JsonProperty("damageDispatcher")]
        public string DamageDispatcherType
        {
            get { return DamageDispatcher.GetType().AssemblyQualifiedName; }
        }

        private static IDamageDispatcher CreateDispatcher(string typeName)
        {
            var type = Type.GetType(typeName);
            var dispatcher = Activator.CreateInstance(type) as IDamageDispatcher;

            if (dispatcher == null)
                throw new InvalidOperationException($"Dispatcher {typeName} does not exist");

            return dispatcher;
        }

        [JsonProperty("horde")]
        public HordeParameters HordeParameters { get; }

        [JsonProperty("soldiers")]
        public SoldierParameters[] SoldierParameters { get; }

        [JsonProperty("city")]
        public CityParameters CityParameters { get; }

        public Parameters(
            int wavesToRun, 
            IDamageDispatcher damageDispatcher,
            HordeParameters hordeParameters,
            CityParameters cityParameters,
            params SoldierParameters[] soldierParameters)
        {
            WavesToRun = wavesToRun;
            DamageDispatcher = damageDispatcher;
            HordeParameters = hordeParameters;
            SoldierParameters = soldierParameters;
            CityParameters = cityParameters;
        }

        // This constructor must not be used. It must be used solely by the deserialization process.
        [JsonConstructor]
        private Parameters(
            int wavesToRun,
            string damageDispatcher,
            HordeParameters hordeParameters,
            CityParameters cityParameters,
            params SoldierParameters[] soldierParameters)
            : this(wavesToRun, CreateDispatcher(damageDispatcher), hordeParameters, cityParameters, soldierParameters)
        { }
    }
}
