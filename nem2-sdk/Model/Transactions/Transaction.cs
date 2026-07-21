using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using System.Reflection;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class VerifiableTransaction
    {
        [Order(1)]
        public uint Size { get; set; }

        [Order(2)]
        public uint VerifiableEntityHeaderReserved { get; set; }

        [Order(3)]
        public byte[] Signature { get; set; }

        [Order(5)]
        public byte[] Signer { get; set; }

        [Order(6)]
        public uint Entity_body_reserved_1 { get; set; }

        [Order(7)]
        public byte Version { get; set; }

        [Order(8)]
        public byte Network { get; set; }

        [Order(9)]
        public ushort Type { get; set; }

        [Order(10)]
        public byte[] Fee { get; set; }

        [Order(11)]
        public byte[] Deadline { get; set; }

        public VerifiableTransaction(bool isEmbedded)
        {
            Size += 48;
            VerifiableEntityHeaderReserved = 0;
            Entity_body_reserved_1 = 0;
            Signature = new byte[64];

            if (!isEmbedded)
                Size += 80;
        }

        public VerifiableTransaction(TransactionTypes.Types type, bool isEmbedded)
        {
            Size += 48;
            VerifiableEntityHeaderReserved = 0;
            Signature = new byte[64];

            if (!isEmbedded)
                Size += 80;

            Type = type.GetValue();
        }

        public abstract VerifiableTransaction SetSigner(string signer);

        public UnsignedTransaction Embed(string signer)
        {
            Signer = signer.FromHex();

            return new UnsignedTransaction()
            {
                Payload = this.Serialize(exclude: [3, 10, 11])
            };
        }

        private bool isAggregate()
        {
            return this.Type == TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue() || this.Type == TransactionTypes.Types.AGGREGATE_BONDED.GetValue();
        }

        public SignedTransaction WrapVerified(SecretKeyPair signer, string genHash)
        {
            Signer = signer.PublicKey;

            byte[] entity = this.Serialize(exclude: []);

            int truncate = 0;

            if (isAggregate())
            {
                truncate = 52; // remove everything before version and after transactionsHash.
            }
            else
            {
                truncate = entity.Length - (4 + 4 + 64 + 32 + 4);
            }

            var signBytes = new byte[32 + truncate];

            for (var x = 0; x < 32; x++)
                signBytes[x] = genHash.FromHex()[x];

            for (var x = 32; x < 32 + truncate; x++)
                signBytes[x] = entity[(4 + 4 + 64 + 32 + 4) + x - 32];

            var sig = NaclFast.SignDetached(msg: signBytes, signer.SecretKey.ToArray());

            if (NaclFast.SignDetachedVerify(signBytes, sig, signer.PublicKey))
            {
                for (var x = 8; x < 72; x++)
                    entity[x] = sig[x - 8];

                var result = new SignedTransaction()
                {
                    Payload = entity,
                    SignedBytes = signBytes,
                    Signer = signer.PublicKeyString,
                    Signature = sig.ToHex(),
                    Hash = HashTransaction(sig, signer.PublicKey, signBytes)
                };

                if (isAggregate())
                {
                    var hash = new byte[32];

                    var sha3Hasher = new Sha3Digest(256);

                    sha3Hasher.BlockUpdate(result.Hash.FromHex(), 0, 32);
                    sha3Hasher.BlockUpdate(result.Signer.FromHex(), 0, 32);
                    sha3Hasher.DoFinal(hash, 0);

                    result.Hash = hash.ToHex();

                }

                return result;
            }
            else throw new Exception("signature error");
        }

        private byte[] Serialize(int[] exclude)
        {
            DataSerializer serializer = new DataSerializer(Size);

            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p =>
                    {
                        return !exclude.Contains(((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order);
                    })
                    .OrderBy(p =>
                    {
                        return ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order;
                    });

            serializer.Serialize(this, properties);

            return serializer.GetBytes();
        }

        public static string HashTransaction(byte[] signature, byte[] signer, byte[] signBytes)
        {
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);

            sha3Hasher.BlockUpdate(signature, 0, signature.Length);
            sha3Hasher.BlockUpdate(signer, 0, signer.Length);
            sha3Hasher.BlockUpdate(signBytes, 0, signBytes.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash.ToHex();
        }
    }
}
