using System;
using Zarwin.Shared.Contracts.Core;

namespace zombieLand
{
    class Soldier : ISoldier
    {

        // ID counter which increments each new Soldier
        static int soldierCounterId = 0;

        private readonly int soldierId;
        private int level;
        private int health;


        // Accessors
        public int Id => this.soldierId;
        public int HealthPoints => this.health;
        public int Level => this.level;


        public Soldier()
        {
            this.soldierId = Soldier.soldierCounterId;
            this.level = 1;
            this.health = 3 + this.level;
            Soldier.soldierCounterId++;
        }

        public void Hurt(int damage)
        {
            this.health -= damage;
        }

        public void LevelUp()
        {
            this.level++;
            this.health += this.level;
        }

        public void Defend(Horde horde)
        {
            // The soldier kill 1 walker, plus 1 every 10 level he reached
            horde.KillWalkers(1 + ((this.level % 10)-1));
        }

        public override String ToString()
        {
            return "Je suis le soldat numero " + this.soldierId;
        }
    }
}
