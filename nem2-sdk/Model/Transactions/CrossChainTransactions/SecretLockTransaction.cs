using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Transactions
{
    public class SecretLockTransaction : TransactionExtension
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

        public SecretLockTransaction(string mosaic, ulong amount, ulong duration, string secret, HashType.Types hashAlgo, string recipient)
        {
            Mosaic = mosaic.FromHex().Reverse().ToArray();
            Amount = amount;
            Duration = duration;
            Secret = secret.FromHex();
            HashAlgo = hashAlgo.GetHashTypeValue();
            Recipient = recipient.IsBase32()
                      ? Address.DecodeAddress(recipient)
                      : recipient.FromHex();
        }

        public byte[] Mosaic { get; set; }

        public ulong Amount { get; set; }

        public ulong Duration { get; set; }

        public byte[] Secret { get; set; }

        public byte HashAlgo { get; set; }

        public byte[] Recipient { get; set; }

        internal override int AddSize()
        {
            return 81;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.SECRET_LOCK;
        }
    }
}
