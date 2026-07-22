using Coppery;
using System.Reflection;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction : VerifiableTransaction
    {
        public override PropertyInfo[] RetrieveProperties()
        {
            return
            [
                .. BaseProperties,
                GetType().GetProperty("Mosaic"),
                GetType().GetProperty("Amount"),
                GetType().GetProperty("Duration​"),
                GetType().GetProperty("TransactionHash")
            ];
        }

        public LockFundsTransaction(TransactionTypes.Types type, bool isEmbedded) : base(type, isEmbedded)
        {

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
