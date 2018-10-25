using System.Linq;
using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v3")]
        public void Recruiting_ReducesMoney()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0, 9),
                new Order[]
                {
                    new Order(1, 0, OrderType.RecruitSoldier)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(0, actualOutput.Waves[1].Turns[0].Money);
        }

        [Fact]
        [Trait("grading", "v3")]
        public void Recruiting_AddNewSoldier()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0, 9),
                new Order[]
                {
                    new Order(1, 0, OrderType.RecruitSoldier)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves[0].Turns.Last().Soldiers);

            Assert.Equal(2, actualOutput.Waves[1].Turns[0].Soldiers.Length);
            Assert.NotNull(actualOutput.Waves[1].Turns[0].SingleOrDefaultSoldier(2));
        }
    }
}
