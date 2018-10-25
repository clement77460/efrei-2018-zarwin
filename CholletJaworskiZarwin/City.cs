using System;
using System.Collections.Generic;
using Zarwin.Shared.Contracts.Core;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using System.Linq;

namespace CholletJaworskiZarwin
{
    public class City
    {
        private List<Soldier> soldiers;
        private List<Order> orders = new List<Order>();
       
        public Wall Wall { get; private set; }
        public int Coin { get; private set; } = 0;

        public City(int numberOfSoldiers, int wallHealth)
        {
            
            this.Wall = new Wall(wallHealth);

            // Populate the city with Soldiers
            this.soldiers = new List<Soldier>();
            for (int i = 0; i < numberOfSoldiers; i++)
            {
                this.AddNewSoldier();
            }
        }

        // Constructor with given parameters
        public City(Parameters parameters)
        {
            this.Wall = new Wall(parameters.CityParameters.WallHealthPoints);

            this.Coin  = parameters.CityParameters.InitialMoney;
            // Populate the city with Soldiers
            this.soldiers = new List<Soldier>();
            this.CreateSoldiersFromParameters(parameters.SoldierParameters);

            this.orders.AddRange(parameters.Orders);
            
        }

        public void HurtSoldiers(int damages, IDamageDispatcher damageDispatcher)
        {
            damageDispatcher.DispatchDamage(damages, soldiers);
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
                this.HurtSoldiers(damage, damageDispatcher);
                
                //checking if soldier is dead
                foreach(Soldier s in soldiers.ToArray())
                {
                    if (s.HealthPoints <= 0)
                        soldiers.Remove(s);
                }

            }
            
        }

        public void DefendFromHorde(Horde horde, int turn)
        {

            int goldAmount = 0;
            foreach (Soldier soldier in this.soldiers)
            {
                soldier.UpdateItems(this);
                goldAmount=soldier.Defend(horde, turn);
                this.IncreaseCoin(goldAmount);
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
        public void ExecuteOrder(int turn,int wave)
        {
            foreach (Order o in orders)
            {
                if (o.TurnIndex == turn && o.WaveIndex==wave)
                {
                    if (Coin  >= 10)
                    {
                        this.Coin  -= 10;
                        this.GeneratingOrder(o);
                    }
                }
            }
        }

        private void GeneratingOrder(Order o)
        {
            switch (o.Type)
            {
                case OrderType.RecruitSoldier:
                    this.AddNewSoldier();
                    break;
                case OrderType.EquipWithShotgun:
                    this.EquipShotGunToSoldier(o.TargetSoldier);
                    break;
                case OrderType.EquipWithMachineGun:
                    this.EquipMachineGunToSoldier(o.TargetSoldier);
                    break;
            }
        }

        private void EquipShotGunToSoldier(int? index)
        {
            var soldierLinq = (from s in soldiers
                               where s.Id == index
                               select s);
            Soldier[] soldier = soldierLinq.ToArray();
            soldier[0].SetShotGun();
            soldier[0].UpdateItems(this);
            
        }
        private void EquipMachineGunToSoldier(int? index)
        {
            var soldierLinq = (from s in soldiers
                               where s.Id == index
                               select s);
            Soldier[] soldier = soldierLinq.ToArray();
            soldier[0].SetMachineGun();
            soldier[0].UpdateItems(this);

        }
    }
}
