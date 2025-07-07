
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Export;

namespace io.nem2.sdk.src.Model2.Transactions.CrossChainTransactions
{
    public class SecretLockTransaction1 : Transaction1
    {
        public SecretLockTransaction1(Tuple<string, ulong> mosaic, ulong duration, string secret, HashType.Types hashAlgo, string recipient)
        {
            Mosaic = mosaic;
            Duration = duration;
            Secret = secret.FromHex();
            HashAlgo = hashAlgo;
            Recipient = recipient;
        }

        public Tuple<string, ulong> Mosaic { get; internal set; }

        public ulong Duration { get; internal set; }

        public byte[] Secret { get; internal set; }

        public HashType.Types HashAlgo { get; internal set; }

        public string Recipient { get; internal set; }
    }
}
