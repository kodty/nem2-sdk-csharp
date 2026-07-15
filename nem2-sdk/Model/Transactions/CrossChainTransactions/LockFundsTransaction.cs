using Coppery;
using io.nem2.sdk.Model.Articles;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : VerifiableTransaction
    {
        public LockFundsTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded)
        {

        }

        public LockFundsTransaction(string mosaic, ulong duration, string transactionHash, bool embedded) : base(embedded)
        {
            Mosaic = mosaic.FromHex();
            VerifiableEntity.Size += (uint)Mosaic.Length;

            Duration = duration;
            VerifiableEntity.Size += 8;

            TransactionHash = transactionHash.FromHex();
            VerifiableEntity.Size += 32;
        }

        public byte[] Mosaic { get; set; }

        public ulong Duration { get; set; }

        public byte[] TransactionHash { get; set; }

        public override LockFundsTransaction SetSigner(string signer)
        {
            EntityBody.Signer = signer.FromHex();

            return this;
        }
    }
}
