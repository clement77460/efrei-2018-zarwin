using System;
using Zarwin.Shared.Contracts.Core;
using System.Diagnostics;
using Zarwin.Shared.Contracts.Input.Orders;

namespace CholletJaworskiZarwin
{
    public class Soldier : ISoldier
    {

        // ID counter which increments each new Soldier
        public static int soldierCounterId = 1; //public because of the test

        private int killMultiplicator=1;

        public int Id { get; }
        public int Level { get; private set; }
        public int HealthPoints { get; private set; }

        private bool hasShotGun = false;
        private bool hasMachineGun = false;
        private bool hasSniper = false;

        public Soldier()
        {
            this.Id = Soldier.soldierCounterId;
            this.Level = 1;
            this.HealthPoints = 3 + this.Level;
            Soldier.soldierCounterId++;
        }

        public Soldier(int id, int level)
        {
            this.Id = id;
            this.Level = level;
            this.HealthPoints = level + 3;
            Soldier.soldierCounterId++;
        }


        public void Hurt(int damage)
        {
            this.HealthPoints -= damage;
        }

        public void LevelUp()
        {
            this.Level++;
            this.HealthPoints += 1;
        }

        public int Defend(Horde horde, int turn)
        {
            if (!hasSniper)
            {
                decimal calcul = 1 + (this.Level - 1) / 10;// The soldier kill 1 walker, plus 1 every 10 level he reached
                int damages = Convert.ToInt32(Math.Floor(calcul));
                damages = damages * killMultiplicator;
                System.Diagnostics.Debug.WriteLine("j'inflige : " + damages);
                int nbWalkersKilled = horde.DoDamages(damages, turn);// Kill walkers

                for (int i = 0; i < nbWalkersKilled; ++i)
                {
                    this.LevelUp();// Level up for each walker killed
                }
                return nbWalkersKilled;
            }
            return 0;
           
        }

        public int sniping(Horde horde)
        {
            if (hasSniper)
            {
                //on tue un walker
                horde.oneShotWalker();
                this.LevelUp();
                return 1;
            }
            return 0;
        }

        public override String ToString()
        {
            return "Je suis le soldat numero " + this.Id + " pv = " + this.HealthPoints;
        }

        public void SetShotGun()
        {
            this.hasShotGun = true;
        }

        public void SetMachineGun()
        {
            this.hasMachineGun = true;
        }

        public void SetSniper()
        {
            this.hasSniper = true;
        }

        public void UpdateItems(City city)
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


        public static void ResetId()
        {
            Soldier.soldierCounterId = 1;
        }
    }
}
