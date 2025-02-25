using io.nem2.sdk.Core.Utils;
using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class BlockchainInfo
    {
        public int Height { get; set; }

        [JsonProperty("scoreHigh")]
        public ulong ScoreHigh { get; set; }

        [JsonProperty("scoreLow")]
        public ulong ScoreLow { get; set; }

        /*
        public ulong Score => ExtractScore();

        internal ulong ExtractScore()
        {
            return new[] { BitConverter.ToInt32(BitConverter.GetBytes(ScoreLow), 0), BitConverter.ToInt32(BitConverter.GetBytes(ScoreHigh), 0) }.FromInt32Array();
        }
        */
        [JsonProperty("latestFinalizedBlock")]
        public LatestFinalizedBlock LatestFinalizedBlock { get; set; }

    }
    public class LatestFinalizedBlock
    {
        [JsonProperty("finalizationPoint")]
        public int FinalizationEpoch { get; set; }


        [JsonProperty("height")]
        public int Height { get; set; }


        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
