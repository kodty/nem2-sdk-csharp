namespace io.nem2.sdk.src.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicSupplyRevocationTransaction : Transaction
    {
        public MosaicSupplyRevocationTransaction(string issuerAddress, Tuple<string, ulong> revokedMosaicAmount, bool embedded) : base (embedded)
        {
            IssuerAddress = issuerAddress;
            RevokedMosaicAmount = revokedMosaicAmount;
        }

        public string IssuerAddress { get; set; }
        public Tuple<string, ulong> RevokedMosaicAmount {get; set;}
    }
}
