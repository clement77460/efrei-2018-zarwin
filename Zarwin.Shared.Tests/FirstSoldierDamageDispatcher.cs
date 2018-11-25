using System;
using System.Linq;
using System.Collections.Generic;
using Zarwin.Shared.Contracts.Core;

namespace Zarwin.Shared.Tests
{
    public class FirstSoldierDamageDispatcher : IDamageDispatcher
    {
        public IEnumerable<TSoldier> DispatchDamage<TSoldier>(int damage, IEnumerable<TSoldier> soldiers)
            where TSoldier : ISoldier
        {
            var damagedSoldiers = new HashSet<TSoldier>();

            while (damage > 0 && soldiers.Sum(soldier => soldier.HealthPoints) > 0)
            {
                var chosenSoldier = soldiers
                    .OrderBy(soldier => soldier.Id)
                    .First(soldier => soldier.HealthPoints > 0);
                int damageDealt = Math.Min(damage, chosenSoldier.HealthPoints);

                chosenSoldier.Hurt(damageDealt);
                damagedSoldiers.Add(chosenSoldier);
                damage -= damageDealt;
            }

            return damagedSoldiers;
        }
    }
}
