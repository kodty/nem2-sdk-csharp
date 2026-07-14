using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregateTransaction : VerifiableTransaction
    {
        public AggregateTransaction(string transactionsHash, UnsignedTransaction[] embeddedTransactions, byte[] cosignatures, TransactionTypes.Types type) : base(type, false)
        {
            TransactionsHash = transactionsHash.FromHex();
            EmbeddedTransactions = Combine(embeddedTransactions.ToList().Select(e => { return e.Payload; }).ToArray());
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

        private static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;

            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}