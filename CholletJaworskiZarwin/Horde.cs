using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;

namespace zombieLand
{
    class Horde
    {

        private List<Walker> walkers;

        public Horde(int numberOfWalkers)
        {
            walkers = new List<Walker>();
            for (int i = 0; i < numberOfWalkers; i++)
            {
                walkers.Add(new Walker());
            }
        }

        public void AttackCity(City city, DamageDispatcher damageDispatcher)
        {
            foreach(Walker walker in this.walkers)
            {
                walker.AttackCity(city, damageDispatcher);
            }
        }

        public void KillWalker()
        {
            // Walkers are little entites, so we remove it from the list, 
            // not the same as the soldiers.
            if(this.walkers.Count > 0)
            {
                this.walkers.RemoveAt(0);
            }
        }

        public int GetNumberWalkersAlive()
        {
            return this.walkers.Count;
        }

        public void KillWalkers(int amountToKill)
        {
            for (int i = 0; i < amountToKill; ++i)
            {
                this.KillWalker();
            }
        }
    }
}
