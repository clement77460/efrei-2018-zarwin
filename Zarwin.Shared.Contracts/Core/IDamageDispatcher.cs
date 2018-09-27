using System.Collections.Generic;

namespace Zarwin.Shared.Contracts.Core
{
    /// <summary>
    /// A service able to dispatch some amount of damages accross a list of soldiers.
    /// </summary>
    public interface IDamageDispatcher
    {
        /// <summary>
        /// Dispatch some amount of damages accross a list of soldiers. 
        /// For each soldier, up to one call to <see cref="ISoldier.Hurt(int)"/> is made with some amount of damage.
        /// No amount of damage sent to <see cref="ISoldier.Hurt(int)"/> may exceed <see cref="ISoldier.HealthPoints"/>.
        /// If the sum of <see cref="ISoldier.HealthPoints"/> is lower to the amount of damage to be dispatched, the extra amount is ignored.
        /// </summary>
        /// <param name="damage">The total amount of damage.</param>
        /// <param name="soldiers">A list of soldiers to deal damage to</param>
        void DispatchDamage(int damage, IEnumerable<ISoldier> soldiers);
    }
}
