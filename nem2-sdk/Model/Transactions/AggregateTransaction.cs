using Coppery;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregateTransaction : VerifiableTransaction
    {
        [Order(12)]
        public byte[] TransactionsHash { get; set; }
        [Order(13)]
        public uint PayloadSize { get; set; }
        [Order(14)]
        public uint Aggregate_​transaction_​header_​reserved_​1 { get; set; }
        [Order(15)]
        public byte[] EmbeddedTransactionsPayload { get; set; }
        [Order(16)]
        public byte[] Cosignatures { get; set; }

        public AggregateTransaction(UnsignedTransaction[] embeddedTransactions, byte[] cosignatures, TransactionTypes.Types type) : base(type, false)
        {
            Version = 0x03;
            EmbeddedTransactionsPayload = Combine(embeddedTransactions.ToList().Select(e => e.Payload).ToArray());

            var embeddedTransactionHashes = embeddedTransactions.ToList().Select(e => {

                var hash = new byte[32];

                var sha3Hasher = new Sha3Digest(256);

                sha3Hasher.BlockUpdate(e.Payload, 44, e.Payload.Length - 44);

                sha3Hasher.DoFinal(hash, 0);

                return hash;

            }).ToArray();

            TransactionsHash = CalculateMerkleRoot(embeddedTransactionHashes);

            PayloadSize = (uint)EmbeddedTransactionsPayload.Length;
            Aggregate_​transaction_​header_​reserved_​1 = 0;
            
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

            while (1 < numRemainingHashes) {

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