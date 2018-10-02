using System;
using Xunit;
using Zarwin.Shared.Contracts;
using Zarwin.Shared.Tests;

namespace ChollerJaworskiZarwin.test
{
    public class UnitTest1:IntegratedTests
    {
        public override IInstantSimulator CreateSimulator 
            => new SimulatorImplement();

        [Fact]
        public void Test1()
        {
            new SimulatorImplement();
        }
    }
}
