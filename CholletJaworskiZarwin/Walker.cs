using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;

namespace CholletJaworskiZarwin
{
    class Walker
    {
        // ID counter which increments each new Walker
        static int walkerCounterId = 0;

        private readonly int idWalker;

        public Walker()
        {
            this.idWalker = walkerCounterId;
            Walker.walkerCounterId++;
        }

        public void AttackCity(City city, DamageDispatcher damageDispatcher)
        {
            // If the wall still up, the walker attacks it
            if(city.Wall.Health > 0)
            {
                city.Wall.WeakenWall(1);
            } 
            // If the wall collapsed, the walker attack the soldiers
            else {
                city.HurtSoldiers(1, damageDispatcher);
            }
        }

        public override String ToString()
        {
            return "Je suis le zombie numero : "+ idWalker;
        }

    }
}
