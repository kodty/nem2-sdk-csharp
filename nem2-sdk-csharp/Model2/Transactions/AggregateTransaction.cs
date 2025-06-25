using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Model2.Transactions
{
    public class AggregateTransaction1 : Transaction1
    {
        public AggregateTransaction1(string transactionsHash, byte[] embeddedTransactions, byte[] cosignatures, TransactionTypes.Types type) : base(type) {
            TransactionsHash = transactionsHash;
            EmbeddedTransactions = embeddedTransactions;
            Cosignatures = cosignatures;
        }

        public string TransactionsHash { get; set; }

        public int TransactionsCount { get; set; }
        public int Aggregate_​transaction_​header_​reserved_​1 { get; set; }
         
        public byte[] EmbeddedTransactions { get; set; }
        public byte[] Cosignatures { get; set; }
    }
}
