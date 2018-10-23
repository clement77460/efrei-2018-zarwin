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
                null,
                new SoldierParameters(1, 1));


            GameEngine ge = new GameEngine(input);

            var actualOutput = ge.GameLoop();

            Assert.Single(actualOutput.Waves);

            Assert.Single(actualOutput.Waves[0].Turns[1].Soldiers);
            Assert.Equal(0, actualOutput.Waves[0].Turns[1].Horde.Size);
        }

    }
}
