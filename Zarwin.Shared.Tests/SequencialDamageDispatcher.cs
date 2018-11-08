using System.Collections.Generic;
using System.Linq;
using Zarwin.Shared.Contracts.Core;

namespace Zarwin.Shared.Tests
{
    public class SequencialDamageDispatcher : IDamageDispatcher
    {
        public IEnumerable<TSoldier> DispatchDamage<TSoldier>(int damage, IEnumerable<TSoldier> soldiers)
            where TSoldier : ISoldier
        {
            if (!soldiers.Any())
                return Enumerable.Empty<TSoldier>();

            var distribution = SplitDamage(damage, soldiers);
            foreach (var pair in distribution)
            {
                pair.Key.Hurt(pair.Value);
            }

            return distribution
                .Where(pair => pair.Value > 0)
                .Select(pair => pair.Key);
        }

        private IDictionary<TSoldier, int> SplitDamage<TSoldier>(int damage, IEnumerable<TSoldier> soldiers)
            where TSoldier : ISoldier
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
