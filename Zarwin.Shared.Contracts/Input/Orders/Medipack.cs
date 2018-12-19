using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zarwin.Shared.Contracts.Input.Orders
{
    public class Medipack : Order
    {
        [JsonProperty("targetSoldier")]
        public int TargetSoldier { get; }

        [JsonProperty("amount")]
        public int Amount { get; }

        public Medipack(int waveIndex, int turnIndex, int targetSoldier, int amount)
            : base(waveIndex, turnIndex, OrderType.DistributeMedipack)
        {
            TargetSoldier = targetSoldier;
            Amount = amount;
        }
    }
}
