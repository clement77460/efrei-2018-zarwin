using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v3")]
        public void OneSoldier_KillOneZombie_AndGainOneCoin()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(0, actualOutput.Waves[0].InitialState.Money);
            Assert.Equal(1, actualOutput.Waves[0].InitialState.Horde.Size);

            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Money);
            Assert.Equal(0, actualOutput.Waves[0].Turns[1].Horde.Size);
        }
    }
}
