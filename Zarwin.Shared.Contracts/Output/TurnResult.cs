using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace Zarwin.Shared.Contracts.Output
{
    public class TurnResult
    {
        [JsonProperty("soldiers")]
        public SoldierState[] Soldiers { get; }

        [JsonProperty("horde")]
        public HordeState Horde { get; }

        [JsonProperty("wallHealthPoints")]
        public int WallHealthPoints { get; }
        
        public TurnResult(
            SoldierState[] soldiers,
            HordeState horde,
            int wallHealthPoints)
        {
            Soldiers = soldiers;
            Horde = horde;
            WallHealthPoints = wallHealthPoints;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TurnResult;

            if (other == null)
                return false;

            var soldiersFound = from thisSoldier in this.Soldiers
                                join otherSoldier in other.Soldiers on thisSoldier.Id equals otherSoldier.Id
                                select Tuple.Create(thisSoldier, otherSoldier);

            return this.Soldiers.Length == other.Soldiers.Length
                && soldiersFound.Count() == this.Soldiers.Length
                && soldiersFound.All(tuple => tuple.Item1.Equals(tuple.Item2))
                && this.Horde.Equals(other.Horde)
                && this.WallHealthPoints == other.WallHealthPoints;
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
            foreach (var soldier in Soldiers.OrderBy(s => s.Id))
            {
                builder.AppendLine(soldier.ToString());
            }
            builder.AppendLine(Horde.ToString());
            builder.AppendLine($"Wall HP = {WallHealthPoints}");

            return builder.ToString();
        }
    }
}
