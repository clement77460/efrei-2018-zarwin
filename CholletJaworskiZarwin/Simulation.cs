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
        public TurnResult turnInit { get; set; }
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

        public void addWaveResult()
        {
            this.waveResults.Add(new WaveResult(turnInit, turnResults.ToArray()));
        }

        public void removeTurnResults()
        {
            this.turnResults.RemoveRange(0, this.turnResults.Count);
        }

        public void createInitTurn(SoldierState[] soldiers, HordeState horde,
            int wallHealthPoints, int money)
        {
            this.turnInit = new TurnResult(soldiers, horde, wallHealthPoints, money);
        }
    }
}
