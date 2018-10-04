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
        public void walkerAttackingWall()
        {
            Walker w = new Walker();
            City city = new City(1, 5);
            w.AttackCity(city,new DamageDispatcher());
            Assert.Equal(4, city.GetWall().Health);
           
        }
    }
}
