
using Coppery;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class SecretLockTransaction : VerifiableTransaction
    {
        public SecretLockTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public SecretLockTransaction(string mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient, bool embedded) : base(embedded)
        {
            Mosaic = mosaic.FromHex();
            Duration = duration;
            Secret = secret.FromHex();
            HashAlgo = hashAlgo.GetHashTypeValue();
            Recipient = recipient.IsBase32() 
                      ? AddressEncoder.DecodeAddress(recipient) 
                      : recipient.FromHex();

            Size += (uint) (50 + Secret.Length);
            Type = TransactionTypes.Types.SECRET_LOCK.GetValue();
        }

        public byte[] Mosaic { get; set; }

        public ulong Duration { get; set; }

        public byte[] Secret { get; set; }

        public byte HashAlgo { get; set; }

        public byte[] Recipient { get; set; }

        public override SecretLockTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }
    }
}
