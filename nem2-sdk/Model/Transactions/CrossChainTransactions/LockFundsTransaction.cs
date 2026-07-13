using Coppery;
using io.nem2.sdk.Model.Articles;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : Transaction
    {
        public LockFundsTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded)
        {

        }

        public LockFundsTransaction(Mosaic mosaic, ulong duration, string transactionHash, bool embedded) : base(embedded)
        {
            Mosaic = mosaic.MosaicId.HexId.FromHex();
            Duration = duration;
            TransactionHash = transactionHash.FromHex();
            Size += 56;
        }

        public byte[] Mosaic { get; set; }

        public ulong Duration { get; set; }

        public byte[] TransactionHash { get; set; }
    }
}
