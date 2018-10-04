using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CholletJaworskiZarwin;

namespace ChollerJaworskiZarwin.test
{
    public class UnitTestWalker
    {


        [Fact]
        public void walkerAttackingCity_redirectingOnWall()
        {
            Walker w = new Walker();
            City city = new City(1, 5);
            w.AttackCity(city,new DamageDispatcher());
            Assert.Equal(4, city.GetWall().Health);
           
        }

        [Fact]
        public void walkerAttackingCity_redirectingOnSoldiers()
        {
            Walker w = new Walker();
            City city = new City(1, 0);
            w.AttackCity(city, new DamageDispatcher());
            Assert.Equal(0, city.GetWall().Health);
            Assert.Equal(3, city.GetSoldiers()[0].HealthPoints);

        }
    }
}
