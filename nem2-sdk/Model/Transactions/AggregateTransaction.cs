using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using System.Reflection;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregateTransaction : VerifiableTransaction
    {
        public override PropertyInfo[] RetrieveProperties()
        {
            return
            [
                .. BaseProperties,
                GetType().GetProperty("TransactionsHash"),
                GetType().GetProperty("PayloadSize"),
                GetType().GetProperty("AggregateHeaderPadding"),
                GetType().GetProperty("EmbeddedTransactionsPayload"),
                GetType().GetProperty("Cosignatures")
            ];
        }

        public byte[] TransactionsHash { get; set; }

        public uint PayloadSize { get; set; }

        public uint AggregateHeaderPadding { get; }

        public byte[] EmbeddedTransactionsPayload { get; set; }

        public byte[] Cosignatures { get; set; }

        public SignedTransaction[] EmbeddedTransactions { get; set; }

        public AggregateTransaction(SignedTransaction[] embeddedTransactions, byte[] cosignatures, TransactionTypes.Types type) : base(type, false)
        {
            Version = 0x03;

            EmbeddedTransactions = embeddedTransactions;

            EmbeddedTransactionsPayload = PaddedCombine();

            PayloadSize = (uint)EmbeddedTransactionsPayload.Length;

            if (cosignatures == null)
                Cosignatures = [];
            else Cosignatures = cosignatures;

            Size += 40;
            Size += (uint)EmbeddedTransactionsPayload.Length;
            Size += (uint)Cosignatures.Length;
        }

        private byte[] CalculateMerkleRoot(byte[][] embeddedTransactionHashes)
        {
            var numRemainingHashes = embeddedTransactionHashes.Length;

            while (1 < numRemainingHashes)
            {

                int i = 0;

                while (i < numRemainingHashes)
                {
                    var hash = new byte[32];

                    var sha3Hasher = new Sha3Digest(256);

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
                }

                numRemainingHashes = (int)Math.Floor((double)numRemainingHashes / 2);
            }

            return embeddedTransactionHashes[0];
        }

        private byte[] PaddedCombine()
        {
            var ets = EmbeddedTransactions.ToList();

            var pPayloads = ets.Select(item =>
            {
                if (item.Payload.Length % 8 != 0 && ets.IndexOf(item) != EmbeddedTransactions.Length - 1)
                {
                    var paddedPayload = new byte[(int)(Math.Ceiling((decimal)item.Payload.Length / 8) * 8)];

                    Buffer.BlockCopy(item.Payload, 0, paddedPayload, 0, item.Payload.Length);

                    return paddedPayload;
                }
                else return item.Payload;

            }).ToArray();

            TransactionsHash = CalculateMerkleRoot(ets.Select(e => e.Hash.FromHex()).ToArray());

            byte[] ap = new byte[pPayloads.Sum(a => a.Length)];

            int offset = 0;

            foreach (byte[] p in pPayloads)
            {
                Buffer.BlockCopy(p, 0, ap, offset, p.Length);

                offset += p.Length;
            }

            return ap;
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