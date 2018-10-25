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

        [JsonProperty("money")]
        public int Money { get; }

        public TurnResult(
            SoldierState[] soldiers,
            HordeState horde,
            int wallHealthPoints,
            int money)
        {
            Soldiers = soldiers;
            Horde = horde;
            Money = money;
            WallHealthPoints = wallHealthPoints;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TurnResult other))
                return false;

            var soldiersFound = from thisSoldier in Soldiers
                                join otherSoldier in other.Soldiers on thisSoldier.Id equals otherSoldier.Id
                                select Tuple.Create(thisSoldier, otherSoldier);

            return Soldiers.Length == other.Soldiers.Length
                && soldiersFound.Count() == Soldiers.Length
                && soldiersFound.All(tuple => tuple.Item1.Equals(tuple.Item2))
                && Horde.Equals(other.Horde)
                && Money == other.Money
                && WallHealthPoints == other.WallHealthPoints;
        }

        // This is not really good in terms of performance, 
        // but this class should not really be hashed anyway
        public override int GetHashCode() => 0;

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
