using System;
using System.Collections.Generic;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts.Core;

namespace CholletJaworskiZarwin
{
    public class Game
    {

        private City city;

        private int nbWalkersPerHorde;
        private Horde currentHorde;
        private int nbHordes;
        private int turn = 0;
        private int currentWave = 0;
        public int WallHealth => this.city.Wall.Health;
        
        private Parameters parameters;
        
        private List<SoldierState> soldierStates = new List<SoldierState>();
        private HordeState hordeState;

        private TurnResult turnInit;
        private List<TurnResult> turnResults = new List<TurnResult>();
        private List<WaveResult> waveResults = new List<WaveResult>();

        private IDamageDispatcher damageDispatcher;

        public String Message { get; private set; }

        // Constructor for the console program
        public Game(int wallHealth, int nbSoldiers, int nbWalkersPerHorde, int nbHordes)
        {
            this.city = new City(nbSoldiers, wallHealth);
            this.nbWalkersPerHorde = nbWalkersPerHorde;
            this.currentHorde = new Horde(nbWalkersPerHorde);
            this.damageDispatcher = new DamageDispatcher();
            this.nbHordes = nbHordes;
            this.parameters = null;
            this.Message = "2078, Villejuif. The city has been fortified because of a Walkers invasion. \n" +
                nbSoldiers + " soldiers are defending the city. Some Walkers are coming to the West of the Wall...";

            // Approach phase
            this.Message = "The horde is coming. Brace yourselves.";
        }

        // Constructor for the tests
        public Game(Parameters parameters)
        {
            Soldier.ResetId();//resetting ID before each simulations

            this.parameters = parameters;
            this.city = new City(parameters);
            this.damageDispatcher = parameters.DamageDispatcher;
            this.nbHordes = this.parameters.WavesToRun;
            this.nbWalkersPerHorde = this.CountNumberOfWalkers(); //faire une methode pr calculer le nbr de zombies
            this.currentHorde = new Horde(parameters.HordeParameters.Waves[0]);

            //
            this.InitTurn();

            // Approach phase
            this.Message = "The horde is coming. Brace yourselves.";
        }

        private void InitTurn()
        {
            this.soldierStates = this.city.GetSoldiersStates();
            this.hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());
            this.turnInit = new TurnResult(this.soldierStates.ToArray(), this.hordeState, this.city.Wall.Health, city.Coin);
            // Create initial results
            for (int i = 0; i < city.nbTower+1; i++)
            {
                this.city.ExecuteOrder(turn, waveResults.Count, city.Coin);
                this.city.SnipersAreShoting(this.currentHorde);
                this.soldierStates = this.city.GetSoldiersStates();
                this.hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());
                if (this.city.GetSoldiers().Count > 0)
                {
                    this.turnResults.Add(new TurnResult(this.soldierStates.ToArray(), this.hordeState, this.city.Wall.Health, city.Coin));
                }
                
            }
            turn++;


            this.Turn();

        }

        public void Turn()
        {

            if (!this.IsFinished())
            {
                int goldAtStartOfTurn = this.city.Coin;


                // Horde attacks the city (and its soldiers)
                currentHorde.AttackCity(this.city, this.damageDispatcher);

                // Soldiers attacks the horde to defend the city
                city.DefendFromHorde(this.currentHorde, this.turn);

                this.Message = "The fight goes on.";

                this.city.ExecuteOrder(turn, waveResults.Count, goldAtStartOfTurn);

                // Update stats
                this.soldierStates = this.city.GetSoldiersStates();
                this.hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());

                // Add turn results
                this.turnResults.Add(new TurnResult(this.soldierStates.ToArray(), this.hordeState, this.city.Wall.Health, city.Coin));
                turn++;
            }

            // Create a new horde if needed.
            this.ManageHordes();

            // Check if the game is finished (AFTER the turn) to set a message or not
            if (this.IsFinished())
            {
                this.Message = "The game is finished.";
                if (this.city.NumberSoldiersAlive> 0)
                {
                    this.Message += " Soldiers defeated the walkers.";
                }
                else
                {
                    if(this.nbHordes != 0)
                        this.Message += " The walkers defeated the soldiers.";
                }

            }
        }

        public Result GetResult()
        {
            return new Result("CholletJawordki", this.waveResults.ToArray());
        }

        private void ManageHordes()
        {
            if (this.currentHorde.GetNumberWalkersAlive() == 0)
            {
                // Add turnResults to waveResults
                this.waveResults.Add(new WaveResult(turnInit, turnResults.ToArray()));

                if (this.nbHordes > 1)
                {
                    this.currentWave++;

                    
                    if (this.parameters.HordeParameters.Waves.Length > 1)
                    {
                        
                        this.currentHorde = new Horde(this.CountNumberOfWalkers());
                    }
                    else
                    {
                        this.currentHorde = new Horde(this.nbWalkersPerHorde);
                    }

                    this.nbHordes--;
                    
                    this.Message = "Uh, it seems that another horde is coming...";

                    //on vide les turnResults
                    this.turn = 0;
                    this.turnResults.RemoveRange(0,this.turnResults.Count);
                    this.InitTurn();
                }
            }
            else
            {
                if (this.city.GetSoldiers().Count <= 0)
                {
                    this.waveResults.Add(new WaveResult(turnInit, turnResults.ToArray()));
                }
            }
        }

        private int CountNumberOfWalkers()
        {
            int nb = 0;
            for (int i = 0; i < this.parameters.HordeParameters.Waves[currentWave].ZombieParameters.Length; i++)
            {
                nb += this.parameters.HordeParameters.Waves[currentWave].ZombieParameters[i].Count;
            }

            return nb;
        }

        public Boolean IsFinished()
        {
            if (city.GetSoldiers().Count <= 0)
                return true;
            if (currentHorde.GetNumberWalkersAlive() == 0)
                return true;
            return false;
        }

        public String SoldiersStats()
        {
            return this.city.SoldiersStats();
        }

        public override String ToString()
        {
            String soldiers = "Soldiers are " + this.city.NumberSoldiersAlive+ " left. ";
            String walkers = this.currentHorde.GetNumberWalkersAlive() + " walker(s) are attacking. ";

            return soldiers + walkers;
        }


    }
}

