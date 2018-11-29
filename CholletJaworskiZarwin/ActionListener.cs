using System;
using System.Collections.Generic;
using System.Text;

namespace CholletJaworskiZarwin
{
    class ActionListener
    {
        public void SaveActionTrigger(ActionTrigger actionTrigger)
        {
            actionTrigger.onEndTurnOrWave += new ActionTrigger.BreakHandler(SleepMe);
            actionTrigger.onSoldierEvent += new ActionTrigger.MessageHandler(DisplayMessage);
        }

        private void SleepMe(ActionTrigger actionTrigger, ParameterEventArgs e)
        {

            System.Console.WriteLine(e.message);
            System.Console.WriteLine("Sleeping :"+e.SleepTime/1000+"s");
            System.Threading.Thread.Sleep(e.SleepTime);
            this.PressEnter();
        }

        private void DisplayMessage(ActionTrigger actionTrigger, ParameterEventArgs e)
        {
            System.Console.WriteLine(e.message);
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
