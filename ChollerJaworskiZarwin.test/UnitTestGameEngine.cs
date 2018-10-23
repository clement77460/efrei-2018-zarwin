using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Tests;

namespace ChollerJaworskiZarwin.test
{
    public class UnitTestGameEngine
    {

        [Fact]
        public void OneSoldier_OneZombie_SoldierStompsZombie()
        {
            
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0),
                new SoldierParameters(1, 1));


            GameEngine ge = new GameEngine(input);

            var actualOutput = ge.GameLoop();

            Assert.Single(actualOutput.Waves);

            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(0, actualOutput.Waves[0].Turns[1].Horde.Size);
        }

        [Fact]
        public void hordeDoingDamage()
        {
            
            var input = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(2),
                new CityParameters(0),
                new SoldierParameters(1, 1));

            GameEngine gameEngine = new GameEngine(input);
            var actualOutput = gameEngine.GameLoop();

            Assert.Single(actualOutput.Waves);
            Assert.Equal(4, actualOutput.Waves[0].Turns[1].Soldiers[0].HealthPoints);
        }
        [Fact]
        public void noSoldiers()
        {
            var input = new Parameters(
                2,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0));


            GameEngine gameEngine = new GameEngine(input);
            var actualOutput = gameEngine.GameLoop();

            Assert.Single(actualOutput.Waves);
            Assert.Empty(actualOutput.Waves[0].Turns);
        }
    }
}
