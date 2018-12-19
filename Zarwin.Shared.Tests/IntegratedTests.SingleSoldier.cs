using System;
using System.Diagnostics;
using System.Linq;
using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v2")]
        public void OneSoldier_OneZombie_SoldierStompsZombie()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);

            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(0, actualOutput.Waves[0].Turns[1].Horde.Size);
            Assert.Equal(2, actualOutput.Waves[0].Turns.Length);
        }

        [Fact]
        [Trait("grading", "v2")]
        public void OneSoldier_OneZombie_SoldierGainsOneLevel()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);

            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].Level);
            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
        }

        [Fact]
        [Trait("grading", "v2")]
        public void OneSoldier_TwoZombies_NoWall_SoldierHurtedOnce_ThenWin()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(2),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);

            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(3, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].Level);
            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Horde.Size);

            Assert.Equal(3, actualOutput.Waves[0].Turns.Length);

            Assert.Single(actualOutput.Waves[0].Turns[2].Soldiers);
            Assert.Equal(3, actualOutput.Waves[0].Turns[2].Soldiers[0].HealthPoints);
            Assert.Equal(3, actualOutput.Waves[0].Turns[2].Soldiers[0].Level);
            Assert.Equal(0, actualOutput.Waves[0].Turns[2].Horde.Size);
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_ThreeZombies_NoWall_SoldierLooses()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(3),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);

            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Horde.Size);
            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].Level);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);

            Assert.Equal(3, actualOutput.Waves[0].Turns.Length);

            Assert.Equal(2, actualOutput.Waves[0].Turns[2].Horde.Size);
            Assert.Empty(actualOutput.Waves[0].Turns[2].Soldiers);
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_FourZombies_NoWall_SoldierStomped()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);

            Assert.Empty(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Horde.Size);

            Assert.Equal(2, actualOutput.Waves[0].Turns.Length);
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_Level9_IncreasesHisLevelTwice()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(2),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 9));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(11, actualOutput.Waves[0].Turns[2].Soldiers[0].Level);
            Assert.Equal(11, actualOutput.Waves[0].Turns[2].Soldiers[0].HealthPoints);
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_Level9_LevelIncreasesHisDamage()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(2),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 9));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[0].Turns.Length);
            Assert.Equal(2, actualOutput.Waves[1].Turns.Length);
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_ManyOneZombieWaves_SoldiersIncreasesHisLevel()
        {
            var stopWatch = Stopwatch.StartNew();

            var input = new Parameters(
                11,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(11, actualOutput.Waves.Length);
            Assert.Equal(2, actualOutput.Waves.Select(wave => wave.Turns.Length).Distinct().Single());

            Assert.Equal(1, actualOutput.Waves[0].InitialState.Soldiers[0].Level);
            Assert.Equal(2, actualOutput.Waves[1].InitialState.Soldiers[0].Level);
            Assert.Equal(3, actualOutput.Waves[2].InitialState.Soldiers[0].Level);
            Assert.Equal(4, actualOutput.Waves[3].InitialState.Soldiers[0].Level);
            Assert.Equal(5, actualOutput.Waves[4].InitialState.Soldiers[0].Level);
            Assert.Equal(6, actualOutput.Waves[5].InitialState.Soldiers[0].Level);
            Assert.Equal(7, actualOutput.Waves[6].InitialState.Soldiers[0].Level);
            Assert.Equal(8, actualOutput.Waves[7].InitialState.Soldiers[0].Level);
            Assert.Equal(9, actualOutput.Waves[8].InitialState.Soldiers[0].Level);
            Assert.Equal(10, actualOutput.Waves[9].InitialState.Soldiers[0].Level);
            Assert.Equal(11, actualOutput.Waves[10].InitialState.Soldiers[0].Level);

            Assert.Equal(4, actualOutput.Waves[0].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[1].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[2].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[3].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[4].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[5].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[6].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[7].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[8].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[9].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(4, actualOutput.Waves[10].InitialState.Soldiers[0].HealthPoints);

            Assert.True(stopWatch.Elapsed < TimeSpan.FromSeconds(0.5));
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_ManyTwoZombiesWaves_LevelGivesMoreDamage()
        {
            var input = new Parameters(
                11,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(2, actualOutput.Waves[10].Turns.Length);
            Assert.Equal(1, actualOutput.Waves[10].InitialState.Horde.Size);

            Assert.Equal(12, actualOutput.Waves[10].Turns[1].Soldiers[0].Level);
            Assert.Equal(4, actualOutput.Waves[10].Turns[1].Soldiers[0].HealthPoints);
        }
    }
}
