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
            GameEngine ge = new GameEngine(parameters);
            return ge.GameLoop();
        }
    }
}
