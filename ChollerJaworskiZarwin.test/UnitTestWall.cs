using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;
using Xunit;


namespace ChollerJaworskiZarwin.test
{
    public class UnitTestWall
    {
        [Fact]
        public void InitWallWith5HP()
        {
            Wall wall = new Wall(5);
            Assert.Equal(5, wall.Health);
        }

        [Fact]
        public void ReducingWallHealthWith2()
        {
            Wall wall = new Wall(5);
            wall.WeakenWall(2);
            Assert.Equal(3, wall.Health);
        }

        [Fact]
        public void OverkillingWall()
        {
            Wall wall = new Wall(5);
            wall.WeakenWall(10);
            Assert.Equal(0, wall.Health);
        }


    }
}
