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
        public byte[] EmbeddedTransactions { get; set; }
        [Order(16)]
        public byte[] Cosignatures { get; set; }

        internal byte[][] EmbeddedTransactionHashes { get; set; }

        public AggregateTransaction(UnsignedTransaction[] embeddedTransactions, byte[] cosignatures, TransactionTypes.Types type) : base(type, false)
        {
            TransactionsHash = CalculateMerkleRoot_Combine(embeddedTransactions);
            PayloadSize = (uint)EmbeddedTransactions.Length;
            Aggregate_​transaction_​header_​reserved_​1 = 0;
            // EmbeddedTransactions = _Combine

            if (cosignatures == null)
                Cosignatures = [];
            else Cosignatures = cosignatures;

            VerifiableEntity.Size += 40;
            VerifiableEntity.Size += (uint)EmbeddedTransactions.Length;
            VerifiableEntity.Size += (uint)Cosignatures.Length;
        }

       /* https://github.com/symbol/symbol/blob/23179e5bd9002bd6856270d2e3649eb6f2066038/sdk/javascript/src/symbol/merkle.js#L34 */
        private byte[] CalculateMerkleRoot_Combine(UnsignedTransaction[] embeddedTransactions)
        {
            EmbeddedTransactionHashes = new byte[embeddedTransactions.Length][];

            int index = 0;

            byte[][] payloads = embeddedTransactions.ToList().Select(e => {

                var sha3Hasher = new Sha3Digest(256);

                sha3Hasher.BlockUpdate(e.Payload, 44, e.Payload.Length - 44);

                EmbeddedTransactionHashes[index] = new byte[32];

                sha3Hasher.DoFinal(EmbeddedTransactionHashes[index++], 0);

                // var paddedPayload = new byte[(int)(Math.Ceiling((decimal)e.Payload.Length / 8) * 8)];

                //Buffer.BlockCopy(e.Payload, 0, paddedPayload, 0, e.Payload.Length);

                return e.Payload;
            
            }).ToArray();

            EmbeddedTransactions = Combine(payloads);

            var numRemainingHashes = EmbeddedTransactionHashes.Length;

            while (1 < numRemainingHashes) {

                int i = 0;

                while (i < numRemainingHashes)
                {
                    var hash = new byte[32];

                    var sha3Hasher = new Sha3Digest(256);

                    sha3Hasher.BlockUpdate(EmbeddedTransactionHashes[i], 0, 32);

                    if (i + 1 < numRemainingHashes)
                    {
                        sha3Hasher.BlockUpdate(EmbeddedTransactionHashes[i + 1], 0, 32);
                    }
                    else
                    {
                        // if there is an odd number of hashes, duplicate the last one
                        sha3Hasher.BlockUpdate(EmbeddedTransactionHashes[i], 0, 32);
                        numRemainingHashes += 1;
                    }

                    sha3Hasher.DoFinal(EmbeddedTransactionHashes[(int)Math.Floor((double)i / 2)]);
                    i += 2;
                }

                numRemainingHashes = (int)Math.Floor((double)numRemainingHashes / 2);
            }

            return EmbeddedTransactionHashes[0];
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
            EntityBody.Signer = signer.FromHex();

            return this;
        }
    }
}