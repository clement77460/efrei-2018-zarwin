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
        public void TestStringReturnGameIsNotFinishedAfterOneTurn()
        {
            Game game = new Game(10, 5, 10, 10);
            game.SoldiersStats();
           
            game.Turn();
            Assert.False(game.IsFinished());
            game.ToString();
            Assert.NotEmpty(game.Message);
            

        }
        [Fact]
        public void WallIsAt1HPBecause9WalkersAreHitting()
        {
            Game game = new Game(10, 5, 9, 1);
            

            game.Turn();
            
            Assert.Equal(1, game.WallHealth);
        }
        [Fact]
        public void FinishingTheGameQuickly()
        {
            Game game = new Game(10, 5, 1, 1);


            game.Turn();

            Assert.True(game.IsFinished());
        }
        [Fact]
        public void SendingTwoHordes()
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
