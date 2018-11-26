using System;
using System.Collections.Generic;
using System.Text;

namespace CholletJaworskiZarwin
{
    class ActionListener
    {
        public void SaveActionTrigger(ActionTrigger actionTrigger)
        {
            actionTrigger.onEndTurnOrWave += new ActionTrigger.BreakHandler(sleepMe);
        }

        private void sleepMe(ActionTrigger actionTrigger, ParameterEventArgs e)
        {
            System.Console.WriteLine("Fin d'un tour ou d'une wave, ajustement du temps de sleep "+e.SleepTime/1000 +" s");
            System.Threading.Thread.Sleep(e.SleepTime);
        }
    }
}
