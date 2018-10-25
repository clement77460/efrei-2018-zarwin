using System;
using System.Collections.Generic;
using System.Text;
using CholletJaworskiZarwin;
using Zarwin.Shared.Contracts;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;
using System.Diagnostics;
using Xunit;


namespace CholletJaworskiZarwin.test
{
    public class SimulatorImplement : IInstantSimulator
    {

        public Result Run(Parameters parameters)
        {
            Game game = new Game(parameters);

            while (!game.IsFinished())
            {
                game.Turn();
            }

            return game.GetResult();
        }
    }
}
