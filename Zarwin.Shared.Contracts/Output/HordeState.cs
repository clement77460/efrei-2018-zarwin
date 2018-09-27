using Newtonsoft.Json;

namespace Zarwin.Shared.Contracts.Output
{
    public class HordeState
    {
        [JsonProperty("size")]
        public int Size { get; }

        public HordeState(int size)
        {
            Size = size;
        }

        public override bool Equals(object obj)
        {
            var other = obj as HordeState;

            if (other == null)
                return false;

            return this.Size == other.Size;
        }

        public override int GetHashCode()
        {
            return Size.GetHashCode();
        }

        public override string ToString()
        {
            return $"Horde Size = {Size}";
        }
    }
}
