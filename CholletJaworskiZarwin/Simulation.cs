using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Input.Orders;
using Zarwin.Shared.Contracts.Output;
using Zarwin.Shared.Tests;

namespace CholletJaworskiZarwin
{
    public class Simulation
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("idString")]
        public String IdString { get; set; }
        [BsonElement("turnInit")]
        public TurnResult turnInit { get; set; }
        [BsonElement("turnResults")]
        public List<TurnResult> turnResults { get; set; }
        [BsonElement("waveResults")]
        public List<WaveResult> waveResults { get; set; }
        public List<ZombieParameter[]> zombieParameter { get; set; }
        public List<OrderWrapperMongoDB> orders { get; set; }
        public int wavesToRun { get; set; }
        public int isRunning { get; set; }

        public Simulation(TurnResult turnInit, List<TurnResult> turnResults, List<WaveResult> waveResults, 
            List<ZombieParameter[]> zombieParameter, List<OrderWrapperMongoDB> orders, int wavesToRun)
        {
            
            this.turnInit = turnInit;
            this.turnResults = turnResults;
            this.waveResults = waveResults;
            this.zombieParameter = zombieParameter;
            this.orders = orders;
            this.wavesToRun = wavesToRun;

            this.isRunning = 1;
        }

        public Simulation(Parameters param)
        {
            Random random = new Random();
            this.IdString = random.Next(1, 999999999).ToString();

            orders = new List<OrderWrapperMongoDB>();
            for(int i = 0; i < param.Orders.Length; i++)
            {
                orders.Add(new OrderWrapperMongoDB(param.Orders[i]));
            }

            this.zombieParameter = new List<ZombieParameter[]>();
            for (int i=0;i< param.HordeParameters.Waves.Length; i++)
            {
                this.zombieParameter.Add(param.HordeParameters.Waves[i].ZombieParameters);
            }

            this.wavesToRun = param.WavesToRun;
            this.turnResults = new List<TurnResult>();
            this.waveResults = new List<WaveResult>();

            this.isRunning = 1;
        }

        public void addTurnResult(SoldierState[] soldiers, HordeState horde,
            int wallHealthPoints,int money)
        {
            this.turnResults.Add(new TurnResult(soldiers, horde, wallHealthPoints, money));

        }

        public void addWaveResult()
        {
            this.waveResults.Add(new WaveResult(turnInit, turnResults.ToArray()));
        }

        public void removeTurnResults()
        {
            this.turnResults.RemoveRange(0, this.turnResults.Count);
            this.turnInit = null;
        }

        public void createInitTurn(SoldierState[] soldiers, HordeState horde,
            int wallHealthPoints, int money)
        {
            this.turnInit = new TurnResult(soldiers, horde, wallHealthPoints, money);
        }

        public Parameters CreateParametersFromOldSimulation()
        {

            return new Parameters(
                  this.wavesToRun - waveResults.Count,
                  new FirstSoldierDamageDispatcher(),
                  this.BuildHordeParameter(),
                  this.BuildCityParameter(),
                  this.BuildOrdersParameters(),
                  this.BuildSoldierParameters());

        }

        private HordeParameters BuildHordeParameter()
        {
            List<WaveHordeParameters> hordeParameters = new List<WaveHordeParameters>();
            for(int i=0;i< zombieParameter.Count; i++)
            {
                hordeParameters.Add(new WaveHordeParameters(zombieParameter[i]));
            }

            return new HordeParameters(hordeParameters.ToArray());

        }

        private CityParameters BuildCityParameter()
        {
            int indexTurn = turnResults.Count-1;
            if (indexTurn >= 0)
            {
                //on Prend le dernier TurnResult
                return new CityParameters(turnResults[indexTurn].WallHealthPoints, turnResults[indexTurn].Money);
            }

            //Pas de tours donc on prend le tour de la derniere WaveResult
            int indexWave = waveResults.Count - 1;
            indexTurn = waveResults[indexWave].Turns.Length - 1;
            return new CityParameters(waveResults[indexWave].Turns[indexTurn].WallHealthPoints, waveResults[indexWave].Turns[indexTurn].Money);
        }

        private SoldierParameters[] BuildSoldierParameters()
        {
            List<SoldierParameters> soldierParameters = new List<SoldierParameters>();

            int indexTurn = turnResults.Count-1;
            if (indexTurn >= 0)
            {
                for(int i = 0; i < turnResults[indexTurn].Soldiers.Length; i++)
                {
                    soldierParameters.Add(new SoldierParameters(turnResults[indexTurn].Soldiers[i].Id, turnResults[indexTurn].Soldiers[i].Level));
                    System.Console.WriteLine("un soldat converti");
                }

                System.Console.WriteLine(soldierParameters[0].Level);
                return soldierParameters.ToArray();
            }

            int indexWave = waveResults.Count - 1;
            indexTurn = waveResults[indexWave].Turns.Length - 1;
            for (int i = 0; i < waveResults[indexWave].Turns[indexTurn].Soldiers.Length; i++)
            {
                soldierParameters.Add(new SoldierParameters(waveResults[indexWave].Turns[indexTurn].Soldiers[i].Id, 
                    waveResults[indexWave].Turns[indexTurn].Soldiers[i].Level));
            }
            return soldierParameters.ToArray();
        }

        private Order[] BuildOrdersParameters()
        {
            List<Order> orderParameter = new List<Order>();

            for(int i = 0; i < orders.Count; i++)
            {
                orderParameter.Add(orders[i].ToOrder());
            }

            return orderParameter.ToArray();
        }
    }
}
