using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zarwin.Shared.Contracts.Input.Orders
{
    public class Equipment: Order
    {
        [JsonProperty("targetSoldier")]
        public int TargetSoldier { get; }

        public Equipment(int waveIndex, int turnIndex, OrderType type, int targetSoldier)
            : base(waveIndex, turnIndex, type)
        {
            EnsureValidEquipment(type);
            TargetSoldier = targetSoldier;
        }

        private void EnsureValidEquipment(OrderType equipment)
        {
            switch (equipment)
            {
                case OrderType.EquipWithMachineGun:
                case OrderType.EquipWithShotgun:
                case OrderType.EquipWithSniper:
                    return;
                default:
                    throw new ArgumentException($"{equipment} is not a valid equipment", "equipment");
            }
        }
    }
}
