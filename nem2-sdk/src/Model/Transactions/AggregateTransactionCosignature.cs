using io.nem2.sdk.src.Model.Accounts;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class AggregateTransactionCosignature
    {
        public string Signature { get; }

        public PublicAccount Signer { get; }

        public AggregateTransactionCosignature(string signature, PublicAccount signer)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
            Signer = signer ?? throw new ArgumentNullException(nameof(signer));
        }
    }
}
