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
            
            SoldierParameters[] sp = parameters.SoldierParameters;
            Soldier[] soldier = new Soldier[1];
            soldier[0]= new Soldier(sp[0].Id, sp[0].Level);
            SoldierState[] st=new SoldierState[1];
            st[0] =new SoldierState(soldier[0].Id, soldier[0].Level, soldier[0].HealthPoints);

            Wall wall = new Wall(parameters.CityParameters.WallHealthPoints);


            Horde horde = new Horde(parameters.HordeParameters.Size);
            HordeState hs = new HordeState(horde.GetNumberWalkersAlive());

            
            

            TurnResult[] tr = new TurnResult[2];
            tr[0]= new TurnResult(st, hs, wall.Health);

            //soldat attaque
            foreach (Soldier s in soldier)
            {
                if (horde.KillWalker())
                {
                    s.LevelUp();
                }
            }
            //zombie attaque pas besoin pour le premier test a generaliser !!

            //parameters.DamageDispatcher.DispatchDamage(1, soldier);

            //permet de voir du print quand on execute le test en débogage 
            Debug.WriteLine("---------------------------------");
            Debug.WriteLine(horde.GetNumberWalkersAlive());
            Debug.WriteLine(soldier[0].HealthPoints);

            //refreshing all xxxxState
            st[0] = new SoldierState(soldier[0].Id, soldier[0].Level, soldier[0].HealthPoints);
            hs = new HordeState(horde.GetNumberWalkersAlive());

            //completing new turn with new xxxxState
            tr[1]=new TurnResult(st, hs, wall.Health);
            
            //pour le premier test qu'une vague donc a generaliser !
            WaveResult[] wr = new WaveResult[1];
            wr[0] = new WaveResult(tr[0], tr);

            return new Result(wr);
        }
    }
}
