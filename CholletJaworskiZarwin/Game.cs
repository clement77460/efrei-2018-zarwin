using System;
using CholletJaworskiZarwin;

namespace zombieLand
{
    class Game
    {

        private City city;

        private int nbWalkersPerHorde;
        private Horde currentHorde;
        private int turn;

        private DamageDispatcher damageDispatcher;

        // Message corresponds to the output of the current turn, if you want to
        // display it in a console for example.
        private String message;

        public Game(int wallHealth, int nbSoldiers, int nbWalkersPerWave)
        {
            this.city = new City(nbSoldiers, wallHealth);
            this.nbWalkersPerHorde = nbWalkersPerWave;
            this.damageDispatcher = new DamageDispatcher();
            this.turn = 0;
        }

        public String Message => this.message;

        public void Turn()
        {
            if(!this.IsFinished())
            {
                // First turn is approach phase
                if (this.turn == 0)
                {
                    // Create the horde
                    this.currentHorde = new Horde(nbWalkersPerHorde);
                    this.message = "The horde is coming. Brace yourselves.";
                    this.turn++;

                }
                // Then comes the siege phase
                else
                {
                    city.DefendFromHorde(this.currentHorde);
                    currentHorde.AttackCity(this.city, this.damageDispatcher);
                    this.message = this.ToString();
                    this.turn++;
                }
            }

            // Check if the game is finished to set a message or not
            if(this.IsFinished())
            {
                this.message = "The game is finished.";
                if(this.city.GetNumberSoldiersAlive() > 0)
                {
                    this.message += " Soldiers defeated the walkers.";
                } else {
                    this.message += " The walkers defeated the soldiers.";
                }
            }

        }

        public Boolean IsFinished()
        {
            return (this.city.GetNumberSoldiersAlive() == 0) || (this.currentHorde.GetNumberWalkersAlive() == 0);
        }

        public override String ToString()
        {
            String soldiers = "Soldiers are " + this.city.GetNumberSoldiersAlive() + " left.";
            String walkers = this.currentHorde.GetNumberWalkersAlive() + " walker(s) still attacking.";
            String current_turn = "This is the turn " + this.turn + ".";

            return soldiers + walkers + current_turn;
        }

    }
}

