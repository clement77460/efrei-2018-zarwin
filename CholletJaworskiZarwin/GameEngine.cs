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

        private List<WaveResult> wr = new List<WaveResult>();
        private SoldierParameters[] sp;

        private Soldier[] soldier;
        private List<SoldierState> st = new List<SoldierState>();

        private HordeState hs;
        private Wall wall;
        private Horde horde;

        private TurnResult trInit;
        private List<TurnResult> tr = new List<TurnResult>();

        public GameEngine(Parameters parameters)
        {
            this.parameters = parameters;
            sp = parameters.SoldierParameters;

            soldier = new Soldier[sp.Length];
            this.buildingSoldiers(sp, soldier);

            this.refreshingSoldierState(st, soldier);

            wall = new Wall(parameters.CityParameters.WallHealthPoints);
            horde = new Horde(parameters.HordeParameters.Size);
            hs = new HordeState(horde.GetNumberWalkersAlive());

            trInit = new TurnResult(st.ToArray(), hs, wall.Health);
        }


        public Result GameLoop()
        {
            if (soldier.Length == 0)
            {
                Debug.WriteLine("--------------------------------");
                Debug.WriteLine("--------------------------------");
                wr.Add(new WaveResult(trInit, tr.ToArray()));
                return new Result(wr.ToArray());
            }

            tr.Add(trInit);
            while (soldier.Length > 0 && horde.GetNumberWalkersAlive() > 0)
            {

                //soldiers attacking
                this.soldiersAttacking(soldier, horde);

                //Horde Attacking
                this.hordeDoingDomages(parameters, horde, soldier);


                //refreshing all xxxxState
                this.refreshingSoldierState(st, soldier);
                hs = new HordeState(horde.GetNumberWalkersAlive());

                //completing new turn with new xxxxState
                tr.Add(new TurnResult(st.ToArray(), hs, wall.Health));
            }

            //pour le premier test qu'une vague donc a generaliser !
            wr.Add(new WaveResult(trInit, tr.ToArray()));

            return new Result(wr.ToArray());
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
