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
            var param = new Parameters(
                1,
                new FirstSoldierDamageDispatcher(),
                new HordeParameters(5),
                new CityParameters(5),
                new Order[0],
                new SoldierParameters(1, 1));

            Game game = new Game(param,false);
            
            /*while (!game.IsFinished())
            {
                Console.WriteLine(game.Message);
                
                game.Turn();

                PressEnter();

                Console.WriteLine(game);
                Console.WriteLine("The Wall has " + game.WallHealth + "HP left.");
                Console.WriteLine(game.SoldiersStats());

            }
            PressEnter();*/
            Console.WriteLine(game);
            Console.WriteLine(game.SoldiersStats());

            PressEnter();
        }
        [ExcludeFromCodeCoverage]
        private static void PressEnter()
        {
            Console.WriteLine("\nPress Enter to continue...");
            ConsoleKeyInfo c;
            do
            {
                c = Console.ReadKey();

            } while (c.Key != ConsoleKey.Enter);
        }
    }
}




