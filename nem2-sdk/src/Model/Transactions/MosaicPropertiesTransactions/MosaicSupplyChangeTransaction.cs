using CopperCurve;
using io.nem2.sdk.src.Model.Articles;

namespace io.nem2.sdk.src.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicSupplyChangeTransaction1 : Transaction
    {
        public MosaicSupplyChangeTransaction1(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, bool embedded) : base(embedded)
        {
            Delta = delta;
            MosaicId = mosaicId.FromHex();
            SupplyType = supplyType.GetValue();
        }

        public ulong Delta { get; }

        public byte[] MosaicId { get; }

        public byte SupplyType { get; }
    }
}
