using System;

namespace zombieLand
{
    class Program
    {
        private const int WALL_HEALTH = 3;
        private const int NB_SOLDIERS = 3;
        private const int NB_WALKERS_PER_HORDE = 10;

        static void Main(string[] args)
        {
            Game game = new Game(WALL_HEALTH, NB_SOLDIERS, NB_WALKERS_PER_HORDE);

            while(!game.IsFinished())
            {
                game.Turn();
                Console.WriteLine(game.Message);
                Console.WriteLine(game.ToString());
            }

            Console.WriteLine(game.Message);

        }

        private void PressEnter()
        {
            Console.WriteLine("Press Enter to continue...");
            ConsoleKeyInfo c;
            do
            {
                c = Console.ReadKey();
            } while (c.Key != ConsoleKey.Enter);
        }
    }
}
