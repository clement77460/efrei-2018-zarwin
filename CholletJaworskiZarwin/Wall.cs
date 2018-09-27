using System;
using System.Collections.Generic;
using System.Text;

namespace zombieLand
{
    class Wall
    {
        private int health;

        public int Health => health;

        public Wall(int health) {
            this.health = health;
        }

        public void WeakenWall(int value)
        {
            this.health -= value;
            if(this.health < 0)
            {
                this.health = 0;
            }
        }
    }
}
