using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;
using Xunit;

namespace ChollerJaworskiZarwin.test
{
    public class UnitTestSoldier
    {
        [Fact]
        public void CreatingLvl1Soldier()
        {
            Soldier s = new Soldier();
            
            Assert.Equal(4, s.HealthPoints);
            Assert.Equal(1, s.Level);
            Assert.NotEmpty(s.ToString());
            int id = s.Id; //impossible to get 0 its a random execution so no ASSERT.EQUAL()
            
        }

        [Fact]
        public void CreatingLvl10Soldier()
        {
            Soldier s = new Soldier(0, 10);
            Assert.Equal(13, s.HealthPoints);
            Assert.Equal(10, s.Level);
        }

        [Fact]
        public void LvlUpASoldier()
        {
            Soldier s = new Soldier(0, 1);
            s.LevelUp();
            Assert.Equal(2, s.Level);
            Assert.Equal(5, s.HealthPoints);
        }

        [Fact]
        public void AtLvl1SoldierCanKill1Horde()
        {
            Soldier s = new Soldier();
            Horde horde = new Horde(2);

            s.Defend(horde);
            Assert.Equal(1, horde.GetNumberWalkersAlive());
        }

        [Fact]
        public void _2SoldiersLVL1CanKill2Walkers()
        {
            Soldier s = new Soldier();
            Soldier s2 = new Soldier();
            Horde horde = new Horde(3);

            s.Defend(horde);
            s2.Defend(horde);
            Assert.Equal(1, horde.GetNumberWalkersAlive());
        }

        [Fact]
        public void AtLvl11SoldierCanKill2Horde()
        {
            Soldier s = new Soldier(0,11);
            Horde horde = new Horde(3);

            s.Defend(horde);
            Assert.Equal(1, horde.GetNumberWalkersAlive());
        }

        [Fact]
        public void AtLvl21SoldierCanKill3Horde()
        {
            Soldier s = new Soldier(0, 21);
            Horde horde = new Horde(4);

            s.Defend(horde);
            Assert.Equal(1, horde.GetNumberWalkersAlive());
        }

        [Fact]
        public void SoldierLosing2HP()
        {
            Soldier s = new Soldier();

            s.Hurt(2);
            Assert.Equal(2, s.HealthPoints);
        }
    }
}
