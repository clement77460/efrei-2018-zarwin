using System;
using System.Collections.Generic;
using Zarwin.Shared.Contracts.Core;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;

namespace CholletJaworskiZarwin
{
    public class City
    {
        private List<Soldier> soldiers;
        private Wall wall;

        public Wall Wall => this.wall;

        public City(int numberOfSoldiers, int wallHealth)
        {
            this.wall = new Wall(wallHealth);

            // Populate the city with Soldiers
            this.soldiers = new List<Soldier>();
            for (int i = 0; i < numberOfSoldiers; i++)
            {
                this.soldiers.Add(new Soldier());
            }
        }

        // Constructor with given parameters
        public City(Parameters parameters)
        {
            this.wall = new Wall(parameters.CityParameters.WallHealthPoints);

            // Populate the city with Soldiers
            this.soldiers = new List<Soldier>();
            this.CreateSoldiersFromParameters(parameters.SoldierParameters);
        }

        public void HurtSoldiers(int damages, IDamageDispatcher damageDispatcher)
        {
            damageDispatcher.DispatchDamage(damages, soldiers);
        }

        public void DefendFromHorde(Horde horde)
        {
            foreach (Soldier soldier in this.soldiers)
            {
                soldier.Defend(horde);
            }
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
    }
}
