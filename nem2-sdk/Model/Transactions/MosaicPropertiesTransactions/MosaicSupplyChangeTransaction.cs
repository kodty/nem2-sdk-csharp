using Coppery;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions
{   
    public class MosaicSupplyChangeTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(Delta);
            serializer.SerializeProperty(SupplyType);
        }

        public MosaicSupplyChangeTransaction(ulong delta, ulong mosaicId, MosaicSupplyType.Type supplyType)
        {
            MosaicId = mosaicId;
            Delta = delta;
            SupplyType = supplyType.GetValue();
        }

        public ulong MosaicId { get; set; }

        public ulong Delta { get; set; }

        public byte SupplyType { get; set; }

        internal override int AddSize()
        {
            return 17;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE;
        }
    }
}
