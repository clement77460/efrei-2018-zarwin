using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CholletJaworskiZarwin;


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
            Game game = new Game(10, 5, 1, 2);


            game.Turn();
            game.Turn();
            Assert.True(game.IsFinished());
        }
    }
}
