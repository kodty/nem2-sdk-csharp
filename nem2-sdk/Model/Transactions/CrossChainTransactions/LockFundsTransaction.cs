using Coppery;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : Transaction
    {
        public LockFundsTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded)
        {

        }

        public LockFundsTransaction(Tuple<string, ulong> mosaic, ulong duration, string transactionHash, bool embedded) : base(embedded)
        {
            Mosaic = new Tuple<byte[], ulong>(mosaic.Item1.FromHex(), mosaic.Item2);
            Duration = duration;
            TransactionHash = transactionHash.FromHex();
            Size += 56;

            
            Type = TransactionTypes.Types.HASH_LOCK.GetValue();
        }

        public Tuple<byte[], ulong> Mosaic { get; set; }

        public ulong Duration { get; set; }

        public byte[] TransactionHash { get; set; }
    }
}
