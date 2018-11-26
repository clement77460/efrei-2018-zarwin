using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using Zarwin.Shared.Contracts.Input;
using Zarwin.Shared.Contracts.Output;

namespace CholletJaworskiZarwin
{
    class Simulation
    {
        public ObjectId Id { get; set; }
        public Parameters parameter { get; set; }
        public List<TurnResult> turnResults { get; set; }
        public List<WaveResult> waveResults { get; set; }

        public Simulation(Parameters parameter, List<TurnResult> turnResults, List<WaveResult> waveResults)
        {
            this.parameter = parameter;
            this.turnResults = turnResults;
            this.waveResults = waveResults;
        }
        
        public Simulation()
        {
            this.turnResults = new List<TurnResult>();
            this.waveResults = new List<WaveResult>();
        }

        public void addTurnResult(SoldierState[] soldiers, HordeState horde,
            int wallHealthPoints,int money)
        {
            this.turnResults.Add(new TurnResult(soldiers, horde, wallHealthPoints, money));

        }
    }
}
