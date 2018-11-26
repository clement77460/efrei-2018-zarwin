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
            onEndTurnOrWave?.Invoke(this, e);
        }

    }
}
