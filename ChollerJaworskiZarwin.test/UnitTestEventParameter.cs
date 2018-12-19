using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;
using Zarwin.Shared.Tests;

namespace CholletJaworskiZarwin.test
{
    public class UnitTestEventParameter
    {
        [Fact]
        public void CreateNewParameterAndVerifySleepTimeAndMessageValue()
        {
            ParameterEventArgs parameterEvent = new ParameterEventArgs();
            parameterEvent.SleepTime = 5;
            parameterEvent.message = "test du sleep";

            Assert.Equal(5, parameterEvent.SleepTime);
            Assert.Equal("test du sleep", parameterEvent.message);
        }
        [Fact]
        public void CreateNewSimulationAndVerifyValue()
        {
            ParameterEventArgs parameterEvent = new ParameterEventArgs();
            var input = new Parameters(
               1,
               null,
               new HordeParameters(2),
               new CityParameters(0, 10),
               new Order[]
               {
                   new Order(0,1,OrderType.RecruitSoldier)
               },
               new SoldierParameters(1, 1));

            Simulation simu = new Simulation(input);
            parameterEvent.simulationToSave = simu;
            Assert.Equal(1, parameterEvent.simulationToSave.isRunning);
            Assert.Single(parameterEvent.simulationToSave.orders);
            Assert.Null(parameterEvent.simulationToSave.turnInit);
            Assert.Empty(parameterEvent.simulationToSave.turnResults);
            Assert.Empty(parameterEvent.simulationToSave.waveResults);
            Assert.Single(parameterEvent.simulationToSave.zombieParameter);
            Assert.Equal(1, simu.wavesToRun);
        }
    }
}
