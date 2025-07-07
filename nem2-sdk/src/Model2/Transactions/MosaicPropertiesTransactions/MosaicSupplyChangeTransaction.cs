using io.nem2.sdk.Model.Mosaics;

namespace io.nem2.sdk.src.Model2.Transactions.MosaicPropertiesTransactions
{
    public class MosaicSupplyChangeTransaction1 : Transaction1
    {
        public MosaicSupplyChangeTransaction1(ulong delta, Tuple<string, ulong> mosaic, MosaicSupplyType.Type supplyType) 
        {
            Delta = delta;
            MosaicId = mosaic;
            SupplyType = supplyType;
        }

        public ulong Delta { get; }

        public Tuple<string, ulong> MosaicId { get; }

        public MosaicSupplyType.Type SupplyType { get; }
    }
}
