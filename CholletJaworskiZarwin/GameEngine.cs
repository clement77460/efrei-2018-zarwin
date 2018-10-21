using System;
using System.Collections.Generic;
using System.Text;
using Zarwin.Shared.Contracts;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using System.Diagnostics;

namespace CholletJaworskiZarwin
{
    public class GameEngine
    {
        private bool testTime;

        private Parameters parameters;

        private List<WaveResult> waveResult = new List<WaveResult>();
        private SoldierParameters[] soldierParameter;

        private List<Soldier> soldiers = new List<Soldier>();
        private List<SoldierState> soldierState = new List<SoldierState>();

        private HordeState hordeState;
        private Wall wall;
        private Horde horde;

        private TurnResult turnInit;
        private List<TurnResult> turnResults = new List<TurnResult>();

        public GameEngine(Parameters parameters,bool test=true)
        {
            this.testTime = test;
            this.parameters = parameters;
            soldierParameter = parameters.SoldierParameters;

            
            this.buildingSoldiers(soldierParameter);

            this.refreshingSoldierState(soldierState);

            wall = new Wall(parameters.CityParameters.WallHealthPoints);
            horde = new Horde(parameters.HordeParameters.Size);
            hordeState = new HordeState(horde.GetNumberWalkersAlive());

            turnInit = new TurnResult(soldierState.ToArray(), hordeState, wall.Health);
        }


        public Result GameLoop()
        {
            if (soldiers.Count == 0)
            {
                waveResult.Add(new WaveResult(turnInit, turnResults.ToArray()));
                return new Result(waveResult.ToArray());
            }

            turnResults.Add(turnInit);
            while (soldiers.Count > 0 && horde.GetNumberWalkersAlive() > 0)
            {

                this.DoingTurnActions();
                if (!testTime)
                {
                    this.displayMsg();
                    this.PressEnter();
                }
            }

            //pour le premier test qu'une vague donc a generaliser !
            waveResult.Add(new WaveResult(turnInit, turnResults.ToArray()));

            return new Result(waveResult.ToArray());
        }

        private void DoingTurnActions()
        {
            //soldiers attacking
            this.soldiersAttacking(horde);

            //Horde Attacking
            this.hordeDoingDomages(parameters, horde);


            //refreshing all xxxxState
            this.refreshingSoldierState(soldierState);
            hordeState = new HordeState(horde.GetNumberWalkersAlive());

            //completing new turn with new xxxxState
            turnResults.Add(new TurnResult(soldierState.ToArray(), hordeState, wall.Health));
        }

        private void hordeDoingDomages(Parameters parameters, Horde horde)
        {
            int domageToDo = 0;

            
            for (int i = 0; i < horde.GetNumberWalkersAlive(); i++)
            {
                domageToDo++;
            }
            if (wall.Health > 0)
            {
                wall.WeakenWall(domageToDo);
            }
            else
            {
                parameters.DamageDispatcher.DispatchDamage(domageToDo, soldiers);
            }
            
            foreach(Soldier s in soldiers.ToArray())
            {
                if (s.HealthPoints <= 0)
                {
                    soldiers.Remove(s);
                }
            }


            domageToDo = 0;
        }

        private void refreshingSoldierState(List<SoldierState> st)
        {
            st.Clear();
            for (int i = 0; i < soldiers.Count; i++)
            {
                st.Add(new SoldierState(soldiers[i].Id, soldiers[i].Level, soldiers[i].HealthPoints));
            }
        }

        private void buildingSoldiers(SoldierParameters[] sp)
        {
            for (int i = 0; i < sp.Length; i++)
            {
                soldiers.Add(new Soldier(sp[i].Id, sp[i].Level));
        
            }
        }

        private void soldiersAttacking(Horde horde)
        {
            foreach (Soldier s in soldiers)
            {
                if (horde.KillWalker())
                {
                    s.LevelUp();
                }
            }
        }

        private void PressEnter()
        {
            Console.WriteLine("\nPress Enter to continue...");
            ConsoleKeyInfo c;
            do
            {
                c = Console.ReadKey();

            } while (c.Key != ConsoleKey.Enter);
        }

        private void displayMsg()
        {
            Console.WriteLine("il reste : {0} walkers", horde.GetNumberWalkersAlive());
            Console.WriteLine("le wall possède : {0} pdv", wall.Health);
            foreach(Soldier s in soldiers)
            {
                Console.WriteLine(s.ToString());
            }
        }
    }
}
