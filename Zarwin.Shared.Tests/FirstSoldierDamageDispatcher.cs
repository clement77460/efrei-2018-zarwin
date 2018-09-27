using System;
using System.Linq;
using System.Collections.Generic;
using Zarwin.Shared.Contracts.Core;

namespace Zarwin.Shared.Tests
{
    public class FirstSoldierDamageDispatcher : IDamageDispatcher
    {
        public void DispatchDamage(int damage, IEnumerable<ISoldier> soldiers)
        {
            while (damage > 0 && soldiers.Sum(soldier => soldier.HealthPoints) > 0)
            {
                var chosenSoldier = soldiers
                    .OrderBy(soldier => soldier.Id)
                    .First(soldier => soldier.HealthPoints > 0);
                int damageDealt = Math.Min(damage, chosenSoldier.HealthPoints);

                chosenSoldier.Hurt(damageDealt);
                damage -= damageDealt;
            }
        }
    }
}
