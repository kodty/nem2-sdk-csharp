namespace io.nem2.sdk.src.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : Transaction
    {
        public LockFundsTransaction(Tuple<string, ulong> mosaic, ulong duration, string transactionHash, bool embedded) : base(embedded)
        {
            Mosaic = mosaic;
            Duration = duration;
            TransactionHash = transactionHash;
        }
        public Tuple<string, ulong> Mosaic { get; internal set; }

        public ulong Duration { get; internal set; }

        public string TransactionHash { get; internal set; }
    }
}
