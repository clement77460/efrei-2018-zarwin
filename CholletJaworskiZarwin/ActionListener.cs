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
            
            System.Console.WriteLine("[Fin tour / Wave] sleep: "+e.SleepTime/1000 +" s");
            System.Threading.Thread.Sleep(e.SleepTime);
            this.PressEnter();
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private void PressEnter()
        {
            Console.WriteLine("[Fin du sleep] Press Enter to continue...");
            ConsoleKeyInfo c;
            do
            {
                c = Console.ReadKey();

            } while (c.Key != ConsoleKey.Enter);
        }
    }
}
