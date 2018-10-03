using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using System.Diagnostics;


namespace ChollerJaworskiZarwin.test
{
    class SimulatorImplement : IInstantSimulator
    {
        public Result Run(Parameters parameters)
        {



            int domageToDo = 0; //retour de la fonction a creer
            
            List<WaveResult> wr = new List<WaveResult>();

            SoldierParameters[] sp = parameters.SoldierParameters;

            //building soldiers (one function toDo)
            Soldier[] soldier = new Soldier[sp.Length];
            for (int i = 0; i < sp.Length; i++)
            {
                soldier[i] = new Soldier(sp[i].Id, sp[i].Level);
            }

            //building soldiersState (one function toDO)
            List<SoldierState> st = new List<SoldierState>();
            for (int i = 0; i < sp.Length; i++)
            {
                st.Add(new SoldierState(soldier[i].Id, soldier[i].Level, soldier[i].HealthPoints));
            }
            

            Wall wall = new Wall(parameters.CityParameters.WallHealthPoints);


            Horde horde = new Horde(parameters.HordeParameters.Size);
            HordeState hs = new HordeState(horde.GetNumberWalkersAlive());

            
            

            TurnResult trInit = new TurnResult(st.ToArray(), hs, wall.Health);
            List<TurnResult> tr=new List<TurnResult>();

            if (soldier.Length == 0)
            {
                wr.Add(new WaveResult(trInit, tr.ToArray()));
                return new Result(wr.ToArray());
            }

            tr.Add(trInit);
            while (soldier.Length > 0 && horde.GetNumberWalkersAlive()>0)
            {
                Debug.WriteLine("-----------------------");
                Debug.WriteLine(horde.GetNumberWalkersAlive());
                
                //soldat attaque
                foreach (Soldier s in soldier)
                {
                    if (horde.KillWalker())
                    {
                        s.LevelUp();
                    }
                }

                
                

                //zombie attaque pas besoin pour le premier test a generaliser !!
                for (int i = 0; i < horde.GetNumberWalkersAlive(); i++)
                {
                    domageToDo++;
                }
                parameters.DamageDispatcher.DispatchDamage(domageToDo, soldier);
                domageToDo = 0;


                //refreshing all xxxxState
                st.Clear();
                for (int i = 0; i < sp.Length; i++)
                {
                    st.Add(new SoldierState(soldier[i].Id, soldier[i].Level, soldier[i].HealthPoints));
                }
                hs = new HordeState(horde.GetNumberWalkersAlive());

                //completing new turn with new xxxxState
                tr.Add(new TurnResult(st.ToArray(), hs, wall.Health));
            }

            //pour le premier test qu'une vague donc a generaliser !
            wr.Add(new WaveResult(trInit, tr.ToArray()));
            return new Result(wr.ToArray());
        }
    }
}
