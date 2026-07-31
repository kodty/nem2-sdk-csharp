
using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class SecretLockTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Recipient);
            serializer.SerializeProperty(Secret);
            serializer.SerializeProperty(Mosaic);
            serializer.SerializeProperty(Amount);
            serializer.SerializeProperty(Duration);
            serializer.SerializeProperty(HashAlgo);
        }

        public SecretLockTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public SecretLockTransaction(string mosaic, ulong amount, ulong duration, string secret, HashType.Types hashAlgo, string recipient, bool embedded) : base(TransactionTypes.Types.SECRET_LOCK, embedded)
        {
            Version = 0x01;

            Size += 81;

            Mosaic = mosaic.FromHex().Reverse().ToArray();
            Amount = amount;
            Duration = duration;
            Secret = secret.FromHex();
            HashAlgo = hashAlgo.GetHashTypeValue();
            Recipient = recipient.IsBase32()
                      ? AddressEncoder.DecodeAddress(recipient)
                      : recipient.FromHex();
        }

        public byte[] Mosaic { get; set; }

        public ulong Amount { get; set; }

        public ulong Duration { get; set; }

        public byte[] Secret { get; set; }

        public byte HashAlgo { get; set; }

        public byte[] Recipient { get; set; }

        public override SecretLockTransaction SetSigner(string signer)
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
