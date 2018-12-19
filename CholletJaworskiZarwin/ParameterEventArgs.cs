using System;
using System.Collections.Generic;
using System.Text;

namespace CholletJaworskiZarwin
{
    public class ParameterEventArgs : EventArgs
    {
        public int SleepTime { get;  set; }
        public String message { get; set; }
        
        public Simulation simulationToSave { get; set; }

        public DataSource ds { get; set; }

    }
}
