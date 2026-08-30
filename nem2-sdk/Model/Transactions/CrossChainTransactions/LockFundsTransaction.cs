using io.nem2.sdk.Infrastructure;

namespace io.nem2.sdk.Model.Transactions
{
    public class LockFundsTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Mosaic);
            serializer.SerializeProperty(Amount);
            serializer.SerializeProperty(Duration);
            serializer.SerializeProperty(TransactionHash);
        }


        public LockFundsTransaction(string mosaic, ulong amount, ulong duration, string transactionHash)
        {
            Mosaic = mosaic.FromHex().Reverse().ToArray();
            Amount = amount;
            Duration = duration;
            TransactionHash = transactionHash.FromHex();
        }

        public byte[] Mosaic { get; set; }

        public ulong Amount { get; set; }

        public ulong Duration { get; set; }

        public byte[] TransactionHash { get; set; }

        internal override int AddSize()
        {
            return 48 + Mosaic.Length;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.HASH_LOCK;
        }
    }
}
