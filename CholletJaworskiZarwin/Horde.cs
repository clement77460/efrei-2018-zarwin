using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts.Core;
using Zarwin.Shared.Contracts.Input;

namespace CholletJaworskiZarwin
{
    public class Horde
    {
        // Walkers list
        private List<Walker> walkers;

        // Walker list accessors by types and powers
        private List<Walker> GetWalkers(ZombieType type, ZombieTrait trait)
        {
            return this.walkers.Where(w => (w.Type == type) && (w.Trait == trait)).ToList();
        }

        // Damage utilities
        private Walker lastHitWalker;


        public Horde(int numberOfWalkers)
        {
            walkers = new List<Walker>();
            for (int i = 0; i < numberOfWalkers; i++)
            {
                walkers.Add(new Walker());
            }
            this.lastHitWalker = null;
        }

        public Horde(WaveHordeParameters parameters)
        {
            walkers = new List<Walker>();
            foreach (ZombieParameter walkerParams in parameters.ZombieTypes)
            {
                for (int i = 0; i < walkerParams.Count; ++i)
                {
                    this.walkers.Add(new Walker(walkerParams));
                }
            }
            this.lastHitWalker = null;
        }

        public void AttackCity(City city, IDamageDispatcher damageDispatcher)
        {
            int damage = 0;
            foreach (Walker walker in this.walkers)
            {
               damage++;
            }
            city.GetAttacked(damage,damageDispatcher);
        }

        public bool DoWalkerDamage(int turn)
        {
            // For each type (we saw that it was sorted by priority in the enum contract).
            foreach (ZombieType type in (ZombieType[])Enum.GetValues(typeof(ZombieType)))
            {
                // For each trait (same, sorten by priority in the enum declaration).
                foreach (ZombieTrait trait in (ZombieTrait[])Enum.GetValues(typeof(ZombieTrait)))
                {
                    // If a walker exists with the current type and with the current trait
                    if (this.GetWalkers(type, trait).Any())
                    {
                        // We take the first one (lowest id) of them as target
                        Walker target = this.GetWalkers(type, trait).First();

                        // If it is Tough, he has to be hit twice to be removed
                        if (trait == ZombieTrait.Tough)
                        {
                            target.Hurt(turn, 1);
                            if(target.DamageTaken >= 2 && target.DamageTurn == turn)
                            {
                                this.walkers.Remove(target);
                                return true;
                            }
                            return false;
                        }
                        // Else, it is directly removed
                        else
                        {
                            this.walkers.Remove(target);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public int GetNumberWalkersAlive()
        {
            return this.walkers.Count;
        }

        public int DoDamages(int damages, int turn)
        {
            int killedWalkers = 0;
            for (int i = 0; i < damages; ++i)
            {
                if (this.DoWalkerDamage(turn))
                {
                    killedWalkers++;
                }
            }
            this.lastHitWalker = null;
            return killedWalkers;
        }

    }
}
