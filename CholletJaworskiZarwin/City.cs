using System;
using System.Collections.Generic;
using Zarwin.Shared.Contracts.Core;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using System.Diagnostics;
namespace CholletJaworskiZarwin
{
    public class City
    {
        private List<Soldier> soldiers;
        private List<Order> orders = new List<Order>();
        private Wall wall;

        private int coin = 0;

        public Wall Wall => this.wall;

        public int Coin { get => coin; }

        public City(int numberOfSoldiers, int wallHealth)
        {
            
            this.wall = new Wall(wallHealth);

            // Populate the city with Soldiers
            this.soldiers = new List<Soldier>();
            for (int i = 0; i < numberOfSoldiers; i++)
            {
                this.addNewSoldier();
            }
        }

        // Constructor with given parameters
        public City(Parameters parameters)
        {
            this.wall = new Wall(parameters.CityParameters.WallHealthPoints);

            this.coin = parameters.CityParameters.InitialMoney;
            // Populate the city with Soldiers
            this.soldiers = new List<Soldier>();
            this.CreateSoldiersFromParameters(parameters.SoldierParameters);

            this.orders.AddRange(parameters.Orders);
            
        }

        public void HurtSoldiers(int damages, IDamageDispatcher damageDispatcher)
        {
            damageDispatcher.DispatchDamage(damages, soldiers);
        }

        public void getAttacked(int damage, IDamageDispatcher damageDispatcher)
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

        public void DefendFromHorde(Horde horde)
        {
            int goldAmount = 0;
            foreach (Soldier soldier in this.soldiers)
            {
                
                goldAmount=soldier.Defend(horde);
                this.increaseCoin(goldAmount);
            }
        }

        public void addNewSoldier()
        {
            this.soldiers.Add(new Soldier());
        }

        public int GetNumberSoldiersAlive()
        {
            int numberSoldiersAlive = 0;
            foreach (Soldier soldier in this.soldiers)
            {
                if (soldier.HealthPoints > 0)
                {
                    numberSoldiersAlive++;
                }
            }
            return numberSoldiersAlive;
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


        public void increaseCoin(int value) {
            this.coin += value;
        }
        public void executeOrder(int turn,int wave)
        {
            foreach (Order o in orders)
            {
                if (o.TurnIndex == turn && o.WaveIndex==wave)
                {
                    if (coin >= 10)
                    {
                        this.coin -= 10;
                        this.generatingOrder(o.Type);
                    }
                }
            }
        }

        private void generatingOrder(OrderType type)
        {
            switch (type)
            {
                case OrderType.RecruitSoldier:
                    this.addNewSoldier();
                    break;
                case OrderType.EquipWithShotgun:
                    break;
                case OrderType.EquipWithMachineGun:
                    break;
            }
        }
    }
}
