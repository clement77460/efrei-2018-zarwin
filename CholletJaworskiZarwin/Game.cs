using System;
using System.Collections.Generic;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts.Core;
using Zarwin.Shared.Contracts.Input.Orders;

namespace CholletJaworskiZarwin
{
    public class Game
    {
        public Simulation simulation { get; private set; }
        public bool stopSimulation { get; set; } = false;
        private ActionTrigger actionTrigger;
        private DataSource ds;

        private City city;
        private Horde currentHorde;
        private IDamageDispatcher damageDispatcher;

        private int nbWalkersPerHorde;
        private int nbHordes;
        private int turn;

        //charger une simulation existante
        public Game(Simulation simulation,bool isTesting = true)
        {

            this.simulation = simulation;
            this.ds = new DataSource();
            this.ds.UpdateRunningStatus(this.simulation, 1);

            this.turn = this.simulation.turnResults.Count;

            this.InitEvent(isTesting);
            this.BuildEntitiesWithParameter(this.simulation.CreateParametersFromOldSimulation());

            this.city.CheckPastOrders(this.simulation.waveResults.Count, this.simulation.turnResults.Count);

            

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

            if(isTesting)
                this.InitTurn();

        }

        private void BuildEntitiesWithParameter(Parameters parameters)
        {
            this.city = new City(parameters.CityParameters, parameters.SoldierParameters, parameters.Orders,actionTrigger);
            

            this.damageDispatcher = parameters.DamageDispatcher;
            this.nbHordes = parameters.WavesToRun;
            this.nbWalkersPerHorde = this.CountNumberOfWalkers();
            this.currentHorde = new Horde(this.simulation.zombieParameter[this.simulation.waveResults.Count]);
        }

        private void InitEvent(bool isTesting)
        {
            this.actionTrigger = new ActionTrigger(isTesting);

            ActionListener actionListener = new ActionListener();
            actionListener.SaveActionTrigger(this.actionTrigger);
            
        }

        public void InitTurn()
        {
            if (!this.stopSimulation)
            {
                List<SoldierState> soldierStates = this.city.GetSoldiersStates();
                HordeState hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());

                this.simulation.createInitTurn(soldierStates.ToArray(), hordeState, this.city.Wall.Health, city.Coin);


                this.ApproachTurn(0);

            }

        }


        public void ApproachTurn(int numberOfTurnDone)
        {

            for (int i = numberOfTurnDone; i < city.nbTower + 1; i++)
            {
                if (!this.stopSimulation)
                {
                    System.Console.WriteLine("tour d'approche\n\n");
                    this.city.ExecuteOrder(turn, this.simulation.waveResults.Count, city.Coin);
                    this.city.SnipersAreShoting(this.currentHorde);
                    List<SoldierState> soldierStates = this.city.GetSoldiersStates();
                    HordeState hordeState = new HordeState(this.currentHorde.GetNumberWalkersAlive());

                    if (this.city.GetSoldiers().Count > 0)
                    {
                        this.simulation.addTurnResult(soldierStates.ToArray(), hordeState, this.city.Wall.Health, city.Coin);
                    }

                    turn++;
                    this.actionTrigger.EndTurnTime(simulation, this.currentHorde.GetNumberWalkersAlive());
                }
            }

            this.Turn();

        }

        public void Turn()
        {
            if (!stopSimulation)
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
            }


            if (!this.stopSimulation)
            {
                this.ManageHordes();

                if (this.IsFinished())
                {
                    //on fini la game donc on supprime en base : info non validee
                    //Si terminée, plus en base, donc plus dans la liste
                    ds.DeleteSimulation(this.simulation.IdString);

                }
            }
            else
            {
                //on update le running status à 0
                ds.UpdateRunningStatus(this.simulation, 0);
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


        public void UpdateCityOrders(Order[] newOrders)
        {
            this.city.ReplacingOrdersList(newOrders);
        }

        public override String ToString()
        {
            String soldiers = "Soldiers are " + this.city.NumberSoldiersAlive+ " left. ";
            String walkers = this.currentHorde.GetNumberWalkersAlive() + " walker(s) are attacking. ";

            return soldiers + walkers;
        }

    }
}

