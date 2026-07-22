using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using System.Reflection;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class VerifiableTransaction
    {
        public PropertyInfo[] BaseProperties => 
            [ 
                GetType().GetProperty("Size"),
                GetType().GetProperty("VerifiableEntityHeaderReserved"),
                GetType().GetProperty("Signature"),
                GetType().GetProperty("Signer"),
                GetType().GetProperty("Entity_body_reserved_1"),
                GetType().GetProperty("Version"),
                GetType().GetProperty("Network"),
                GetType().GetProperty("Type"),
                GetType().GetProperty("Fee"),
                GetType().GetProperty("Deadline")
            ];

        // return extended transaction properties intended for serialization.
        public abstract PropertyInfo[] RetrieveProperties();

        public uint Size { get; set; }

        public uint VerifiableEntityHeaderReserved { get; }

        public byte[] Signature { get; set; }

        public byte[] Signer { get; set; }

        public uint Entity_body_reserved_1 { get; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public ushort Type { get; set; }

        public byte[] Fee { get; set; }

        public byte[] Deadline { get; set; }

        public VerifiableTransaction(TransactionTypes.Types type, bool isEmbedded)
        {
            Size += 48;
            Signature = new byte[64];

            if (!isEmbedded)
                Size += 80;

            Type = type.GetValue();
        }

        public abstract VerifiableTransaction SetSigner(string signer);

        public abstract void SetVersion(byte version);


        public UnsignedTransaction Embed(string signer)
        {
            Signer = signer.FromHex();

            return new UnsignedTransaction()
            {
                Payload = this.Serialize(exclude: [3, 9, 10])
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

            var props = RetrieveProperties();

            for (var x = 0; x < props.Length; x++)
                if (!exclude.Contains(x)) 
                    serializer.SerializeProperty(props[x].GetValue(this), props[x].PropertyType);

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
