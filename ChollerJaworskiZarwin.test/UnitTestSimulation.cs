using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;
using Zarwin.Shared.Tests;

namespace CholletJaworskiZarwin.test
{
    public class UnitTestSimulation
    {

        [Fact]
        public void CreateNewSimulation()
        {
            var input = new Parameters(
               1,
               new FirstSoldierDamageDispatcher(),
               new HordeParameters(2),
               new CityParameters(0,10),
               new Order[]
               {
                   new Order(0,1,OrderType.RecruitSoldier)
               },
               new SoldierParameters(1, 1));

            Simulation simu = new Simulation(input);
            Assert.Equal(1, simu.isRunning);
            Assert.Single(simu.orders);
            Assert.Null(simu.turnInit);
            Assert.Empty(simu.turnResults);
            Assert.Empty(simu.waveResults);
            Assert.Single(simu.zombieParameter);
            Assert.Equal(1, simu.wavesToRun);
        }

        [Fact]
        public void ConvertSimulationIntoParameterAfterTheGameEnd()
        {
            var input = new Parameters(
               2,
               new FirstSoldierDamageDispatcher(),
               new HordeParameters(2),
               new CityParameters(0, 10),
               new Order[]
               {
                   new Order(0,1,OrderType.RecruitSoldier),
                   new Equipment(0, 1, OrderType.EquipWithShotgun, 1),
                   new Medipack(0, 1, 1, 3)
               },
               new SoldierParameters(1, 1));

            Simulation simu = new Simulation(input);
            var idSimulation = simu.Id;

            Game game = new Game(input);

            var newParam = game.simulation.CreateParametersFromOldSimulation();

            Assert.Equal(0, newParam.WavesToRun);
            Assert.Equal(OrderType.RecruitSoldier, newParam.Orders[0].Type);
            Assert.Equal(idSimulation, game.simulation.Id);

            game = new Game(newParam);
            Assert.Equal(2, game.GetResult().Waves[0].Turns[0].Soldiers.Length);

        }
    }
}
