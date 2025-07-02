using io.nem2.sdk.src.Model2.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.nem2.sdk.src.Model2.Transactions.MosaicPropertiesTransactions
{
    public class MosaicSupplyRevocationTransaction1 : Transaction1
    {
        public MosaicSupplyRevocationTransaction1(string issuerAddress, Tuple<string, ulong> revokedMosaicAmount)
        {
            IssuerAddress = issuerAddress;
            RevokedMosaicAmount = revokedMosaicAmount;
        }

        public string IssuerAddress { get; set; }
        public Tuple<string, ulong> RevokedMosaicAmount {get; set;}
    }
}
