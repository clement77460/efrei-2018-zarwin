using System;
using System.Collections.Generic;
using System.Linq;
using Zarwin.Shared.Contracts.Core;

namespace CholletJaworskiZarwin
{
    public class DamageDispatcher : IDamageDispatcher
    {
        public IEnumerable<TSoldier> DispatchDamage<TSoldier>(int damage, IEnumerable<TSoldier> soldiers) where TSoldier : ISoldier
        {
            List<TSoldier> hurtSoldiers = new List<TSoldier>();
            while (damage > 0 && soldiers.Sum(soldier => soldier.HealthPoints) > 0)
            {
                // Get a random number
                Random random = new Random();
                int randomNumber = random.Next(0, soldiers.Count());

                var chosenSoldier = soldiers.ElementAt(randomNumber);
                int damageDealt = Math.Min(damage, chosenSoldier.HealthPoints);

                chosenSoldier.Hurt(damageDealt);
                damage -= damageDealt;
                hurtSoldiers.Add(chosenSoldier);
            }
            return hurtSoldiers;
        }
    }
}
