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
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);

            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
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
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);

            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].Level);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
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
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);

            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].Level);
            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Horde.Size);

            Assert.Equal(3, actualOutput.Waves[0].Turns.Length);

            Assert.Single(actualOutput.Waves[0].Turns[2].Soldiers);
            Assert.Equal(5, actualOutput.Waves[0].Turns[2].Soldiers[0].HealthPoints);
            Assert.Equal(3, actualOutput.Waves[0].Turns[2].Soldiers[0].Level);
            Assert.Equal(0, actualOutput.Waves[0].Turns[2].Horde.Size);
        }
    }
}
