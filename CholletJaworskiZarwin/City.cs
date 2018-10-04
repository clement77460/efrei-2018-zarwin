using System;
using System.Collections.Generic;
using CholletJaworskiZarwin;

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
        
        public void HurtSoldiers(int damages, DamageDispatcher damageDispatcher)
        {
            damageDispatcher.DispatchDamage(damages, soldiers);
        }

        public void DefendFromHorde(Horde horde)
        {
            foreach(Soldier soldier in this.soldiers)
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

        public String SoldiersStats()
        {
            String stats = "";
            foreach(Soldier soldier in this.soldiers)
            {
                stats += "Soldier " + soldier.Id + " : " + soldier.HealthPoints + "HP. \n";
            }

            return stats;
        }

        public Boolean AreAllSoldiersDead()
        {
            foreach(Soldier soldier in this.soldiers)
            {
                if(soldier.HealthPoints > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public Wall GetWall()
        {
            return this.wall;
        }
    }
}
