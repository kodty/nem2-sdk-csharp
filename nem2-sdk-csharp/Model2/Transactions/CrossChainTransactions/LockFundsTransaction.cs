using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.nem2.sdk.Model2.Transactions.CrossChainTransactions
{
    public class LockFundsTransaction1 : Transaction1
    {
        public LockFundsTransaction1(Tuple<string, ulong> mosaic, ulong duration, string transactionHash)
        {
            Mosaic = mosaic;
            Duration = duration;
            TransactionHash = transactionHash;
        }
        public Tuple<string, ulong> Mosaic { get; internal set; }

        public ulong Duration { get; internal set; }

        public string TransactionHash { get; internal set; }
    }
}
