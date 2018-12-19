using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zarwin.Shared.Contracts.Input
{
    public class Order
    {
        [JsonProperty("waveIndex")]
        public int WaveIndex { get; }

        [JsonProperty("turnIndex")]
        public int TurnIndex { get; }

        [JsonProperty("type")]
        public OrderType Type { get; }

        [JsonProperty("targetSoldier")]
        public int? TargetSoldier { get; }

        public Order(int waveIndex, int turnIndex, OrderType type, int? targetSoldier = null)
        {
            WaveIndex = waveIndex;
            TurnIndex = turnIndex;
            Type = type;
            TargetSoldier = targetSoldier;
        }
    }
}
