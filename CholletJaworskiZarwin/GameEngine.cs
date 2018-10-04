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
        private Parameters parameters;

        private List<WaveResult> waveResult = new List<WaveResult>();
        private SoldierParameters[] soldierParameter;

        private Soldier[] soldier;
        private List<SoldierState> soldierState = new List<SoldierState>();

        private HordeState hordeState;
        private Wall wall;
        private Horde horde;

        private TurnResult turnInit;
        private List<TurnResult> turnResults = new List<TurnResult>();

        public GameEngine(Parameters parameters)
        {
            this.parameters = parameters;
            soldierParameter = parameters.SoldierParameters;

            soldier = new Soldier[soldierParameter.Length];
            this.buildingSoldiers(soldierParameter, soldier);

            this.refreshingSoldierState(soldierState, soldier);

            wall = new Wall(parameters.CityParameters.WallHealthPoints);
            horde = new Horde(parameters.HordeParameters.Size);
            hordeState = new HordeState(horde.GetNumberWalkersAlive());

            turnInit = new TurnResult(soldierState.ToArray(), hordeState, wall.Health);
        }


        public Result GameLoop()
        {
            if (soldier.Length == 0)
            {
                waveResult.Add(new WaveResult(turnInit, turnResults.ToArray()));
                return new Result(waveResult.ToArray());
            }

            turnResults.Add(turnInit);
            while (soldier.Length > 0 && horde.GetNumberWalkersAlive() > 0)
            {

                this.DoingTurnActions();
            }

            //pour le premier test qu'une vague donc a generaliser !
            waveResult.Add(new WaveResult(turnInit, turnResults.ToArray()));

            return new Result(waveResult.ToArray());
        }

        private void DoingTurnActions()
        {
            //soldiers attacking
            this.soldiersAttacking(soldier, horde);

            //Horde Attacking
            this.hordeDoingDomages(parameters, horde, soldier);


            //refreshing all xxxxState
            this.refreshingSoldierState(soldierState, soldier);
            hordeState = new HordeState(horde.GetNumberWalkersAlive());

            //completing new turn with new xxxxState
            turnResults.Add(new TurnResult(soldierState.ToArray(), hordeState, wall.Health));
        }

        private void hordeDoingDomages(Parameters parameters, Horde horde, Soldier[] soldier)
        {
            int domageToDo = 0;

            //zombie attaque pas besoin pour le premier test a generaliser !!
            for (int i = 0; i < horde.GetNumberWalkersAlive(); i++)
            {
                domageToDo++;
            }
            parameters.DamageDispatcher.DispatchDamage(domageToDo, soldier);
            domageToDo = 0;
        }

        private void refreshingSoldierState(List<SoldierState> st, Soldier[] soldier)
        {
            st.Clear();
            for (int i = 0; i < soldier.Length; i++)
            {
                st.Add(new SoldierState(soldier[i].Id, soldier[i].Level, soldier[i].HealthPoints));
            }
        }

        private void buildingSoldiers(SoldierParameters[] sp, Soldier[] soldier)
        {
            for (int i = 0; i < sp.Length; i++)
            {
                soldier[i] = new Soldier(sp[i].Id, sp[i].Level);
            }
        }

        private void soldiersAttacking(Soldier[] soldier, Horde horde)
        {
            foreach (Soldier s in soldier)
            {
                if (horde.KillWalker())
                {
                    s.LevelUp();
                }
            }
        }

       


    }
}
