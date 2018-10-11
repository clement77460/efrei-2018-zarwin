using System;
using CholletJaworskiZarwin;

/**
 * 
 * NEED TO BE ADAPTED WITH GAME ENGINE FOR V3
 * 
**/


namespace CholletJaworskiZarwin
{
    public class Game
    {

        private City city;

        private int nbWalkersPerHorde;
        private Horde currentHorde;
        private int turn;
        private int nbHordes;

        private DamageDispatcher damageDispatcher;

        // Message corresponds to the output of the current turn, if you want to
        // display it in a console for example.
        private String message;

        public Game(int wallHealth, int nbSoldiers, int nbWalkersPerHorde, int nbHordes)
        {
            this.city = new City(nbSoldiers, wallHealth);
            this.damageDispatcher = new DamageDispatcher();
            this.nbWalkersPerHorde = nbWalkersPerHorde;
            this.nbHordes = nbHordes;
            this.turn = 0;
            this.message = "2078, Villejuif. The city has been fortified because of a Walkers invasion. \n" + 
                nbSoldiers + " soldiers are defending the city. Some Walkers are coming to the West of the Wall...";
        }

        public String Message => this.message;

        public void Turn()
        {
            if (!this.IsFinished())
            {
                // Create the horde
                this.currentHorde = new Horde(nbWalkersPerHorde);
                this.message = "The horde is coming. Brace yourselves.";
                
                // Then comes the siege phase

                city.DefendFromHorde(this.currentHorde);
                currentHorde.AttackCity(this.city, this.damageDispatcher);
                this.message = "The fight goes on.";

                this.turn++;

                // Create a new horde if needed.
                this.ManageHordes();
            }

            // Check if the game is finished (AFTER the turn) to set a message or not
            if(this.IsFinished())
            {
                this.message = "The game is finished.";
                if(this.city.GetNumberSoldiersAlive() > 0 && this.nbHordes == 0)
                {
                    this.message += " Soldiers defeated the walkers.";
                } else {
                    this.message += " The walkers defeated the soldiers.";
                }
            }

        }

        private void ManageHordes()
        {

            if(this.currentHorde.GetNumberWalkersAlive() == 0 && this.nbHordes > 1)
                //prise en compte de la vague actuelle
            {
                this.currentHorde = new Horde(this.nbWalkersPerHorde);
                this.nbHordes--;
                this.message = "Uh, it seems that another horde is coming...";
                this.turn = 0;
            }
        }

        public Boolean IsFinished()
        {
            if (this.currentHorde == null)
            {
                return false;
            }

            return (this.city.GetNumberSoldiersAlive() == 0 || this.currentHorde.GetNumberWalkersAlive() == 0);
        }

        public String SoldiersStats()
        {
            return this.city.SoldiersStats();
        }

        public int WallHealth => this.city.Wall.Health;

        public override String ToString()
        {
            String soldiers = "Soldiers are " + this.city.GetNumberSoldiersAlive() + " left. ";
            String walkers = this.currentHorde.GetNumberWalkersAlive() + " walker(s) are attacking. ";
            String current_turn = "This is the turn " + this.turn + ".";

            return soldiers + walkers + current_turn;
        }

    }
}

