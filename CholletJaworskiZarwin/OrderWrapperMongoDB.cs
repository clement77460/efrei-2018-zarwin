using System;
using System.Collections.Generic;
using System.Text;
using Zarwin.Shared.Contracts.Input.Orders;

namespace CholletJaworskiZarwin
{
    public class OrderWrapperMongoDB
    {
        public int WaveIndex { get; set; }
        public int TurnIndex { get; set; }
        public OrderType Type { get; set; }

        public int Amount { get; set; } //wall + MediPack
        public int TargetSoldier { get; set; }//Equipment +MediPack

        public OrderWrapperMongoDB(Order order)
        {
            WaveIndex = order.WaveIndex;
            TurnIndex = order.TurnIndex;
            Type = order.Type;

            this.FillSecondStat(order);
        }

        public void FillSecondStat(Order order)
        {
            switch (order)
            {
                case Equipment equipment:
                    this.TargetSoldier=equipment.TargetSoldier;
                    break;

                case Medipack mediPack:
                    this.TargetSoldier = mediPack.TargetSoldier;
                    this.Amount = mediPack.Amount;
                    break;

                case Zarwin.Shared.Contracts.Input.Orders.Wall wallOrder:
                    this.Amount = wallOrder.Amount;
                    break;
                    

            }
        }

        public Order ToOrder()
        {
            switch (Type)
            {
                case OrderType.EquipWithSniper:
                    return new Equipment(this.WaveIndex, this.TurnIndex, OrderType.EquipWithSniper,this.TargetSoldier);

                case OrderType.ReinforceTower:
                    return new Order(this.WaveIndex, this.TurnIndex, OrderType.ReinforceTower);
            }
            return null;
        }

    }
}
