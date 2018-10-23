﻿using System;
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
        private int turn;
        private int nbHordes;

        // Parameters given to the engine
        private Parameters parameters;

        // Stats objects
        private List<SoldierState> soldierStates = new List<SoldierState>();
        private HordeState hordeState;

        // Results
        private TurnResult turnInit;
        private List<TurnResult> turnResults = new List<TurnResult>();
        private List<WaveResult> waveResults = new List<WaveResult>();

        IDamageDispatcher damageDispatcher;

        // Message corresponds to the output of the current turn, if you want to
        // display it in a console for example.
        private String message;

        // Constructor for the console program
        public Game(int wallHealth, int nbSoldiers, int nbWalkersPerHorde, int nbHordes)
        {
            this.city = new City(nbSoldiers, wallHealth);
            this.nbWalkersPerHorde = nbWalkersPerHorde;
            this.currentHorde = new Horde(nbWalkersPerHorde);
            this.damageDispatcher = new DamageDispatcher();
            this.nbHordes = nbHordes;
            this.turn = 0;
            this.message = "2078, Villejuif. The city has been fortified because of a Walkers invasion. \n" + 
                nbSoldiers + " soldiers are defending the city. Some Walkers are coming to the West of the Wall...";

            // Approach phase
            this.message = "The horde is coming. Brace yourselves.";
        }

        // Constructor for the tests
        public Game(Parameters parameters)
        {
            this.parameters = parameters;
            this.city = new City(parameters);
            this.damageDispatcher = parameters.DamageDispatcher;
            this.nbHordes = this.parameters.WavesToRun;
            this.nbWalkersPerHorde = this.parameters.HordeParameters.Waves.Length;
            this.currentHorde = new Horde(nbWalkersPerHorde);
            this.turn = 0;

            // Create initial results
            this.soldierStates = this.city.GetSoldiersStates();
            this.hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());
            this.turnInit = new TurnResult(this.soldierStates.ToArray(), this.hordeState, this.city.Wall.Health,0);

            // Approach phase
            this.message = "The horde is coming. Brace yourselves.";
        }

        public String Message => this.message;

        public void Turn()
        {
            if(this.turn == 0)
            {
                if(this.city.GetNumberSoldiersAlive() == 0)
                {
                    // Add turnResults to waveResults
                    this.waveResults.Add(new WaveResult(turnInit, turnResults.ToArray()));
                }
                this.turnResults.Add(this.turnInit);
            }

            // Increment turn counter
            this.turn++;

            if (!this.IsFinished())
            {
                // Soldiers attacks the horde to defend the city
                city.DefendFromHorde(this.currentHorde);

                // Horde attacks the city (and its soldiers)
                currentHorde.AttackCity(this.city, this.damageDispatcher);
                this.message = "The fight goes on.";

                // Update stats
                this.soldierStates = this.city.GetSoldiersStates();
                this.hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());


                // Add turn results
                this.turnResults.Add(new TurnResult(this.soldierStates.ToArray(), this.hordeState, this.city.Wall.Health,0));
            }

            // Create a new horde if needed.
            this.ManageHordes();

            // Check if the game is finished (AFTER the turn) to set a message or not
            if (this.IsFinished())
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

        public Result GetResult()
        {
            return new Result(this.waveResults.ToArray());
        }

        private void ManageHordes()
        {
            if (this.currentHorde.GetNumberWalkersAlive() == 0)
            {
                // Add turnResults to waveResults
                this.waveResults.Add(new WaveResult(turnInit, turnResults.ToArray()));

                if (this.nbHordes > 1)
                {
                    this.currentHorde = new Horde(this.nbWalkersPerHorde);
                    this.nbHordes--;
                    this.message = "Uh, it seems that another horde is coming...";
                }
            }
        }

        public Boolean IsFinished()
        {
            return (turn > 0 && (this.city.GetNumberSoldiersAlive() == 0 || this.currentHorde.GetNumberWalkersAlive() == 0));
        }

        public String SoldiersStats()
        {
            return this.city.SoldiersStats();
        }

        public int WallHealth => this.city.Wall.Health;

        public void RefreshSoldierStates()
        {
            this.soldierStates.Clear();
            this.soldierStates = city.GetSoldiersStates();
        }

        public override String ToString()
        {
            String soldiers = "Soldiers are " + this.city.GetNumberSoldiersAlive() + " left. ";
            String walkers = this.currentHorde.GetNumberWalkersAlive() + " walker(s) are attacking. ";
            String current_turn = "This is the turn " + this.turn + ".";

            return soldiers + walkers + current_turn;
        }

    }
}

