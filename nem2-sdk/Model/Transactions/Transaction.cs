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

        public SignedTransaction SignedTransaction { get; set; }

        internal bool IsEmbedded { get; set; }

        public VerifiableTransaction(TransactionTypes.Types type, bool isEmbedded)
        {
            Size += 128;
            Signature = new byte[64];

            IsEmbedded = isEmbedded;

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

        internal bool isAggregate()
        {
            return this.Type == TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue() || this.Type == TransactionTypes.Types.AGGREGATE_BONDED.GetValue();
        }

        public SignedTransaction SignEmbeddedTransaction(SecretKeyPair keyPair)
           => SignTransaction(keyPair, exclude: [2, 8, 9], excludeLen: 124);

        public SignedTransaction SignTransaction(SecretKeyPair keyPair, string networkGenHash = null)
           => SignTransaction(keyPair, exclude: [], excludeLen: 108, networkGenHash);

        protected SignedTransaction SignTransaction(SecretKeyPair signer, int[] exclude, uint excludeLen, string networkGenHash = null)
        {
            uint appendLen = 0;

            if (!IsEmbedded && networkGenHash == null)
                throw new Exception("conflict");
            if (!IsEmbedded && networkGenHash != null)
                appendLen = 32;
            if (Signer != null && Signer.ToHex() != signer.PublicKeyString)
                throw new Exception("signer mismatch");

            Signer = signer.PublicKey;

            var signBytes = new byte[appendLen + Size - excludeLen];

            if (IsEmbedded)
            { 
                exclude = [2, 8, 9]; excludeLen = 124; 
            }
            if (!IsEmbedded)
            {
                for (var x = 0; x < 32; x++)
                    signBytes[x] = networkGenHash.FromHex()[x];
            }

            var tBytes = this.Serialize(exclude: [0, 1, 2, 3, 4, ..exclude], excludeLen: excludeLen);

            for (var x = appendLen; x < appendLen + tBytes.Length; x++)
                signBytes[x] = tBytes[x - appendLen];

            this.Signature = NaclFast.SignDetached(msg: signBytes, signer.SecretKey.ToArray());

            if (NaclFast.SignDetachedVerify(signBytes, this.Signature, signer.PublicKey))
            {
                var entity = this.Serialize(IsEmbedded ? exclude : [], IsEmbedded ? (uint)80 : 0);

                return new SignedTransaction()
                {
                    Signature = this.Signature,
                    SignedBytes = signBytes, 
                    Signer = signer.PublicKeyString,
                    Payload = entity,
                    Hash = HashTransaction(this.Signature, signer.PublicKey, signBytes).ToHex()
                };
            }
            else throw new Exception("invalid signature");
        }

        internal byte[] Serialize(int[] exclude, uint excludeLen = 0)
        {
            lock (this)
            {
                DataSerializer serializer = new DataSerializer(Size -= excludeLen);

                var props = RetrieveProperties();

                for (var x = 0; x < props.Length; x++)
                    if (!exclude.Contains(x))
                        serializer.SerializeProperty(props[x].GetValue(this), props[x].PropertyType);

                Size += excludeLen;

                return serializer.GetBytes();
            }
        }

        public static byte[] HashTransaction(byte[] signature, byte[] signer, byte[] signBytes)
        {
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);

            sha3Hasher.BlockUpdate(signature, 0, signature.Length);
            sha3Hasher.BlockUpdate(signer, 0, signer.Length);
            sha3Hasher.BlockUpdate(signBytes, 0, signBytes.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }
    }
}
