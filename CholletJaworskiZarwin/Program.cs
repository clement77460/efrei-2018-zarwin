using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Tests;

namespace CholletJaworskiZarwin
{
    class Program
    {
        private const int WALL_HEALTH = 3;
        private const int NB_SOLDIERS = 3;
        private const int NB_WALKERS_PER_HORDE = 10;
        private const int NB_HORDES = 1;

        [ExcludeFromCodeCoverage]
        private static void Main(string[] args)
        {
            Game game = new Game(WALL_HEALTH, NB_SOLDIERS, NB_WALKERS_PER_HORDE, NB_HORDES);

            while (!game.IsFinished())
            {
                Console.WriteLine(game.Message);
                game.Turn();

                PressEnter();

                Console.WriteLine(game);
                Console.WriteLine("The Wall has " + game.WallHealth + "HP left.");
                Console.WriteLine(game.SoldiersStats());

            }

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

//    [ExcludeFromCodeCoverage]
//    private static void Main(string[] args)
//    {
//        //Creating 2 soldiers
//        List<SoldierParameters> sp = new List<SoldierParameters>();
//        sp.Add(new SoldierParameters(0, 1));
//        sp.Add(new SoldierParameters(1, 1));

//        GameEngine gameEngine = new GameEngine(new Parameters(
//            1,
//            new FirstSoldierDamageDispatcher(),
//            new HordeParameters(10),
//            new CityParameters(5),
//            sp.ToArray()), false);

//        gameEngine.GameLoop();

//    }
}




