using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;

namespace CholletJaworskiZarwin.test
{
    public class UnitTestWalker
    {


        [Fact]
        public void WalkerAttackingCity_redirectingOnWall()
        {
            Walker w = new Walker();
            City city = new City(new CityParameters(5, 0), new SoldierParameters[] { new SoldierParameters(1, 1) }, new Order[0],
                new ActionTrigger(true));
            w.AttackCity(city, new DamageDispatcher());
            w.ToString();

            int idWalker = w.Id; //impossible to get 0 its a random execution so no ASSERT.EQUAL()

            Assert.Equal(4, city.Wall.Health);

        }

        [Fact]
        public void WalkerAttackingCity_redirectingOnSoldiers()
        {
            Walker.walkerCounterId = 0;
            Walker w = new Walker();
            
            City city = new City(new CityParameters(0, 0), new SoldierParameters[] 
            { new SoldierParameters(1, 1) }, new Order[0],new ActionTrigger(true));
            w.AttackCity(city, new DamageDispatcher());
            Assert.Equal(0, city.Wall.Health);
            Assert.Equal(3, city.GetSoldiers()[0].HealthPoints);

        }

        [Fact]
        public void WalkerIsHurtedTwoTimesOnTheSameTurn()
        {
            Walker.walkerCounterId = 0;
            Walker w = new Walker();

            Assert.Equal(0, w.DamageTaken);
            Assert.Equal(0, w.DamageTurn);

            w.Hurt(1, 1);

            Assert.Equal(1, w.DamageTaken);
            Assert.Equal(1, w.DamageTurn);

            w.Hurt(1, 1);

            Assert.Equal(2, w.DamageTaken);
            Assert.Equal(1, w.DamageTurn);

        }
        [Fact]
        public void WalkerIsHurtedOneTimeOnEachTurn()
        {
            Walker.walkerCounterId = 0;
            Walker w = new Walker();

            Assert.Equal(0, w.DamageTaken);
            Assert.Equal(0, w.DamageTurn);

            w.Hurt(1, 1);

            Assert.Equal(1, w.DamageTaken);
            Assert.Equal(1, w.DamageTurn);

            w.Hurt(2, 1);

            Assert.Equal(1, w.DamageTaken);
            Assert.Equal(2, w.DamageTurn);

        }

    }
}
