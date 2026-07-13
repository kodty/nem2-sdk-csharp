using Coppery;
using io.nem2.sdk.Model.Articles;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : Transaction
    {
        public LockFundsTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded)
        {

        }

        public LockFundsTransaction(string mosaic, ulong duration, string transactionHash, bool embedded) : base(embedded)
        {
            Mosaic = mosaic.FromHex();
            Size += (uint)Mosaic.Length;

            Duration = duration;
            Size += 8;

            TransactionHash = transactionHash.FromHex();
            Size += 32;
        }

        public byte[] Mosaic { get; set; }

        public ulong Duration { get; set; }

        public byte[] TransactionHash { get; set; }
    }
}
