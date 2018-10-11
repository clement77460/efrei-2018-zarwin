using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CholletJaworskiZarwin;


namespace ChollerJaworskiZarwin.test
{
    public class UnitTestGame
    {
        [Fact]
        public void testStringReturnGameIsNotFinishedAfterOneTurn()
        {
            Game game = new Game(10, 5, 10, 10);
            game.SoldiersStats();
           
            game.Turn();
            Assert.False(game.IsFinished());
            game.ToString();
            Assert.NotEmpty(game.Message);
            

        }
        [Fact]
        public void WallIsAt6HPBecause4WalkersAreLeft()
        {
            Game game = new Game(10, 5, 9, 1);
            

            game.Turn();
            
            Assert.Equal(6, game.WallHealth);
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
