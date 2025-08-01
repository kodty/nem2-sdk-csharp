using CopperCurve;
using io.nem2.sdk.src.Model.Articles;

namespace io.nem2.sdk.src.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicSupplyChangeTransaction1 : Transaction
    {
        public MosaicSupplyChangeTransaction1(ulong delta, string mosaicId, MosaicSupplyType.Type supplyType, bool embedded) : base(embedded)
        {
            MosaicId = mosaicId.FromHex();
            Delta = delta;
            SupplyType = supplyType.GetValue();
        }

        public byte[] MosaicId { get; set; }

        public ulong Delta { get; set; }

        public byte SupplyType { get; set; }
    }
}
