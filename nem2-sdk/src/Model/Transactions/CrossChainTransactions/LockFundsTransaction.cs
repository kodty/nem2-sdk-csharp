using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : Transaction
    {
        public LockFundsTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded)
        {

        }

        public LockFundsTransaction(Tuple<string, ulong> mosaic, ulong duration, string transactionHash, bool embedded) : base(embedded)
        {
            Mosaic = mosaic;
            Duration = duration;
            TransactionHash = transactionHash.FromHex();
        }

        public Tuple<string, ulong> Mosaic { get; set; }

        public ulong Duration { get; set; }

        public byte[] TransactionHash { get; set; }
    }
}
