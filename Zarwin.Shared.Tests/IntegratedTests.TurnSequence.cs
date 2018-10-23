using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v2")]
        public void NoSoldier_InstantEnd()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0),
                new Order[0]);

            var actualOutput = CreateSimulator().Run(input);

            Assert.Single(actualOutput.Waves);
            Assert.Empty(actualOutput.Waves[0].Turns);
        }

        [Fact]
        [Trait("grading", "v2")]
        public void OneSoldier_ExpectedInitialState()
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

            Assert.Single(actualOutput.Waves[0].InitialState.Soldiers);
            Assert.Equal(4, actualOutput.Waves[0].InitialState.Soldiers[0].HealthPoints);
            Assert.Equal(1, actualOutput.Waves[0].InitialState.Horde.Size);
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_FirstApproachTurnWithNoChange()
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

            Assert.Equal(
                actualOutput.Waves[0].InitialState, 
                actualOutput.Waves[0].Turns[0]);
        }
    }
}
