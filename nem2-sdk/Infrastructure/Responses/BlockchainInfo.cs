namespace io.nem2.sdk.Infrastructure.Responses
{
    public class BlockchainInfo
    {
        public int Height { get; set; }

        public ulong ScoreHigh { get; set; }

        public ulong ScoreLow { get; set; }

        public LatestFinalizedBlock LatestFinalizedBlock { get; set; }

    }
    public class LatestFinalizedBlock
    {
        public int FinalizationEpoch { get; set; }

        public int Height { get; set; }

        public string Hash { get; set; }
    }
}
