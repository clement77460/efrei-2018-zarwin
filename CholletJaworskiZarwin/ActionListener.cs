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
            e.ds.SaveSimulation(e.simulationToSave);

            System.Console.WriteLine(e.message);
            System.Console.WriteLine("Sleeping :"+e.SleepTime/1000+"s");
            System.Threading.Thread.Sleep(e.SleepTime);

            if (!this.PressEnter()) {//on quitte la partie et change le status de la game
                e.ds.UpdateRunningStatus(e.simulationToSave, 0);
                Environment.Exit(0);
            }
        }

        private void DisplayMessage(ActionTrigger actionTrigger, ParameterEventArgs e)
        {
            System.Console.WriteLine(e.message);
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private bool PressEnter()
        {
            Console.WriteLine("[Fin du sleep] Press Enter to continue or q to Quit...");
            ConsoleKeyInfo c;
            do
            {
                c = Console.ReadKey();

            } while (c.Key != ConsoleKey.Enter && c.Key.ToString()!="Q");


            if (c.Key.ToString() == "Q")
            {
                return false;
                //Cette solution car essai non concluant avec AppDomain.CurrentDomain.ProcessExit += fonction :
                //on update la simulation a isRunning =0;
            }
            return true;
        }
    }
}
