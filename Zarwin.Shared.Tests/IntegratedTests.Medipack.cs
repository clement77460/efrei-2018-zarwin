using System.Linq;
using Xunit;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingMedipack_RemovesMoney()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(3),
                new CityParameters(0, 2),
                new Order[]
                {
                    new Medipack(0, 1, 1, 2)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Money);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingMedipack_IncreasesLife()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(3),
                new CityParameters(0, 2),
                new Order[]
                {
                    new Medipack(0, 1, 1, 2)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingMedipack_NotPurchasedIfNotEnoughMoneyAtStartOfTurn()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(3),
                new CityParameters(0, 1),
                new Order[]
                {
                    new Medipack(0, 1, 1, 2)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Money);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingMedipack_LifeCappedByMaximum()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(3),
                new CityParameters(0, 5),
                new Order[]
                {
                    new Medipack(0, 1, 1, 5)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].Level);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
        }

        [Fact]
        [Trait("grading", "final")]
        public void PurchasingMedipack_DoesNotSaveFromDeath()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(0, 5),
                new Order[]
                {
                    new Medipack(0, 1, 1, 5)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Empty(actualOutput.Waves[0].Turns[1].Soldiers);
        }
    }
}
