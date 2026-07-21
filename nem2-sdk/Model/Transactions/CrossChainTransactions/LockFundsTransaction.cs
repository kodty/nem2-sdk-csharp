using Coppery;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : VerifiableTransaction
    {
        public LockFundsTransaction(TransactionTypes.Types type, bool isEmbedded) : base(type, isEmbedded)
        {

        }

        public LockFundsTransaction(string mosaic, ulong amount, ulong duration, string transactionHash, bool isEmbedded) : base(TransactionTypes.Types.HASH_LOCK, isEmbedded)
        {
            Size += 48;

            Mosaic = mosaic.FromHex().Reverse().ToArray();
            Amount = amount;
            Duration = duration;
            TransactionHash = transactionHash.FromHex();

            Size += (uint)Mosaic.Length;
        }

        [Order(12)]
        public byte[] Mosaic { get; set; }

        [Order(13)]
        public ulong Amount { get; set; }

        [Order(14)]
        public ulong Duration { get; set; }

        [Order(15)]
        public byte[] TransactionHash { get; set; }

        public override LockFundsTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }
    }
}
