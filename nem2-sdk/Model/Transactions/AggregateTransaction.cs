using Coppery;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregatePayload
    {
        public byte[] TransactionsHash { get; set; }

        public uint PayloadSize { get; set; }

        public UnsignedTransaction[] EmbeddedTransactions { get; set; }

        public byte[][] EmbeddedTransactionsPayload { get; set; }

        public AggregatePayload(Transaction[] transactions)
        {
            EmbeddedTransactions = transactions.Select(t => t.Prepare()).ToArray();

            TransactionsHash = CalculateMerkleRoot(EmbeddedTransactions.Select(e => Transaction.Hash(e.VerifiablePayload)).ToArray());

            EmbeddedTransactionsPayload = Pad(EmbeddedTransactions);

            PayloadSize = (uint)EmbeddedTransactionsPayload.Sum(e => e.Length);
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

        private byte[][] Pad(UnsignedTransaction[] embeddedTransactions)
        {
            uint bufPadding = 0;

            var pPayloads = embeddedTransactions.ToList().Select(item =>
            {
                if (item.Payload.Length % 8 != 0)
                {
                    var paddedPayload = new byte[(int)(Math.Ceiling((decimal)item.Payload.Length / 8) * 8)];

                    var size = DataConverter.ConvertTo<uint>(item.Payload.Take(4).ToArray());

                    Buffer.BlockCopy(item.Payload, 4, paddedPayload, 4, item.Payload.Length - 4);

                    var s = (uint)(paddedPayload.Length - item.Payload.Length);

                    size += s;

                    bufPadding += s;

                    Buffer.BlockCopy(DataConverter.ConvertFrom(size), 0, paddedPayload, 0, 4);

                    return paddedPayload;
                }
                else return item.Payload;

            }).ToArray();

            return pPayloads;
        }
    }

    public class AggregateTransaction : VerifiableTransaction
    {
        public AggregatePayload Payload { get; set; }

        public AggregateTransaction(AggregatePayload payload, TransactionTypes.Types type) : base(TransactionTypes.Types.AGGREGATE_COMPLETE, false)
        {
            Version = 0x03;

            Size += 40;

            Payload = payload;

            Size += payload.PayloadSize;

        }

        internal override UnsignedTransaction Prepare()
        {
            var tBytes = base.Serialize(Size - 8 - Payload.PayloadSize);

            return new UnsignedTransaction()
            {
                Payload = tBytes[0].Concat(Serialize()).ToArray(),
                VerifiablePayload = tBytes[1]
            };
        }

        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Payload.TransactionsHash);
        }

        internal byte[] Serialize()
        {
            lock (this)
            {
                uint len = Payload.PayloadSize;

                byte[] ap = new byte[len];

                int offset = 0;

                foreach (byte[] p in Payload.EmbeddedTransactionsPayload)
                {
                    Buffer.BlockCopy(p, 0, ap, offset, p.Length);

                    offset += p.Length;
                }

                DataSerializer serializer = new DataSerializer(8 + len, 0);

                serializer.SerializeProperty(Payload.PayloadSize);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(ap);

                return serializer.GetBytes()[0];
            }
        }

        public override AggregateTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}