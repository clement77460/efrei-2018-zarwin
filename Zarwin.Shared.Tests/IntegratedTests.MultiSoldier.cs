using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{

    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_FirstSoldierDispatcher_FirstEndSecondRoundWith2Hp()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[0].Turns.Length);
            Assert.Equal(3, actualOutput.Waves[1].Turns.Length);
            Assert.Equal(2, actualOutput.Waves[1].Turns[2].SingleOrDefaultSoldier(1).HealthPoints);
        }

        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_FirstSoldierDispatcher_OthersUntouched()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(3, actualOutput.Waves[1].Turns[2].SingleOrDefaultSoldier(2).Level);
            Assert.Equal(3, actualOutput.Waves[1].Turns[2].SingleOrDefaultSoldier(3).Level);
            Assert.Equal(6, actualOutput.Waves[1].Turns[2].SingleOrDefaultSoldier(2).HealthPoints);
            Assert.Equal(6, actualOutput.Waves[1].Turns[2].SingleOrDefaultSoldier(3).HealthPoints);
        }

        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_FirstSoldierDispatcher_FirstFallsAtThirdWave()
        {
            var input = new Parameters(
                3,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Null(actualOutput.Waves[2].Turns[1].SingleOrDefaultSoldier(1));
        }

        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_FirstSoldierDispatcher_SecondGetsExtraDamage()
        {
            var input = new Parameters(
                3,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(4, actualOutput.Waves[2].Turns[1].SingleOrDefaultSoldier(2).Level);
            Assert.Equal(5, actualOutput.Waves[2].Turns[1].SingleOrDefaultSoldier(2).HealthPoints);
        }

        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_FirstSoldierDispatcher_ThirdGetsNone()
        {
            var input = new Parameters(
                3,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(4, actualOutput.Waves[2].Turns[1].SingleOrDefaultSoldier(3).Level);
            Assert.Equal(7, actualOutput.Waves[2].Turns[1].SingleOrDefaultSoldier(3).HealthPoints);
        }

        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_FirstSoldierDispatcher_LoosesAtThirdWave()
        {
            var input = new Parameters(
                5,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Empty(actualOutput.Waves[4].Turns[2].Soldiers);
        }

        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_SequencialDispatcher_HitsUniform()
        {
            var input = new Parameters(
                8,
                new SequencialDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(5, actualOutput.Waves[1].Turns[0].SingleOrDefaultSoldier(1).HealthPoints);
            Assert.Equal(5, actualOutput.Waves[1].Turns[0].SingleOrDefaultSoldier(2).HealthPoints);
            Assert.Equal(5, actualOutput.Waves[1].Turns[0].SingleOrDefaultSoldier(3).HealthPoints);

            Assert.Equal(4, actualOutput.Waves[1].Turns[1].SingleOrDefaultSoldier(1).HealthPoints);
            Assert.Equal(5, actualOutput.Waves[1].Turns[1].SingleOrDefaultSoldier(2).HealthPoints);
            Assert.Equal(5, actualOutput.Waves[1].Turns[1].SingleOrDefaultSoldier(3).HealthPoints);

            Assert.Equal(4, actualOutput.Waves[2].Turns[0].SingleOrDefaultSoldier(1).HealthPoints);
            Assert.Equal(5, actualOutput.Waves[2].Turns[0].SingleOrDefaultSoldier(2).HealthPoints);
            Assert.Equal(5, actualOutput.Waves[2].Turns[0].SingleOrDefaultSoldier(3).HealthPoints);

            Assert.Equal(3, actualOutput.Waves[2].Turns[1].SingleOrDefaultSoldier(1).HealthPoints);
            Assert.Equal(5, actualOutput.Waves[2].Turns[1].SingleOrDefaultSoldier(2).HealthPoints);
            Assert.Equal(5, actualOutput.Waves[2].Turns[1].SingleOrDefaultSoldier(3).HealthPoints);
        }

        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_SequencialDispatcher_FirstFallsFifthWave()
        {
            var input = new Parameters(
                8,
                new SequencialDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Null(actualOutput.Waves[4].Turns[1].SingleOrDefaultSoldier(1));
        }

        [Fact]
        [Trait("grading", "final")]
        public void ThreeSoldier_FourZombies_4HpWall_SequencialDispatcher_SecondAndThirdImmortalAtEighthWave()
        {
            var input = new Parameters(
                8,
                new SequencialDamageDispatcher(),
                new HordeParameters(4),
                new CityParameters(4),
                new Order[0],
                new SoldierParameters(1, 1),
                new SoldierParameters(2, 1),
                new SoldierParameters(3, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(13, actualOutput.Waves[7].Turns[1].SingleOrDefaultSoldier(2).Level);
            Assert.Equal(13, actualOutput.Waves[7].Turns[1].SingleOrDefaultSoldier(3).Level);
            Assert.Equal(0, actualOutput.Waves[7].Turns[1].Horde.Size);

            Assert.Equal(
                actualOutput.Waves[7].Turns[0].SingleOrDefaultSoldier(2).HealthPoints,
                actualOutput.Waves[7].Turns[1].SingleOrDefaultSoldier(2).HealthPoints);
            Assert.Equal(
                actualOutput.Waves[7].Turns[0].SingleOrDefaultSoldier(3).HealthPoints,
                actualOutput.Waves[7].Turns[1].SingleOrDefaultSoldier(3).HealthPoints);
        }
    }
}
