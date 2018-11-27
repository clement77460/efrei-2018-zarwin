using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;
using Zarwin.Shared.Tests;

namespace CholletJaworskiZarwin
{
    class Program
    {

        [ExcludeFromCodeCoverage]
        private static void Main(string[] args)
        {
            /*var input = new Parameters(
                11,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(1),
                new CityParameters(0),
                new Order[0],
                new SoldierParameters(1, 1));*/


            int typeGame = 1; // 1: on reprend une simulation existante (la premiere de la liste donc la derniere executé il me semble)
                              //    il faudrait ajouter un systeme de choix a voir pour plus tard
                              //2: on commence une partie ce qui permet d'ajouter une simulation 
            if (typeGame == 1)
            {
                Game game = new Game(false);
            }
            else
            {

                var input = new Parameters(
                       2,
                       new FirstSoldierDamageDispatcher(),
                       new HordeParameters(
                           new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 1)),
                           new WaveHordeParameters(new ZombieParameter(ZombieType.Stalker, ZombieTrait.Normal, 4))),
                       new CityParameters(1, 20),
                       new Order[] //ne pas les ajouter sinon ca casse mongoDB ....zzzz...
                       {
                                //new Order(0, 1, OrderType.ReinforceTower),
                                //new Equipment(0, 1, OrderType.EquipWithSniper, 1),
                       },
                       new SoldierParameters(1, 1));

                Game game = new Game(input, false);
            }

        }
        
    }
}




