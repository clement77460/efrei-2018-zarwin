using System.Linq;
using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingShotgun_RemovesMoney()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(6, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithShotgun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(5, actualOutput.Waves[0].Turns[0].Money);
            // Instead of 5
        }

        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingShotgun_IncreasesKills()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(6, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithShotgun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Horde.Size);
            // Instead of 15
        }

        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingShotgun_IncreasesLevelGainIndirectly()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(6, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithShotgun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[0].Turns[1].Soldiers.Single().Level);
            // Instead of 2
        }

        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingShotgun_DoesNotWorkWithWall()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(1000, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithShotgun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(5, actualOutput.Waves[0].Turns[0].Money);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Horde.Size);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers.Single().Level);
        }

        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingShotgun_WorksWhenWallFalls()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(11, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithShotgun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[0].Turns[2].Horde.Size);
            Assert.Equal(4, actualOutput.Waves[0].Turns[2].Soldiers.Single().Level);
        }

        [Fact]
        [Trait("grading", "final")]
        public void PurchasingShotgun_AdvancedExample()
        {
            // Tests ensuring that the condition are correct in order
            // for the next shotgun tests to work
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(0, 9),
                new Order[]
                {
                    new Order(1, 0, OrderType.EquipWithShotgun, 4)
                },
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1),
                new SoldierParameters(4, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[0].Turns[1].Soldiers.Length);
            Assert.Equal(3, actualOutput.Waves[0].Turns[1].Soldiers.First().HealthPoints);
            Assert.Equal(3, actualOutput.Waves[0].Turns[1].Horde.Size);

            Assert.Equal(2, actualOutput.Waves[0].Turns[2].Soldiers.Length);
            Assert.Equal(6, actualOutput.Waves[0].Turns[2].Soldiers.First().HealthPoints);
            Assert.Equal(1, actualOutput.Waves[0].Turns[2].Horde.Size);

            Assert.Equal(2, actualOutput.Waves[1].Turns[0].Soldiers.Length);
            Assert.Equal(6, actualOutput.Waves[1].Turns[0].Soldiers.First().HealthPoints);
            Assert.Equal(6, actualOutput.Waves[1].Turns[0].Horde.Size);

            Assert.Equal(5, actualOutput.Waves[1].Turns[0].Money);
            // Instead of 15

            Assert.Single(actualOutput.Waves[1].Turns[1].Soldiers);

            Assert.Equal(4, actualOutput.Waves[1].Turns[1].Horde.Size);
            // Instead of 5
            Assert.Equal(5, actualOutput.Waves[1].Turns[1].Soldiers.Single().Level);
            // Instead of 4
        }

    }
}
