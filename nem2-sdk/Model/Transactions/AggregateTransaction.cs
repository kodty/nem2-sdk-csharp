using Coppery;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregatePayload : TransactionExtension
    {
        public byte[] TransactionsHash { get; set; }

        public uint PayloadSize { get; set; }

        public UnsignedTransaction[] EmbeddedTransactions { get; set; }

        public byte[][] EmbeddedTransactionsPayload { get; set; }

        public AggregatePayload(Transaction[] transactions)
        {
            EmbeddedTransactions = new UnsignedTransaction[transactions.Count()];
            EmbeddedTransactionsPayload = new byte[transactions.Count()][];

            for (int i = 0; i < transactions.Count(); i++)
            {
                EmbeddedTransactions[i] = transactions[i].Prepare();
                EmbeddedTransactionsPayload[i] = EmbeddedTransactions[i].Payload;
                PayloadSize += (uint)EmbeddedTransactions[i].Payload.Length;
            }
 
            TransactionsHash 
                = CalculateMerkleRoot(
                    EmbeddedTransactions.Select(e => Transaction.Hash(e.VerifiablePayload)).ToArray()
                    );
        }

        private byte[] CalculateMerkleRoot(byte[][] embeddedTransactionHashes)
        {
            var numRemainingHashes = embeddedTransactionHashes.Length;

            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);

            while (1 < numRemainingHashes)
            {

                int i = 0;

                while (i < numRemainingHashes)
                {
                    sha3Hasher.BlockUpdate(embeddedTransactionHashes[i], 0, 32);

                    if (i + 1 < numRemainingHashes)
                    {
                        sha3Hasher.BlockUpdate(embeddedTransactionHashes[i + 1], 0, 32);
                    }
                    else
                    {
                        // duplicate
                        sha3Hasher.BlockUpdate(embeddedTransactionHashes[i], 0, 32);
                        numRemainingHashes += 1;
                    }

                    sha3Hasher.DoFinal(embeddedTransactionHashes[(int)Math.Floor((double)i / 2)]);
                    i += 2;

                    sha3Hasher.Reset();
                }

                numRemainingHashes = (int)Math.Floor((double)numRemainingHashes / 2);
            }

            return embeddedTransactionHashes[0];
        }

        internal byte[] Serialize()
        {
            lock (this)
            {
                DataSerializer serializer = new DataSerializer(8 + PayloadSize, 0);

                serializer.SerializeProperty(PayloadSize);
                serializer.SerializeProperty(new byte[4]);

                foreach (byte[] p in EmbeddedTransactionsPayload)
                    serializer.SerializeProperty(p);

                return serializer.GetBytes()[0];
            }
        }

        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(TransactionsHash);
        }

        internal override int AddSize()
        {
            return (int)PayloadSize + 40;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.AGGREGATE_COMPLETE;
        }

    }

    public class AggregateTransaction<T> : VerifiableTransaction where T : TransactionExtension
    {
        public AggregatePayload Payload { get; set; }

        public AggregateTransaction(AggregatePayload payload, TransactionTypes.Types type, ulong fee)
        {
            
            Payload = payload;
            Size += (uint)Payload.AddSize();
            Type = type.GetValue();
            Version = 0x03;
            Fee = DataConverter.ConvertFrom(fee);
        }

        internal override UnsignedTransaction Prepare()
        {
            var tBytes = base.Serialize(Size - Payload.PayloadSize - 8);

            return new UnsignedTransaction()
            {
                Payload = [.. tBytes[0], .. Payload.Serialize()],
                VerifiablePayload = tBytes[1]
            };
        }

        internal override void Extend(DataSerializer serializer) => Payload.Extend(serializer);

    }
}