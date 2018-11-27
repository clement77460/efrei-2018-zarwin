using System;
using System.Collections.Generic;
using System.Text;

namespace CholletJaworskiZarwin
{

    

    public class ActionTrigger
    {
        public event BreakHandler onEndTurnOrWave;
        public delegate void BreakHandler(ActionTrigger m, ParameterEventArgs e);
        public ParameterEventArgs e = new ParameterEventArgs(); //peut etre remplacé par une classe : EventArgs

        private DataSource ds;
        private bool isTesting;

        public ActionTrigger(bool isTesting)
        {
            this.isTesting = isTesting;
            this.ds = new DataSource();
        }

        public void EndTurnTime(Simulation simulation)
        {

            e.SleepTime = 1000;
            this.SendSignalToListenerAndSaveResults(simulation);
        }

        public void EndWaveTime(Simulation simulation)
        {
            e.SleepTime = 3000;
            this.SendSignalToListenerAndSaveResults(simulation);
        }

        private void SendSignalToListenerAndSaveResults(Simulation simulation)
        {
            if (!isTesting)
            {
                ds.SaveSimulation(simulation);
                onEndTurnOrWave?.Invoke(this, e);
            }
        }

    }
}
