using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Zarwin.Shared.Contracts.Input
{
    public class ZombieParameter
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ZombieType Type { get; }

        [JsonProperty("trait")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ZombieTrait Trait { get; }

        [JsonProperty("count")]
        public int Count { get; }

        public ZombieParameter(ZombieType type, ZombieTrait trait, int count)
        {
            Type = type;
            Trait = trait;
            Count = count;
        }

        public override bool Equals(object obj)
        {
            return obj is ZombieParameter other
                && other.Type == Type
                && other.Trait == Trait
                && other.Count == Count;
        }

        public override int GetHashCode()
        {
            var hashCode = 1094353492;
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Trait.GetHashCode();
            hashCode = hashCode * -1521134295 + Count.GetHashCode();
            return hashCode;
        }
    }
}
