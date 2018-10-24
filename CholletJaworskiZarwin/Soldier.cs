using System;
using Zarwin.Shared.Contracts.Core;
using System.Diagnostics;

namespace CholletJaworskiZarwin
{
    public class Soldier : ISoldier
    {

        // ID counter which increments each new Soldier
        public static int soldierCounterId = 1; //public because of the test

        private int soldierId;
        private int level;
        private int health;
        private int killMultiplicator=1;

        // Accessors
        public int Id => this.soldierId;
        public int HealthPoints => this.health;
        public int Level => this.level;

        private bool hasShotGun = false;
        private bool hasMachineGun = false;

        public Soldier()
        {
            this.soldierId = Soldier.soldierCounterId;
            this.level = 1;
            this.health = 3 + this.level;
            Soldier.soldierCounterId++;
        }

        public Soldier(int id, int level)
        {
            this.soldierId = id;
            this.level = level;
            this.health = level + 3;
            Soldier.soldierCounterId++;
        }


        public void Hurt(int damage)
        {
            this.health -= damage;
        }

        public void LevelUp()
        {
            this.level++;
            this.health += 1;
        }

        public int Defend(Horde horde)
        {
            // The soldier kill 1 walker, plus 1 every 10 level he reached
            decimal calcul = 1 + (level - 1) / 10;
            int numberToKill = Convert.ToInt32(Math.Floor(calcul));
            numberToKill = numberToKill * killMultiplicator;
            // Kill walkers
            int nbWalkersKilled = horde.KillWalkers(numberToKill);
            // Level up for each walker killed
            for (int i = 0; i < nbWalkersKilled; ++i)
            {
                this.LevelUp();
            }

            return nbWalkersKilled;
        }

        public override String ToString()
        {
            return "Je suis le soldat numero " + this.soldierId + " pv = " + this.HealthPoints;
        }

        public void SetShotGun()
        {
            this.hasShotGun = true;
        }

        public void SetMachineGun()
        {
            this.hasMachineGun = true;
        }

        public void updateItems(City city)
        {
            killMultiplicator = 1;
            if (hasShotGun)
            {
                if (city.Wall.Health <= 0)
                {
                    killMultiplicator = 2;
                }
            }
            if (hasMachineGun)
            {
                if (city.Wall.Health > 0)
                {
                    killMultiplicator = 4;
                }
            }
        }
    }
}
