using System;
using System.Collections.Generic;
using Zarwin.Shared.Contracts.Core;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using System.Linq;
using Zarwin.Shared.Contracts.Input.Orders;

namespace CholletJaworskiZarwin
{
    public class City
    {
        private ActionTrigger actionTrigger;

        private List<Soldier> soldiers;
        private List<Order> orders = new List<Order>();
       
        public Wall Wall { get; private set; }
        public int Coin { get; private set; } = 0;

        public int nbTower { get; private set; } = 0;

        public City(CityParameters cityParameter,SoldierParameters[] soldierParameter,Order[] orders,
            ActionTrigger actionTrigger)
        {
            this.actionTrigger = actionTrigger;

            this.Wall = new Wall(cityParameter.WallHealthPoints);
            this.Coin  = cityParameter.InitialMoney;
            
            this.soldiers = new List<Soldier>();
            this.CreateSoldiersFromParameters(soldierParameter);

            this.orders.AddRange(orders);
            
        }

        public void CheckPastOrders(int waveIndex,int turnIndex)
        {
            foreach(Order o in orders)
            {

                
                if (o.WaveIndex < waveIndex)
                {
                    this.ExecutePastOrders(o);
                }
                else
                {
                    if (o.WaveIndex <= waveIndex && o.TurnIndex < turnIndex)
                    {
                        this.ExecutePastOrders(o);
                    }
                }
            }
        }

        private void ExecutePastOrders(Order o)
        {
            switch (o.Type)
            {
                case OrderType.ReinforceTower:
                    this.nbTower++;
                    break;

                case OrderType.EquipWithSniper:
                    (this.GetSoldierById(((Equipment)o).TargetSoldier))[0].SetSniper();
                    break;

                case OrderType.EquipWithShotgun:
                    (this.GetSoldierById(((Equipment)o).TargetSoldier))[0].SetShotGun();
                    break;

                case OrderType.EquipWithMachineGun:
                    (this.GetSoldierById(((Equipment)o).TargetSoldier))[0].SetMachineGun();
                    break;

                case OrderType.RecruitSoldier:
                    this.AddNewSoldier();
                    break;

                case OrderType.DistributeMedipack:
                    Soldier[] soldier = this.GetSoldierById(((Medipack)o).TargetSoldier);

                    if (soldier.Length > 0)
                        soldier[0].HealMe(((Medipack)o).Amount);
                    break;
            }
        }

        public void GetAttacked(int damage, IDamageDispatcher damageDispatcher)
        {

            if (this.Wall.Health > 0)
            {

                this.Wall.WeakenWall(damage);
            }
            // If the wall has collapsed, the walker attack the soldiers
            else
            {
                Soldier[] soldierHitten =this.HurtSoldiers(damage, damageDispatcher);

                foreach(Soldier s in soldierHitten)
                    actionTrigger.SoldierLosingHp(s.Id, damage);
                
                foreach(Soldier s in soldiers.ToArray())
                {
                    if (s.HealthPoints <= 0)
                    {
                        soldiers.Remove(s);
                        actionTrigger.SoldierDieing(s.Id);
                    }
                }

            }
            
        }

        public Soldier[] HurtSoldiers(int damages, IDamageDispatcher damageDispatcher)
        {
            return damageDispatcher.DispatchDamage(damages, soldiers).ToArray();
        }

        public void DefendFromHorde(Horde horde, int turn)
        {

            int nbWalkersKilled = 0;
            foreach (Soldier soldier in this.soldiers)
            {
                soldier.UpdateItems(this);
                nbWalkersKilled = soldier.Defend(horde, turn);

                this.WalkerHasBeenKilled(soldier, nbWalkersKilled);
                
            }
        }


        public void SnipersAreShoting(Horde horde)
        {
            int nbWalkersKilled = 0;
            foreach (Soldier soldier in this.soldiers)
            {
                nbWalkersKilled = soldier.Sniping(horde);

                this.WalkerHasBeenKilled(soldier, nbWalkersKilled);
            }
        }

        private void WalkerHasBeenKilled(Soldier killer, int nbWalkersKilled)
        {
            if (nbWalkersKilled > 0)
            {
                this.IncreaseCoin(nbWalkersKilled);
                actionTrigger.SoldierStriking(killer, nbWalkersKilled);

            }
        }

        public void AddNewSoldier()
        {
            this.soldiers.Add(new Soldier());
        }

        public int NumberSoldiersAlive
        {
            get
            {
                return this.soldiers.Where(s => s.HealthPoints > 0).ToList().Count;
            }
        }

        // Get a string to display soldiers' ids and HP left
        public String SoldiersStats()
        {
            String stats = "";
            foreach (Soldier soldier in this.soldiers)
            {
                stats += "Soldier " + soldier.Id + " : " + soldier.HealthPoints + "HP. \n";
            }

            return stats;
        }

        public Boolean AreAllSoldiersDead()
        {
            foreach (Soldier soldier in this.soldiers)
            {
                if (soldier.HealthPoints > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Soldier> GetSoldiers()
        {
            return this.soldiers;
        }

        private void CreateSoldiersFromParameters(SoldierParameters[] soldierParameters)
        {
            for (int i = 0; i < soldierParameters.Length; i++)
            {
                soldiers.Add(new Soldier(soldierParameters[i].Id, soldierParameters[i].Level));

            }
        }

        // Returns a list of soldiers states based on the local soldiers list
        public List<SoldierState> GetSoldiersStates()
        {
            List<SoldierState> soldierStates = new List<SoldierState>();
            for (int i = 0; i < soldiers.Count; i++)
            {
                soldierStates.Add(new SoldierState(soldiers[i].Id, soldiers[i].Level, soldiers[i].HealthPoints));
            }
            return soldierStates;
        }


        public void IncreaseCoin(int value) {
            this.Coin  += value;
        }
        public void ExecuteOrder(int turn,int wave,int coin)
        {
            foreach (Order o in orders)
            {
                if (o.TurnIndex == turn && o.WaveIndex==wave)
                {
                    this.GeneratingOrder(o,coin);
                }
            }
        }

        private bool CheckIfEnoughGold(int amountAtStartOfTurn,int amountToReduce)
        {
            if(amountAtStartOfTurn >= amountToReduce)
            {
                this.Coin -= amountToReduce;
                return true;
            }

            return false;
        }

        private void GeneratingOrder(Order o,int amountAtStart)
        {
            
            switch (o)
            {
                case Equipment equipment:
                    this.SetEquipmentToSoldier(equipment,amountAtStart);
                    break;

                case Medipack mediPack:
                    this.BuyMediPack(mediPack,amountAtStart);
                    break;

                case Zarwin.Shared.Contracts.Input.Orders.Wall wallOrder:
                    this.RepairWall(wallOrder, amountAtStart);
                    break;

                default:
                    this.AddOrderForCity(o, amountAtStart);
                    break;
                
            }
        }

        private void SetEquipmentToSoldier(Equipment equipment, int amountAtStart)
        {
            if (this.CheckIfEnoughGold(amountAtStart, 10))
            {
                Soldier[] soldier=this.GetSoldierById(equipment.TargetSoldier);

                switch (equipment.Type)
                {
                    case OrderType.EquipWithShotgun:
                        soldier[0].SetShotGun();
                        break;

                    case OrderType.EquipWithMachineGun:
                        soldier[0].SetMachineGun();
                        break;

                    case OrderType.EquipWithSniper:
                        soldier[0].SetSniper();
                        break;
                }

                soldier[0].UpdateItems(this);
            }

        }

        private void AddOrderForCity(Order o, int amountAtStart)
        {
            if (this.CheckIfEnoughGold(amountAtStart, 10))
            {
                switch (o.Type)
                {
                    case OrderType.RecruitSoldier:
                        this.AddNewSoldier();
                        break;

                    case OrderType.ReinforceTower:
                        this.CreateTower();
                        break;
                }
            }
        }

        private void CreateTower()
        {
            this.nbTower++;
        }

        private void RepairWall(Zarwin.Shared.Contracts.Input.Orders.Wall wallOrder,int amountAtStart)
        {
            int value = wallOrder.Amount;
            if (this.CheckIfEnoughGold(amountAtStart, value ))
                this.Wall.RepairMe(value);
        }

        private void BuyMediPack(Medipack mediPack,int amountAtStart)
        {
            int value = mediPack.Amount;
            if (this.CheckIfEnoughGold(amountAtStart,value))
            {
                Soldier[] soldier = this.GetSoldierById(mediPack.TargetSoldier);
                
                if(soldier.Length>0)
                    soldier[0].HealMe(value);
            }
        }

        private Soldier[] GetSoldierById(int TargetSoldier)
        {
            var soldierLinq = (from s in soldiers
                               where s.Id == TargetSoldier
                               select s);
            return soldierLinq.ToArray(); //fonction a faire
        }

        public void ReplacingOrdersList(Order[] NewOrderList)
        {
            orders = new List<Order>();
            orders.AddRange(NewOrderList);
        }

    }
}
