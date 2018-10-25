using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CholletJaworskiZarwin;

namespace CholletJaworskiZarwin.test
{
    public class UnitTestCity
    {
        [Fact]
        public void DefendFromHorde_KillingOneWalker_NoSoldiersDying()
        {
            Horde horde = new Horde(5);
            City city = new City(1, 5);
            city.DefendFromHorde(horde, 1);
            //getting stats useless for unit test .........
            city.SoldiersStats();
            Assert.Equal(4,horde.GetNumberWalkersAlive() );
            Assert.Equal(1, city.NumberSoldiersAlive);
            Assert.False(city.AreAllSoldiersDead());
        }
        [Fact]
        public void WalkersKilledEverySoldiers()
        {
            Horde horde = new Horde(8);
            City city = new City(1, 5);
            city.HurtSoldiers(8, new DamageDispatcher());
            Assert.True(city.AreAllSoldiersDead());
        }
    }
}
