using System;
using System.Collections.Generic;
using System.Text;

namespace zombieLand
{
    class Wave
    {

        List<Walker> walkers;

        public Wave(int numberOfWalkers)
        {
            walkers = new List<Walker>();
            this.createWalkers(numberOfWalkers);
        }

        private void createWalkers(int numberOfWalkers)
        {
            for(int i=0;i< numberOfWalkers; i++)
            {
                walkers.Add(new Walker());
            }
        }

        public List<Walker> getWalkers()
        {
            return this.walkers;
        }

        public void killWalker()
        {
            this.walkers.RemoveAt(0); //mettre de l'aléatoire
            Console.Write("[ZOMBIE] il resre :");
            Console.Write(this.walkers.Count);
            Console.ReadLine();
        }

    }
}
