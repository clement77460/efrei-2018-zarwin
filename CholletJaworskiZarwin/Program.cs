using System;
using System.Diagnostics.CodeAnalysis;

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
