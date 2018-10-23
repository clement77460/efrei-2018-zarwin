using System.Linq;
using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "v3")]
        public void Priority_ToughFatty_BeforeNormalFatty()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(new WaveHordeParameters[]
                {
                    new WaveHordeParameters(
                        new ZombieParameter(ZombieType.Fatty, ZombieTrait.Normal, 1),
                        new ZombieParameter(ZombieType.Fatty, ZombieTrait.Tough, 1))
                }),
                new CityParameters(0, 0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Horde.Size);
        }

        [Fact]
        [Trait("grading", "v3")]
        public void Priority_FattyNormal_BeforeToughStalker()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(new WaveHordeParameters[]
                {
                    new WaveHordeParameters(
                        new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 1),
                        new ZombieParameter(ZombieType.Fatty, ZombieTrait.Normal, 1))
                }),
                new CityParameters(0, 0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(1, actualOutput.Waves[0].Turns[1].Horde.Size);
        }

        [Fact]
        [Trait("grading", "v3")]
        public void Priority_ToughStalker_BeforeStalkerNormal()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(new WaveHordeParameters[]
                {
                    new WaveHordeParameters(
                        new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 1),
                        new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1))
                }),
                new CityParameters(0, 0),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Horde.Size);
        }
    }
}
