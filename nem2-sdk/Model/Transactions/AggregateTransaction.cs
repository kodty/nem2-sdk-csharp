using Coppery;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregatePayload : TransactionExtension
    {
        public byte[] TransactionsHash { get; set; }

        public uint PayloadSize { get; set; }

        public UnsignedTransaction[] EmbeddedTransactions { get; set; }

        public SignedTransaction[] Cosignatures = [];

        internal bool IsComplete { get; set; }


        public void AddCosignatures(SignedTransaction[] cosignatures, SimpleTransaction<AggregatePayload> aggregate)
        {
            Cosignatures = cosignatures;
   
            aggregate.Size += (uint)((8 + 32 + 64) * Cosignatures.Count());
        }

        public AggregatePayload(Transaction[] transactions, bool isComplete)
        {
            IsComplete = isComplete;

            EmbeddedTransactions = new UnsignedTransaction[transactions.Count()];

            for (int i = 0; i < transactions.Count(); i++)
            {
                EmbeddedTransactions[i] = transactions[i].Prepare();

                PayloadSize += (uint)EmbeddedTransactions[i].Payload.Length;
            }
 
            TransactionsHash 
                = CalculateMerkleRoot(
                    EmbeddedTransactions.Select(e => Hash(e.VerifiablePayload)).ToArray()
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

        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(TransactionsHash);
            serializer.SerializeProperty(PayloadSize);
            serializer.SerializeProperty(new byte[4]);

            foreach (UnsignedTransaction p in EmbeddedTransactions)
                serializer.SerializeProperty(p.Payload);

            foreach (SignedTransaction c in Cosignatures)
            {
                serializer.SerializeProperty((ulong)0);
                serializer.SerializeProperty(c.Signer.FromHex());
                serializer.SerializeProperty(c.Signature.FromHex());
            }
        }

        internal override int AddSize()
        {
            return 32 + 4 + 4 + (int)PayloadSize;
        }

        internal override byte SetVersion()
        {
            return 0x03;
        }

        internal override TransactionTypes.Types SetType()
        {
            return IsComplete ? TransactionTypes.Types.AGGREGATE_COMPLETE : TransactionTypes.Types.AGGREGATE_BONDED;
        }

        private static byte[] Hash(byte[] data)
        {
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);
            sha3Hasher.BlockUpdate(data, 0, data.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }
    }
}