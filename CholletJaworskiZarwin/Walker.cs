using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts.Core;
using Zarwin.Shared.Contracts.Input;

namespace CholletJaworskiZarwin
{
    public class Walker
    {
        // ID counter which increments each new Walker
        public static int walkerCounterId = 0;

        // Type
        internal ZombieType Type { get; set; }

        // Powers
        internal ZombieTrait Trait { get; set; }

        // Last turn the walker have been damaged
        internal int DamageTurn { get; set; }

        // Damage taken during DamageTurn
        internal int DamageTaken { get; set; }

        public int Id { get; private set; }

        public Walker()
        {
            this.Id = walkerCounterId;
            Walker.walkerCounterId++;
            this.Trait = ZombieTrait.Normal;
            this.Type = ZombieType.Stalker;
        }

        public Walker(ZombieParameter parameter)
        {
            this.Id = walkerCounterId;
            Walker.walkerCounterId++;
            this.Type = parameter.Type;
            this.Trait = parameter.Trait;
        }

        public void AttackCity(City city, IDamageDispatcher damageDispatcher)
        {
            // If the wall still up, the walker attacks it
            if (city.Wall.Health > 0)
            {
                city.Wall.WeakenWall(1);
            }
            // If the wall collapsed, the walker attack the soldiers
            else
            {
                city.HurtSoldiers(1, damageDispatcher);
            }
        }

        public void Hurt(int turn, int damages)
        {
            if(this.DamageTurn != turn)
            {
                this.DamageTaken = damages;
            }
            else
            {
                this.DamageTaken += damages;
            }
            this.DamageTurn = turn;
        }

        public override String ToString()
        {
            return "Je suis le zombie numero : " + Id;
        }

    }
}
