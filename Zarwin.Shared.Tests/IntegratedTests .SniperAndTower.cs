using System.Linq;
using Xunit;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "final")]
        public void PurchasingSniper_Prerequesites()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 4))),
                new CityParameters(1, 10),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(0, actualOutput.Waves[0].Turns[1].Horde.Size);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
            Assert.Equal(11, actualOutput.Waves[0].Turns[1].Money);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingSniper_RemovesMoney()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 4))),
                new CityParameters(1, 10),
                new Order[]
                {
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Money);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingTower_RemovesMoney()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 4))),
                new CityParameters(1, 10),
                new Order[]
                {
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Money);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingSniper_ShootsDuringApproach()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 4))),
                new CityParameters(1, 10),
                new Order[]
                {
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[1].Turns[0].Horde.Size);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingSniper_IgnoresToughness()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 4))),
                new CityParameters(1, 10),
                new Order[]
                {
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[1].Turns[0].Horde.Size);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingSniper_IncreasesLevelNormally()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 4))),
                new CityParameters(1, 10),
                new Order[]
                {
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[1].Turns[1].Soldiers.Single().Level);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingSniper_DoesNotShootOutsideOfApproach()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 4))),
                new CityParameters(1, 10),
                new Order[]
                {
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1)
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[1].Turns[1].Horde.Size);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingTower_DelaysWave()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 4))),
                new CityParameters(1, 10),
                new Order[]
                {
                    new Order(0, 1, OrderType.ReinforceTower),
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(5, actualOutput.Waves[1].Turns[1].Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[1].Turns[1].Horde.Size);
        }

        [Fact]
        [Trait("grading", "v4")]
        public void PurchasingTower_IncreasesNumberOfSniperShots()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 4))),
                new CityParameters(1, 20),
                new Order[]
                {
                    new Order(0, 1, OrderType.ReinforceTower),
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1),
                },
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[1].Turns[0].Horde.Size);
            Assert.Equal(2, actualOutput.Waves[1].Turns[1].Horde.Size);
        }


    }
}
