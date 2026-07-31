using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregateTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(TransactionsHash);
            serializer.SerializeProperty(PayloadSize);
            serializer.SerializeProperty(new byte[4]);
            serializer.SerializeProperty(EmbeddedTransactionsPayload);
            
            if(Cosignatures != null)
                foreach(var e in Cosignatures) {
                    serializer.SerializeProperty((byte)0x0);
                    serializer.SerializeProperty(e.Signer);
                    serializer.SerializeProperty(e.Signature);
                }
        }

        public byte[] TransactionsHash { get; set; }

        public uint PayloadSize { get; set; }

        public byte[] EmbeddedTransactionsPayload { get; set; }

        public Cosignature[] Cosignatures { get; set; }

        public UnsignedTransaction[] EmbeddedTransactions { get; set; }

        public AggregateTransaction(UnsignedTransaction[] embeddedTransactions, TransactionTypes.Types type) : base(type, false)
        {
            Version = 0x03;

            EmbeddedTransactions = embeddedTransactions;

            EmbeddedTransactionsPayload = PaddedCombine();

            PayloadSize += (uint)EmbeddedTransactionsPayload.Length;

            Size += 40;
            Size += (uint)EmbeddedTransactionsPayload.Length;     
        }

        public void Cosign(SecretKeyPair[] signers)
        {
            var si = Size;

            var tBytes = this.Serialize(si);

            Cosignatures = new Cosignature[signers.Count()];

            for (int i = 0; i < signers.Length; i++)
            {
                SecretKeyPair? signer = signers[i];

                if (Signer == signer.PublicKey) return;

                var sig = NaclFast.SignDetached(msg: tBytes[1], signer.SecretKey.ToArray());

                Cosignatures[i] = new Cosignature()
                {
                    Version = 0x0,
                    Signature = sig,
                    Signer = signer.PublicKey,
                };

                Size += 1;
                Size += 64;
                Size += 32;
            }
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

            uint bufPadding = 0;

            var pPayloads = ets.Select(item =>
            {
                if (item.Payload.Length % 8 != 0 && ets.IndexOf(item) != EmbeddedTransactions.Length)
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

            TransactionsHash = CalculateMerkleRoot(ets.Select(e => Hash(e.VerifiablePayload)).ToArray());

            byte[] ap = new byte[pPayloads.Sum(a => a.Length)];

            int offset = 0;

            foreach (byte[] p in pPayloads)
            {
                Buffer.BlockCopy(p, 0, ap, offset, p.Length);

                offset += p.Length;
            }

            PayloadSize += bufPadding;

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