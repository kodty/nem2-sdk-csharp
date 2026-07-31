using Coppery;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicReclamationTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(DebtorImposed);
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(Amount);
        }

        public MosaicReclamationTransaction(Address debtorImposed, string mosaicId, ulong amount, bool embedded) : base (TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION, embedded)
        {
            Version = 0x01;

            Size += 40;

            DebtorImposed = AddressEncoder.DecodeAddress(debtorImposed.Plain);
            MosaicId = mosaicId.FromHex().Reverse().ToArray();
            Amount = amount;          
        }

        public byte[] DebtorImposed { get; set; }

        public byte[] MosaicId { get; set; }

        public ulong Amount { get; set; }

        public override MosaicReclamationTransaction SetSigner(string signer)
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
