using Coppery;
using io.nem2.sdk.Model.Accounts;
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
                GetType().GetProperty("MosaicId"),
                GetType().GetProperty("Amount")
            ];
        }

        public MosaicSupplyRevocationTransaction(Address issuerAddress, string mosaicId, ulong amount, bool embedded) : base (TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION, embedded)
        { 
            IssuerAddress = AddressEncoder.DecodeAddress(issuerAddress.Plain);
            MosaicId = mosaicId.FromHex();
            Amount = amount;

            Size += (uint)IssuerAddress.Length;
            Size += 16;
        }

        public byte[] IssuerAddress { get; set; }

        public byte[] MosaicId { get; set; }

        public ulong Amount { get; set; }

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
