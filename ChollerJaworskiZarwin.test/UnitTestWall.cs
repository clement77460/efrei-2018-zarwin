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
        public void initWallWith5HP()
        {
            Wall wall = new Wall(5);
            Assert.Equal(5, wall.Health);
        }

        [Fact]
        public void reducingWallHealthWith2()
        {
            Wall wall = new Wall(5);
            wall.WeakenWall(2);
            Assert.Equal(3, wall.Health);
        }


    }
}
