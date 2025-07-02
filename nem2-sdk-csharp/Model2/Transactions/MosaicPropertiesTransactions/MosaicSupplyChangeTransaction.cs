using io.nem2.sdk.Model.Mosaics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.nem2.sdk.Model2.Transactions.MosaicPropertiesTransactions
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
