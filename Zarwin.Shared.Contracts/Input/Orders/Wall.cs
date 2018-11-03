using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zarwin.Shared.Contracts.Input.Orders
{
    public class Wall : Order
    {
        [JsonProperty("amount")]
        public int Amount { get; }

        public Wall(int waveIndex, int turnIndex, int amount)
            : base(waveIndex, turnIndex, OrderType.ReinforceWall)
        {
            Amount = amount;
        }
    }
}
