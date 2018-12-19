using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;
using Xunit;

namespace CholletJaworskiZarwin.test
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
            horde.DoWalkerDamage(1);
            Assert.Equal(7, horde.GetNumberWalkersAlive());
        }

        [Fact]
        public void Creating8Walkers_KillingTwo()
        {
            Horde horde = new Horde(8);
            horde.DoDamages(2, 1);
            Assert.Equal(6, horde.GetNumberWalkersAlive());
        }

    }
}
