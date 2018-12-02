using System;
using System.Collections.Generic;
using System.Text;

namespace CholletJaworskiZarwin
{

    

    public class ActionTrigger
    {
        public event BreakHandler onEndTurnOrWave;
        public delegate void BreakHandler(ActionTrigger m, ParameterEventArgs e);
        public event MessageHandler onSoldierEvent;
        public delegate void MessageHandler(ActionTrigger m, ParameterEventArgs e);
        public ParameterEventArgs e = new ParameterEventArgs(); //peut etre remplacé par une classe : EventArgs

        private DataSource ds;
        private bool isTesting;

        public ActionTrigger(bool isTesting)
        {
            this.isTesting = isTesting;
            this.ds = new DataSource();
            e.ds = this.ds;
        }

        public void EndTurnTime(Simulation simulation,int numberOfWalkers)
        {
           
            e.message="Fin du tour, il reste "+ numberOfWalkers  + " zombies";
            e.SleepTime = 5000;//1
            this.SendSignalToListenerAndSaveResults(simulation);
        }

        public void EndWaveTime(Simulation simulation)
        {
            
            e.message = "Fin de vague";
            e.SleepTime = 5000;//3
            this.SendSignalToListenerAndSaveResults(simulation);
        }

        public void SoldierLosingHp(int soldierID,int damageTaken)
        {
            e.message = "Soldat " + soldierID + " a perdu " + damageTaken + " PV";

            this.SendSoldierSignalToListener();
        }

        public void SoldierDieing(int soldierID)
        {
            e.message = "Soldat " + soldierID + " est tombé";

            this.SendSoldierSignalToListener();
        }

        public void SoldierStriking(Soldier soldier,int number)
        {
            e.message = "Soldat #" + soldier.Id + " a tué " + number + " zombies";

            this.SendSoldierSignalToListener();
        }

        private void SendSignalToListenerAndSaveResults(Simulation simulation)
        {
            if (!isTesting)
            {
                e.simulationToSave = simulation;
                onEndTurnOrWave?.Invoke(this, e);
            }
        }

        private void SendSoldierSignalToListener()
        {
            if (!isTesting)
            {
                
                onSoldierEvent?.Invoke(this, e);
            }
        }

    }
}
