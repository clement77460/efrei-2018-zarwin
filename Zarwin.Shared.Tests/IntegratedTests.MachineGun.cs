using System.Linq;
using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingMachineGun_RemovesMoney()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(1000, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithMachineGun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(5, actualOutput.Waves[0].Turns[0].Money);
            // Instead of 5
        }

        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingMachineGun_IncreasesKills()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(1000, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithMachineGun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Horde.Size);
            // Instead of 15
        }

        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingMachineGun_IncreasesLevelGainIndirectly()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(1000, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithMachineGun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers.Single().Level);
            // Instead of 2
        }

        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingMachineGun_DoesNotWorkWithoutWall()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(6, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithMachineGun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(5, actualOutput.Waves[0].Turns[0].Money);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Horde.Size);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers.Single().Level);
        }

        [Fact]
        [Trait("grading", "v3")]
        public void PurchasingMachineGun_StopWorkingWhenWallFalls()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(6),
                new CityParameters(7, 15),
                new Order[]
                {
                    new Order(0, 0, OrderType.EquipWithMachineGun, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(1, actualOutput.Waves[0].Turns[2].Horde.Size);
            Assert.Equal(6, actualOutput.Waves[0].Turns[2].Soldiers.Single().Level);
        }
    }
}
