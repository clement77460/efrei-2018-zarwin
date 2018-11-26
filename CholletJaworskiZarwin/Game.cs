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
        private Simulation simulation;
        private ActionTrigger actionTrigger;

        private City city;
        private Horde currentHorde;

        private int nbWalkersPerHorde;
        private int nbHordes;
        private int turn = 0;

        public Game(Parameters parameters,bool isTesting=true)
        {

            Soldier.ResetId();//resetting ID before each simulations

            this.simulation = new Simulation();
            this.simulation.parameter = parameters;


            this.city = new City(this.simulation.parameter.CityParameters, this.simulation.parameter.SoldierParameters, this.simulation.parameter.Orders);

            this.nbHordes = this.simulation.parameter.WavesToRun;
            this.nbWalkersPerHorde = this.CountNumberOfWalkers(); 
            this.currentHorde = new Horde(this.simulation.parameter.HordeParameters.Waves[0]);

            this.InitEvent(isTesting);
            this.InitTurn();

        }

        private void InitEvent(bool isTesting)
        {
            this.actionTrigger = new ActionTrigger(isTesting);

            ActionListener actionListener = new ActionListener();
            actionListener.SaveActionTrigger(this.actionTrigger);
            
        }

        private void InitTurn()
        {
            List<SoldierState> soldierStates = this.city.GetSoldiersStates();
            HordeState hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());

            this.simulation.createInitTurn(soldierStates.ToArray(), hordeState, this.city.Wall.Health, city.Coin);
            
            // Create initial results
            for (int i = 0; i < city.nbTower+1; i++)
            {
                this.city.ExecuteOrder(turn, this.simulation.waveResults.Count, city.Coin);
                this.city.SnipersAreShoting(this.currentHorde);
                soldierStates = this.city.GetSoldiersStates();
                hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());
                if (this.city.GetSoldiers().Count > 0)
                {
                    this.simulation.addTurnResult(soldierStates.ToArray(), hordeState, this.city.Wall.Health, city.Coin);
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

                currentHorde.AttackCity(this.city, this.simulation.parameter.DamageDispatcher);
                city.DefendFromHorde(this.currentHorde, this.turn);

                this.city.ExecuteOrder(turn, this.simulation.waveResults.Count, goldAtStartOfTurn);

                List<SoldierState> soldierStates = this.city.GetSoldiersStates();
                HordeState hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());


                this.simulation.addTurnResult(soldierStates.ToArray(), hordeState, this.city.Wall.Health, city.Coin);
                turn++;
            }
            
            this.actionTrigger.EndTurnTime();

            this.ManageHordes();

            // Check if the game is finished (AFTER the turn) to set a message or not
            if (this.IsFinished())
            {
                
                if (this.city.NumberSoldiersAlive> 0)
                {
                    //Soldiers defeated the walkers
                }
                else
                {
                    if(this.nbHordes != 0)
                    {
                        // The walkers defeated the soldiers
                    }

                }

            }
        }

        public Result GetResult()
        {
            return new Result("CholletJawordki", this.simulation.waveResults.ToArray());
        }

        private void ManageHordes()
        {
            this.actionTrigger.EndWaveTime();

            if (this.currentHorde.GetNumberWalkersAlive() == 0)
            {
                // Add turnResults to waveResults
                this.simulation.addWaveResult();

                if (this.nbHordes > 1)
                {

                    
                    if (this.simulation.parameter.HordeParameters.Waves.Length > 1)
                    {
                        
                        this.currentHorde = new Horde(this.CountNumberOfWalkers());
                    }
                    else
                    {
                        this.currentHorde = new Horde(this.nbWalkersPerHorde);
                    }

                    this.nbHordes--;                   

                    //on vide les turnResults
                    this.turn = 0;
                    this.simulation.removeTurnResults();
                    this.InitTurn();
                }
            }
            else
            {
                if (this.city.GetSoldiers().Count <= 0)
                {
                    this.simulation.addWaveResult();
                }
            }
        }

        private int CountNumberOfWalkers()
        {
            int currentWave = this.simulation.waveResults.Count;
            int nb = 0;

            for (int i = 0; i < this.simulation.parameter.HordeParameters.Waves[currentWave].ZombieParameters.Length; i++)
            {
                nb += this.simulation.parameter.HordeParameters.Waves[currentWave].ZombieParameters[i].Count;
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

