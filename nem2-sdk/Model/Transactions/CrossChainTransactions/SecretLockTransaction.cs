
using Coppery;
using io.nem2.sdk.Utils;
using System.Reflection;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class SecretLockTransaction : VerifiableTransaction
    {
        public override PropertyInfo[] RetrieveProperties()
        {
            return
            [
                .. BaseProperties,
                GetType().GetProperty("Mosaic"),
                GetType().GetProperty("Duration"),
                GetType().GetProperty("Secret​"),
                GetType().GetProperty("HashAlgo"),
                GetType().GetProperty("Recipient")
            ];
        }

        public SecretLockTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public SecretLockTransaction(string mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient, bool embedded) : base(TransactionTypes.Types.SECRET_LOCK, embedded)
        {
            Version = 0x01;
            Mosaic = mosaic.FromHex();
            Duration = duration;
            Secret = secret.FromHex();
            HashAlgo = hashAlgo.GetHashTypeValue();
            Recipient = recipient.IsBase32() 
                      ? AddressEncoder.DecodeAddress(recipient) 
                      : recipient.FromHex();

            Size += (uint) (50 + Secret.Length);
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

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}
