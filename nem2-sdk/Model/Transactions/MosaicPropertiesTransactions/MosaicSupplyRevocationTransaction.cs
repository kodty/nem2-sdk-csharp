using Coppery;
using io.nem2.sdk.Utils;
using System.Reflection;

namespace io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicSupplyRevocationTransaction : VerifiableTransaction
    {
        public override PropertyInfo[] RetrieveProperties()
        {
            return
            [
                .. BaseProperties,
                GetType().GetProperty("IssuerAddress"),
                GetType().GetProperty("RevokedMosaicAmount")
            ];
        }

        public MosaicSupplyRevocationTransaction(string issuerAddress, Tuple<string, ulong> revokedMosaicAmount, bool embedded) : base (TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION, embedded)
        {
            IssuerAddress = issuerAddress.IsBase32() ? AddressEncoder.DecodeAddress(issuerAddress) : issuerAddress.FromHex(); ;
            RevokedMosaicAmount = revokedMosaicAmount;
        }

        public byte[] IssuerAddress { get; set; }

        public Tuple<string, ulong> RevokedMosaicAmount {get; set;}

        public override MosaicSupplyRevocationTransaction SetSigner(string signer)
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
