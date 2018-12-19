using System;
using Xunit;
using Zarwin.Shared.Contracts;
using Zarwin.Shared.Tests;

namespace CholletJaworskiZarwin.test
{
    public class IntegrationTestImplement : IntegratedTests
    {
        public override IInstantSimulator CreateSimulator()
        {
            return new SimulatorImplement();
        }
    }


}
