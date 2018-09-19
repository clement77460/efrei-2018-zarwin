using System;
using System.Collections.Generic;
using System.Text;

namespace zombieLand
{
    class Wall
    {
        int health = 10;

        public void reduceHealth(int value)
        {
            this.health-=value;
            Console.Write("[MUR] il reste :");
            Console.Write(this.health);
            Console.ReadLine();
        }

        public int getHealth() => health;
        

    }
}
