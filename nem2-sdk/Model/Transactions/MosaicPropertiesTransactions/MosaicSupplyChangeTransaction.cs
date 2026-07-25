using Coppery;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions
{   
    public class MosaicSupplyChangeTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId, typeof(byte[]), 10);
            serializer.SerializeProperty(Delta, typeof(ulong), 11);
            serializer.SerializeProperty(SupplyType, typeof(byte), 12);
        }

        public MosaicSupplyChangeTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicSupplyChangeTransaction(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, bool embedded) : base(TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE, embedded)
        {
            Version = 0x01;
            MosaicId = mosaicId.FromHex().Reverse().ToArray();
            Delta = delta;
            SupplyType = supplyType.GetValue();
            Size += 17;
        }

        public byte[] MosaicId { get; set; }

        public ulong Delta { get; set; }

        public byte SupplyType { get; set; }

        public override MosaicSupplyChangeTransaction SetSigner(string signer)
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
