using Coppery;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions
{
    
    public class MosaicSupplyChangeTransaction : VerifiableTransaction
    {
        public MosaicSupplyChangeTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicSupplyChangeTransaction(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, bool embedded) : base(embedded)
        {
            MosaicId = mosaicId.FromHex();
            Delta = delta;
            SupplyType = supplyType.GetValue();
            Size += 17;

            
            Type = TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE.GetValue();
        }

        public byte[] MosaicId { get; set; }

        public ulong Delta { get; set; }

        public byte SupplyType { get; set; }
    }
}
