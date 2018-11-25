using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zarwin.Shared.Contracts.Input.Orders
{
    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(Wall), OrderType.ReinforceWall)]
    [JsonSubtypes.KnownSubType(typeof(Equipment), OrderType.EquipWithMachineGun)]
    [JsonSubtypes.KnownSubType(typeof(Equipment), OrderType.EquipWithShotgun)]
    [JsonSubtypes.KnownSubType(typeof(Equipment), OrderType.EquipWithSniper)]
    [JsonSubtypes.KnownSubType(typeof(Medipack), OrderType.DistributeMedipack )]
    public class Order
    {
        [JsonProperty("waveIndex")]
        public int WaveIndex { get; }

        [JsonProperty("turnIndex")]
        public int TurnIndex { get; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType Type { get; }

        public Order(int waveIndex, int turnIndex, OrderType type)
        {
            WaveIndex = waveIndex;
            TurnIndex = turnIndex;
            Type = type;
        }
    }
}
