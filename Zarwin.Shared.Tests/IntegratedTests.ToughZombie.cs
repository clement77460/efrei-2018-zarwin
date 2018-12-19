using System.Linq;
using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v3")]
        public void Tough_ResistsOneShot()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(new WaveHordeParameters[]
                {
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 1))
                }),
                new CityParameters(0, 0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Horde.Size);
        }

        [Fact]
        [Trait("grading", "v3")]
        public void Tough_ResistsMultipleOneShot()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(new WaveHordeParameters[]
                {
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 1))
                }),
                new CityParameters(0, 0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(1, actualOutput.Waves[0].Turns[2].Horde.Size);
            Assert.Equal(1, actualOutput.Waves[0].Turns[3].Horde.Size);
            Assert.Equal(1, actualOutput.Waves[0].Turns[4].Horde.Size);
        }

        [Fact]
        [Trait("grading", "v3")]
        public void Tough_KilledByTwoSimultaneousShots()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(new WaveHordeParameters[]
                {
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 1))
                }),
                new CityParameters(0, 0),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(0, actualOutput.Waves[0].Turns[1].Horde.Size);
        }

        [Fact]
        [Trait("grading", "final")]
        public void Tough_SecondShotIncreasesLevel()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(new WaveHordeParameters[]
                {
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 1))
                }),
                new CityParameters(0, 0),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers.Last().Level);
            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Soldiers.First().Level);
        }

        [Fact]
        [Trait("grading", "final")]
        public void Tough_AdvancedScenario()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(new WaveHordeParameters[]
                {
                    new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 5))
                }),
                new CityParameters(0, 0), // 2
                new Order[0],
                new SoldierParameters(1, 1), // 0
                new SoldierParameters(2, 1), // 0
                new SoldierParameters(3, 1), // 0
                new SoldierParameters(4, 1));// 5

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Horde.Size);
            Assert.Equal(3, actualOutput.Waves[0].Turns[1].Soldiers.Length);
            Assert.Equal(3, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers[1].HealthPoints); // Level increase
            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Soldiers[2].HealthPoints);

            Assert.Equal(3, actualOutput.Waves[0].Turns[2].Horde.Size);
            Assert.Equal(2, actualOutput.Waves[0].Turns[2].Soldiers.Length);
            Assert.Equal(4, actualOutput.Waves[0].Turns[2].Soldiers[0].HealthPoints);
            Assert.Equal(5, actualOutput.Waves[0].Turns[2].Soldiers[1].HealthPoints); // Level increase

            Assert.Equal(2, actualOutput.Waves[0].Turns[3].Horde.Size);
            Assert.Equal(2, actualOutput.Waves[0].Turns[3].Soldiers.Length);
            Assert.Equal(1, actualOutput.Waves[0].Turns[3].Soldiers[0].HealthPoints);
            Assert.Equal(6, actualOutput.Waves[0].Turns[3].Soldiers[1].HealthPoints); // Level increase

            Assert.Equal(2, actualOutput.Waves[0].Turns[4].Horde.Size);
            Assert.Single(actualOutput.Waves[0].Turns[4].Soldiers);
            Assert.Equal(5, actualOutput.Waves[0].Turns[4].Soldiers[0].HealthPoints);

            Assert.Equal(2, actualOutput.Waves[0].Turns[5].Horde.Size);
            Assert.Single(actualOutput.Waves[0].Turns[5].Soldiers);
            Assert.Equal(3, actualOutput.Waves[0].Turns[5].Soldiers[0].HealthPoints);

            Assert.Equal(2, actualOutput.Waves[0].Turns[6].Horde.Size);
            Assert.Single(actualOutput.Waves[0].Turns[6].Soldiers);
            Assert.Equal(1, actualOutput.Waves[0].Turns[6].Soldiers[0].HealthPoints);

            Assert.Equal(2, actualOutput.Waves[0].Turns[7].Horde.Size);
            Assert.Empty(actualOutput.Waves[0].Turns[7].Soldiers);
        }
    }
}
