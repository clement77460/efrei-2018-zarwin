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
        private DataSource ds;

        private City city;
        private Horde currentHorde;
        private IDamageDispatcher damageDispatcher;

        private int nbWalkersPerHorde;
        private int nbHordes;
        private int turn;

        //charger une simulation existante
        public Game(bool isTesting = true)
        {
            this.ds = new DataSource();
            this.simulation=ds.ReadAllSimulations();
            

            this.turn = this.simulation.turnResults.Count;

            this.BuildEntitiesWithParameter(this.simulation.CreateParametersFromOldSimulation());

            this.InitEvent(isTesting);

            if (simulation.turnInit == null)
            {
                this.InitTurn();
            }
            else
            {
                this.Turn();
            }
        }

        //nouvelle partie avec des paramètres
        public Game(Parameters parameters,bool isTesting=true)
        {

            Soldier.ResetId();//resetting ID before each simulations

            this.simulation = new Simulation(parameters);
            this.ds = new DataSource();

            turn = 0;

            this.InitEvent(isTesting);
            this.BuildEntitiesWithParameter(parameters);

            
            this.InitTurn();

        }

        private void BuildEntitiesWithParameter(Parameters parameters)
        {
            this.city = new City(parameters.CityParameters, parameters.SoldierParameters, parameters.Orders,actionTrigger);
            this.damageDispatcher = parameters.DamageDispatcher;
            this.nbHordes = parameters.WavesToRun;
            this.nbWalkersPerHorde = this.CountNumberOfWalkers();
            this.currentHorde = new Horde(this.simulation.zombieParameter[0]);
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

                currentHorde.AttackCity(this.city, this.damageDispatcher);
                city.DefendFromHorde(this.currentHorde, this.turn);

                this.city.ExecuteOrder(turn, this.simulation.waveResults.Count, goldAtStartOfTurn);

                List<SoldierState> soldierStates = this.city.GetSoldiersStates();
                HordeState hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());


                this.simulation.addTurnResult(soldierStates.ToArray(), hordeState, this.city.Wall.Health, city.Coin);
                turn++;
                this.actionTrigger.EndTurnTime(simulation, this.currentHorde.GetNumberWalkersAlive());
            }
            
            

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


            if (this.currentHorde.GetNumberWalkersAlive() == 0)
            {
                this.EndWaveActions();

                if (this.nbHordes > 1)
                {
                    if (this.simulation.zombieParameter.Count > 1)
                    {
                        
                        this.currentHorde = new Horde(this.CountNumberOfWalkers());
                    }
                    else
                    {
                        this.currentHorde = new Horde(this.nbWalkersPerHorde);
                    }

                    this.nbHordes--;
                    this.InitTurn();
                }
            }
            else
            {
                if (this.city.GetSoldiers().Count <= 0)
                {
                    this.EndWaveActions();

                }
                else
                {
                    this.Turn();
                }
            }
        }

        private void EndWaveActions()
        {
            //on vide les turnResults
            this.turn = 0;
            this.simulation.addWaveResult();
            this.simulation.removeTurnResults();
            this.actionTrigger.EndWaveTime(simulation);
        }

        private int CountNumberOfWalkers()
        {
            int currentWave = this.simulation.waveResults.Count;
            int nb = 0;

            for (int i = 0; i < this.simulation.zombieParameter[currentWave].Length; i++)
            {
                nb += this.simulation.zombieParameter[currentWave][i].Count;
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

