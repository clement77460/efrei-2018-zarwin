using System;
using System.Collections.Generic;
using System.Text;

namespace zombieLand
{
    class Soldier
    {

        static int id = 0;

        int level=1;
        int soldierId;
        int maxHealth;
        int NumberOfTarget = 1;

        int health;
        
        public Soldier()
        {
            this.soldierId = id;
            Soldier.id++;
            this.maxHealth = 3 + level;
            this.health = maxHealth;
        }

        public void levelUp()
        {
            this.level++;
            //ou juste ajouter ancien level - nouveau lvl???
            this.maxHealth += level;
            this.health += level;
            

            if (this.level % 10 == 0)
            {
                this.NumberOfTarget++;
            }
        }



        public void toString()
        {
            Console.WriteLine("je suis le soldat numero :{0}", this.soldierId);
        }

        public void reduceHealth()
        {
            this.health--;
            Console.Write("[SOLDAT {0}] il me reste :");
            Console.Write(this.health);
            Console.ReadLine();
        }

    }
}
