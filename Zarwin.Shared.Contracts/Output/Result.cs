using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace Zarwin.Shared.Contracts.Output
{
    public class Result
    {
        [JsonProperty("waves")]
        public WaveResult[] Waves { get; }
        
        public Result(params WaveResult[] waves)
        {
            Waves = waves;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Result;

            if (other == null)
                return false;

            if (this.Waves.Length != other.Waves.Length)
                return false;

            return AreAllWavesEqual(this.Waves, other.Waves);
        }

        private static bool AreAllWavesEqual(WaveResult[] theseWaves, WaveResult[] theirWaves)
        {
            return theseWaves.Zip(theirWaves, AreWavesEqual)
                .All(result => result);
        }

        private static bool AreWavesEqual(WaveResult thisWave, WaveResult otherWave)
        {
            return thisWave.Equals(otherWave);
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

            int waveIndex = 1;
            foreach (var wave in Waves)
            {
                builder.AppendLine($"== Wave #{waveIndex++} == ");
                builder.AppendLine(wave.ToString());
            }

            return builder.ToString();
        }
    }
}
