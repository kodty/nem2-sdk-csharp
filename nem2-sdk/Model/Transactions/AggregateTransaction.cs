using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregateTransaction : Transaction
    {
        public AggregateTransaction(string transactionsHash, UnsignedTransaction[] embeddedTransactions, byte[] cosignatures, TransactionTypes.Types type) : base(type, false) {
            TransactionsHash = transactionsHash.FromHex();          
            EmbeddedTransactions = DataConverter.Combine(embeddedTransactions.ToList().Select(e => { return e.Payload; } ).ToArray());
            PayloadSize = (uint)EmbeddedTransactions.Length;
            Cosignatures = cosignatures;
            Aggregate_​transaction_​header_​reserved_​1 = 0;

            Size += 32;
            Size += (uint)EmbeddedTransactions.Length;
            Size += PayloadSize;
            Size += (uint)Cosignatures.Length;
            Size += 4;

            Type = type.GetValue();
        }

        public byte[] TransactionsHash { get; set; }
        public uint PayloadSize { get; set; }
        public uint Aggregate_​transaction_​header_​reserved_​1 { get; set; }
        public byte[] EmbeddedTransactions { get; set; }
        public byte[] Cosignatures { get; set; }
    }
}