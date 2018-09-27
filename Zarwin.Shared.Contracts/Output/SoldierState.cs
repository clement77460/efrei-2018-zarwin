using Newtonsoft.Json;

namespace Zarwin.Shared.Contracts.Output
{
    public class SoldierState
    {
        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("level")]
        public int Level { get; }

        [JsonProperty("healthPoints")]
        public int HealthPoints { get; }

        public SoldierState(int id, int level, int healthPoints)
        {
            Id = id;
            Level = level;
            HealthPoints = healthPoints;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SoldierState;

            if (other == null)
                return false;

            return Id == other.Id
                && Level == other.Level
                && HealthPoints == other.HealthPoints;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Soldier #{Id} : Level={Level}, HP={HealthPoints}";
        }
    }
}
