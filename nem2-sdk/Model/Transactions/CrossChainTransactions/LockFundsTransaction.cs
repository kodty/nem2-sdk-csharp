using Coppery;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Mosaic);
            serializer.SerializeProperty(Amount);
            serializer.SerializeProperty(Duration);
            serializer.SerializeProperty(TransactionHash);
        }


        public LockFundsTransaction(string mosaic, ulong amount, ulong duration, string transactionHash, bool isEmbedded) : base(TransactionTypes.Types.HASH_LOCK, isEmbedded)
        {
            Size += 48;

            Version = 0x01;
            Mosaic = mosaic.FromHex().Reverse().ToArray();
            Amount = amount;
            Duration = duration;
            TransactionHash = transactionHash.FromHex();

            Size += (uint)Mosaic.Length;
        }

        public byte[] Mosaic { get; set; }

        public ulong Amount { get; set; }

        public ulong Duration { get; set; }

        public byte[] TransactionHash { get; set; }

        public override LockFundsTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}
