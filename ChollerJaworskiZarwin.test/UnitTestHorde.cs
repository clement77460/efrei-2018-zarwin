using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;
using Xunit;

namespace ChollerJaworskiZarwin.test
{
    public class UnitTestHorde
    {
        [Fact]
        public void Creating8Walkers()
        {
            Horde horde = new Horde(8);
            Assert.Equal(8, horde.GetNumberWalkersAlive());
        }

        [Fact]
        public void Creating8Walkers_KillingOne()
        {
            Horde horde = new Horde(8);
            horde.KillWalker();
            Assert.Equal(7, horde.GetNumberWalkersAlive());
        }

        [Fact]
        public void Creating8Walkers_KillingTwo()
        {
            Horde horde = new Horde(8);
            horde.KillWalkers(2);
            Assert.Equal(6, horde.GetNumberWalkersAlive());
        }

    }
}
