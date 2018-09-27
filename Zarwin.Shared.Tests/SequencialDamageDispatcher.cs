using System.Collections.Generic;
using System.Linq;
using Zarwin.Shared.Contracts.Core;

namespace Zarwin.Shared.Tests
{
    public class SequencialDamageDispatcher : IDamageDispatcher
    {
        public void DispatchDamage(int damage, IEnumerable<ISoldier> soldiers)
        {
            if (!soldiers.Any())
                return;

            foreach (var pair in SplitDamage(damage, soldiers))
            {
                pair.Key.Hurt(pair.Value);
            }
        }

        private IDictionary<ISoldier, int> SplitDamage(int damage, IEnumerable<ISoldier> soldiers)
        {
            var soldiersArray = soldiers.OrderBy(soldier => soldier.Id).ToArray();
            var result = soldiers.ToDictionary(
                s => s,
                s => 0);

            for (int i = 0; i < damage; i++)
            {
                var soldier = soldiersArray[i % soldiersArray.Length];
                result[soldier]++;
            }

            return result;
        }
    }

}
