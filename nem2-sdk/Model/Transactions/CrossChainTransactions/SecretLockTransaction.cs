
using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class SecretLockTransaction : Transaction
    {
        public SecretLockTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public SecretLockTransaction(Tuple<string, ulong> mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient, bool embedded) : base(embedded)
        {
            Mosaic = mosaic;
            Duration = duration;
            Secret = secret.FromHex();
            HashAlgo = hashAlgo.GetHashTypeValue();
            Recipient = recipient.IsBase32() 
                      ? AddressEncoder.DecodeAddress(recipient) 
                      : recipient.FromHex();

            
            Type = TransactionTypes.Types.SECRET_LOCK.GetValue();
        }

        public Tuple<string, ulong> Mosaic { get; set; }

        public ulong Duration { get; set; }

        public byte[] Secret { get; set; }

        public byte HashAlgo { get; set; }

        public byte[] Recipient { get; set; }
    }
}
