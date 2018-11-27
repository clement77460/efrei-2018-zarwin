using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;
using Xunit;
using Zarwin.Shared.Contracts.Input;

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
        public void Creating8Walkers_KillingTwo_Then_OneShooting_One()
        {
            Horde horde = new Horde(8);
            horde.DoDamages(2, 1);
            Assert.Equal(6, horde.GetNumberWalkersAlive());
            horde.OneShotWalker();
            Assert.Equal(5, horde.GetNumberWalkersAlive());
        }

        /*[Fact] cause du changement du constructeur Horde a modifier !
        public void Creating_One_Tough_Walker_KillingIt_OnlyWithTwoDomages()
        {
            Horde horde = new Horde(new WaveHordeParameters(
                                    new ZombieParameter(ZombieType.Stalker, ZombieTrait.Tough, 1)));
            horde.DoDamages(1, 1);
            Assert.Equal(1, horde.GetNumberWalkersAlive());

            horde.DoDamages(2, 2);
            Assert.Equal(0, horde.GetNumberWalkersAlive());
        }*/

        //doWalkerDomages
        

    }
}
