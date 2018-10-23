using System.Linq;
using Zarwin.Shared.Contracts.Output;

namespace Zarwin.Shared.Tests
{
    public static class Extensions
    {
        public static SoldierState SingleOrDefaultSoldier(this TurnResult result, int id)
        {
            return result.Soldiers.SingleOrDefault(state => state.Id == id);
        }
    }
}
