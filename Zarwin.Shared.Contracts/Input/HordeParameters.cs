using Newtonsoft.Json;

namespace Zarwin.Shared.Contracts.Input
{
    public class HordeParameters
    {
        [JsonProperty("size")]
        public int Size { get; }

        public HordeParameters(int size)
        {
            Size = size;
        }
    }
}
