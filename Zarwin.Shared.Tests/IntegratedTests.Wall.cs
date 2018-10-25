using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Zarwin.Shared.Contracts.Input;

namespace Zarwin.Shared.Tests
{
    public partial class IntegratedTests
    {
        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_5Zombies_5HpWall_WallProtectsOneTurn()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(5),
                new CityParameters(5),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(0, actualOutput.Waves[0].Turns[1].WallHealthPoints);
            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Horde.Size);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
            Assert.Equal(2, actualOutput.Waves[0].Turns[1].Soldiers[0].Level);

            Assert.Equal(0, actualOutput.Waves[0].Turns[2].WallHealthPoints);
            Assert.Equal(3, actualOutput.Waves[0].Turns[2].Horde.Size);
            Assert.Equal(2, actualOutput.Waves[0].Turns[2].Soldiers[0].HealthPoints);
            Assert.Equal(3, actualOutput.Waves[0].Turns[2].Soldiers[0].Level);

            Assert.Equal(0, actualOutput.Waves[0].Turns[3].WallHealthPoints);
            Assert.Equal(3, actualOutput.Waves[0].Turns[3].Horde.Size);
            Assert.Empty(actualOutput.Waves[0].Turns[3].Soldiers);
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_5Zombies_1HpWall_ExtraDamageNotDealt()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(5),
                new CityParameters(1),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(0, actualOutput.Waves[0].Turns[1].WallHealthPoints);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
        }

        [Fact]
        [Trait("grading", "final")]
        public void OneSoldier_5Zombies_6HpWall_WallsProtectsTwoTurns()
        {
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(5),
                new CityParameters(6),
                new Order[0],
                new SoldierParameters(1, 1));

            var actualOutput = CreateSimulator().Run(input);

            Assert.Equal(1, actualOutput.Waves[0].Turns[1].WallHealthPoints);
            Assert.Equal(5, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
            Assert.Equal(0, actualOutput.Waves[0].Turns[2].WallHealthPoints);
            Assert.Equal(6, actualOutput.Waves[0].Turns[2].Soldiers[0].HealthPoints);
        }
    }
}
