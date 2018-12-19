using System;
using System.Collections.Generic;
using System.Text;

namespace CholletJaworskiZarwin
{
    public class Wall
    {

        public int Health { get; private set; }

        public Wall(int health) {
            this.Health = health;
        }

        public void WeakenWall(int value)
        {
            this.Health -= value;
            if(this.Health < 0)
            {
                this.Health = 0;
            }
        }

    }
}
