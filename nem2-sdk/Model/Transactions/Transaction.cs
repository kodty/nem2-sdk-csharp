using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using System.Reflection;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class VerifiableTransaction
    {
        [Order(0)]
        public VerifiableEntity VerifiableEntity { get; set; }

        [Order(4)]
        public EntityBody EntityBody { get; set; }

        [Order(9)]
        public ushort Type { get; set; }

        [Order(10)]
        public byte[] Fee { get; set; }

        [Order(11)]
        public byte[] Deadline { get; set; }

        public VerifiableTransaction(bool isEmbedded)
        {
            VerifiableEntity = new VerifiableEntity();

            if (!isEmbedded)
                VerifiableEntity.Size += 80;
        }

        public VerifiableTransaction(TransactionTypes.Types type, bool isEmbedded)
        {
            VerifiableEntity = new VerifiableEntity();

            if (!isEmbedded)
                VerifiableEntity.Size += 80;

            Type = type.GetValue();
        }

        public abstract VerifiableTransaction SetSigner(string signer);

        public UnsignedTransaction Embed(string signer)
        {
            EntityBody.Signer = signer.FromHex();

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
            EntityBody.Signer = signer.PublicKey;

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

                return new SignedTransaction()
                {
                    Payload = entity,
                    SignedBytes = signBytes,
                    Signer = signer.PublicKeyString,
                    Signature = sig.ToHex(),
                    Hash = HashTransaction(sig, signer.PublicKey, signBytes)
                };
            }
            else throw new Exception("signature error");
        }

        private byte[] Serialize(int[] exclude)
        {
            DataSerializer serializer = new DataSerializer(this.VerifiableEntity.Size);

            serializer.Serialize(this, this.GetType(), (Type type) =>
            {
                return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p =>
                        {
                            return !exclude.Contains(((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order);
                        })
                    .OrderBy(p =>
                        {
                            return ((OrderAttribute)p.GetCustomAttributes(typeof(OrderAttribute), false)[0]).Order;
                        });
            });

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
