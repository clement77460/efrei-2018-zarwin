using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace Zarwin.Shared.Contracts.Output
{
    public class WaveResult
    {
        [JsonProperty("initialState")]
        public TurnResult InitialState { get; }

        [JsonProperty("turns")]
        public TurnResult[] Turns { get; }

        public WaveResult(TurnResult initialState, params TurnResult[] turns)
        {
            InitialState = initialState;
            Turns = turns;
        }

        public override bool Equals(object obj)
        {
            var other = obj as WaveResult;

            if (other == null)
                return false;

            if (!AreTurnsEqual(this.InitialState, other.InitialState))
                return false;

            if (this.Turns.Length != other.Turns.Length)
                return false;

            return AreAllTurnsEqual(this.Turns, other.Turns);
        }

        private static bool AreAllTurnsEqual(TurnResult[] theseTurns, TurnResult[] theirTurns)
        {
            return theseTurns.Zip(theirTurns, AreTurnsEqual)
                .All(result => result);
        }

        private static bool AreTurnsEqual(TurnResult thisTurn, TurnResult otherTurn)
        {
            return thisTurn.Equals(otherTurn);
        }

        public override int GetHashCode()
        {
            // This is not really good in terms of performance, 
            // but this class should not really be hashed anyway
            return 0;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Initial turn");
            builder.AppendLine(InitialState.ToString());

            int turnIndex = 1;
            foreach (var turn in Turns)
            {
                builder.AppendLine($"Turn #{turnIndex++}");
                builder.AppendLine(turn.ToString());
            }

            return builder.ToString();
        }
    }
}
