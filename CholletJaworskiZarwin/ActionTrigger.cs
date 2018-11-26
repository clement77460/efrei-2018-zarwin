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

        private bool isTesting;

        public ActionTrigger(bool isTesting)
        {
            this.isTesting = isTesting;
        }

        public void EndTurnTime()
        {

            e.SleepTime = 1000;
            this.SendSignalToListener();
        }

        public void EndWaveTime()
        {
            e.SleepTime = 3000;
            this.SendSignalToListener();
        }

        private void SendSignalToListener()
        {
            if (!isTesting) 
                onEndTurnOrWave?.Invoke(this, e);
        }

    }
}
