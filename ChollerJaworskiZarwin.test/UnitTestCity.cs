using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;

namespace CholletJaworskiZarwin.test
{
    public class UnitTestCity
    {
        [Fact]
        public void DefendFromHorde_KillingOneWalker_NoSoldiersDying()
        {
            Horde horde = new Horde(5);
            City city = new City(new CityParameters(5, 0), new SoldierParameters[] 
            { new SoldierParameters(1, 1) }, new Order[0],new ActionTrigger(true));
            city.DefendFromHorde(horde, 1);
            //getting stats useless for unit test .........
            city.SoldiersStats();
            Assert.Equal(4, horde.GetNumberWalkersAlive());
            Assert.Equal(1, city.NumberSoldiersAlive);
            Assert.False(city.AreAllSoldiersDead());
        }
        [Fact]
        public void WalkersKilledEverySoldiers()
        {
            //nbSoldats - wallhealt
            Horde horde = new Horde(8);
            City city = new City(new CityParameters(5, 0), new SoldierParameters[] 
            { new SoldierParameters(1, 1) },new Order[0], new ActionTrigger(true));
            city.HurtSoldiers(8, new DamageDispatcher());
            Assert.True(city.AreAllSoldiersDead());
        }

        [Fact]
        public void Creating_MachineGun_And_Rifle_And_Recruiting_Soldier()
        {
            Parameters param = new Parameters(
                1, null, null,
                new CityParameters(2, 30),
                new Order[]
                {
                    new Order(0, 0, OrderType.RecruitSoldier),
                    new Equipment(0,1,OrderType.EquipWithShotgun,1),
                    new Equipment(0, 1, OrderType.EquipWithMachineGun, 1),
                },
                new SoldierParameters(1, 1));

            City city = new City(param.CityParameters, param.SoldierParameters, param.Orders,
                 new ActionTrigger(true));
            Horde h = new Horde(11);

            city.ExecuteOrder(0, 0, city.Coin);
            Assert.Equal(20, city.Coin);

            city.ExecuteOrder(1, 0, city.Coin);
            Assert.Equal(0, city.Coin);

            city.DefendFromHorde(h, 1);
            Assert.Equal(6, h.GetNumberWalkersAlive());//tue 4+1 zombies
            Assert.Equal(5, city.GetSoldiers()[0].Level);

            city.Wall.WeakenWall(2);//destruction du mur pour utiliser le shotgun
            Assert.Equal(0, city.Wall.Health);

            city.DefendFromHorde(h, 1);
            Assert.Equal(3, h.GetNumberWalkersAlive());//tue 2+1 zombies

        }

        [Fact]
        public void HealingSoldier()
        {
            Parameters param = new Parameters(
                1, null, null,
                new CityParameters(0, 1),
                new Order[]
                {
                    new Medipack(0, 1, 1, 1),
                },
                new SoldierParameters(1, 1));

            City city = new City(param.CityParameters, param.SoldierParameters, param.Orders,
                 new ActionTrigger(true));

            city.GetSoldiers()[0].Hurt(1);
            Assert.Equal(3, city.GetSoldiers()[0].HealthPoints);

            city.ExecuteOrder(1, 0, city.Coin);
            Assert.Equal(0, city.Coin);
            Assert.Equal(4, city.GetSoldiers()[0].HealthPoints);

        }

        [Fact]
        public void OverHealingSoldier_DoesntAddMoreLife()
        {
            Parameters param = new Parameters(
                1, null, null,
                new CityParameters(0, 3),
                new Order[]
                {
                    new Medipack(0, 1, 1, 3),
                },
                new SoldierParameters(1, 1));

            City city = new City(param.CityParameters, param.SoldierParameters, param.Orders
                , new ActionTrigger(true));

            city.ExecuteOrder(1, 0, city.Coin);
            Assert.Equal(0, city.Coin);
            Assert.Equal(4, city.GetSoldiers()[0].HealthPoints);

        }

        [Fact]
        public void EquipSniperToSnipeThemAll_ButCantAttackAfterInitTurn()
        {
            var param = new Parameters(
                1,null,null,
                new CityParameters(1, 20),
                new Order[]
                {
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1),
                },
                new SoldierParameters(1, 1));

            Horde horde = new Horde(2);
            City city = new City(param.CityParameters, param.SoldierParameters, 
                param.Orders, new ActionTrigger(true));
            city.ExecuteOrder(1, 0,city.Coin);

            city.SnipersAreShoting(horde);
            Assert.Equal(1, horde.GetNumberWalkersAlive());

            city.DefendFromHorde(horde, 1);
            Assert.Equal(1, horde.GetNumberWalkersAlive());//cant attack with a sniper
        }

        [Fact]
        public void CantBuyIfNoGold()
        {
            var param = new Parameters(
                1, null, null,
                new CityParameters(1, 1),
                new Order[]
                {
                    new Equipment(0, 1, OrderType.EquipWithSniper, 1),
                    new Equipment(0, 1, OrderType.EquipWithMachineGun, 1),
                    new Equipment(0, 1, OrderType.EquipWithShotgun, 1),
                    new Medipack(0, 1, 1, 3),
                    new Order(0, 1, OrderType.ReinforceTower),
                    new Zarwin.Shared.Contracts.Input.Orders.Wall(0, 1, 3),
                    new Order(0, 1, OrderType.RecruitSoldier)
                },
                new SoldierParameters(1, 1));

            City city = new City(param.CityParameters, param.SoldierParameters, param.Orders, 
                new ActionTrigger(true));
            city.ExecuteOrder(1, 0, city.Coin);
            Assert.Equal(1, city.Coin);
        }

        [Fact]
        public void PurchasingOneSoldierReduceMoneyAndAddOneSoldier()
        {
            var param = new Parameters(
                1, null, null,
                new CityParameters(1, 10),
                new Order[]
                {
                    new Order(0, 1, OrderType.RecruitSoldier)
                },
                new SoldierParameters(1, 1));

            City city = new City(param.CityParameters, param.SoldierParameters, param.Orders
                , new ActionTrigger(true));
            city.ExecuteOrder(1, 0, city.Coin);

            Assert.Equal(0, city.Coin);
            Assert.Equal(2, city.NumberSoldiersAlive);
        }
    }
}
