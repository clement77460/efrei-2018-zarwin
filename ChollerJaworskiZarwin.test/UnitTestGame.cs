using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Tests;
using Zarwin.Shared.Contracts.Input.Orders;

namespace CholletJaworskiZarwin.test
{
    public class UnitTestGame
    {
        [Fact]
        public void TestStringReturnGamen_When1Soldiervs2Walkers()
        {
            var param = new Parameters(
               1,
               new FirstSoldierDamageDispatcher(),
               new HordeParameters(2),
               new CityParameters(10),
               new Order[0],
               new SoldierParameters(1, 1));

            Game game = new Game(param);
            game.SoldiersStats();
           
            game.Turn();
            Assert.True(game.IsFinished());
            Assert.Equal("Soldiers are 1 left. "+ "0 walker(s) are attacking. ", game.ToString());
            

        }
        [Fact]
        public void FinishingTheGameQuickly_TestingBreakTurn()
        {
            var param = new Parameters(
              1,
              new FirstSoldierDamageDispatcher(),
              new HordeParameters(1),
              new CityParameters(10),
              new Order[0],
              new SoldierParameters(1, 1));

            Game game = new Game(param, false);

            Assert.True(game.IsFinished());
        }
        [Fact]
        public void SendingTwoHordes_WhenTestMode()
        {
            var input = new Parameters(
               1,
               new FirstSoldierDamageDispatcher(),
               new HordeParameters(2),
               new CityParameters(0),
               new Order[0],
               new SoldierParameters(1, 1));

            Game game = new Game(input);


            game.Turn();
            game.Turn();
            Assert.True(game.IsFinished());
        }
    }
}
