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
            w.ToString();

            int idWalker = w.Id; //impossible to get 0 its a random execution so no ASSERT.EQUAL()
            
            Assert.Equal(4, city.Wall.Health);
           
        }

        [Fact]
        public void walkerAttackingCity_redirectingOnSoldiers()
        {
            Walker.walkerCounterId = 0;
            Walker w = new Walker();
            City city = new City(1, 0);
            w.AttackCity(city, new DamageDispatcher());
            Assert.Equal(0, city.Wall.Health);
            Assert.Equal(3, city.GetSoldiers()[0].HealthPoints);

        }
    }
}
