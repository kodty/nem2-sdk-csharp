using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using System.Reflection;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class VerifiableTransaction
    {
        public VerifiableTransaction(TransactionTypes.Types type, bool isEmbedded)
        {
            Size += 128;

            Signature = new byte[64];

            IsEmbedded = isEmbedded;

            if (isEmbedded) Size -= 80;

            Type = type.GetValue();
        }

        private byte[] _Signature { get; set; }
        private byte[] _Fee { get; set; }
        private byte[] _Deadline { get; set; }
        internal bool IsEmbedded { get; set; }
        public SignedTransaction SignedTransaction { get; set; }
        public abstract VerifiableTransaction SetSigner(string signer);      
        public abstract PropertyInfo[] RetrieveProperties();
        public abstract void SetVersion(byte version);

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

        public uint Size { get; set; }

        public uint VerifiableEntityHeaderReserved { get; }

        public byte[] Signature
        {
            get
            {
                if (IsEmbedded)
                {
                    return new byte[] { };
                }
                else return _Signature;
            }
            set
            {
                if (_Signature != value && !IsEmbedded)
                {
                    _Signature = value;
                }
            }
        }

        public byte[] Signer { get; set; }

        public uint Entity_body_reserved_1 { get; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public ushort Type { get; set; }

        public byte[] Fee
        {
            get
            {
                if (IsEmbedded)
                {
                    return new byte[] { };
                }
                else return _Fee;
            }
            set
            {
                if (_Fee != value && !IsEmbedded)
                {
                    _Fee = value;
                }
            }
        }

        public byte[] Deadline
        {
            get
            {
                if (IsEmbedded)
                {
                    return new byte[] { };
                }
                else return _Deadline;
            }
            set
            {
                if (_Deadline != value && !IsEmbedded)
                {
                    _Deadline = value;
                }
            }
        }

        internal bool isAggregate()
        {
            return this.Type == TransactionTypes.Types.AGGREGATE_COMPLETE.GetValue() || this.Type == TransactionTypes.Types.AGGREGATE_BONDED.GetValue();
        }

        public SignedTransaction SignEmbeddedTransaction(SecretKeyPair keyPair)
           => SignTransaction(keyPair, exclude: [[], [0, 1, 3, 4]], excludeLen: 44);

        public SignedTransaction SignTransaction(SecretKeyPair keyPair, string networkGenHash = null)
           => SignTransaction(keyPair, exclude: [[],[0, 1, 2, 3, 4]], excludeLen: 108, networkGenHash.FromHex());

        protected SignedTransaction SignTransaction(SecretKeyPair signer, uint[][] exclude, uint excludeLen, byte[] networkGenHash = null)
        {
            if (!IsEmbedded && networkGenHash == null)
                throw new Exception("conflict");
            if (Signer != null && Signer.ToHex() != signer.PublicKeyString)
                throw new Exception("signer mismatch");

            Signer = signer.PublicKey;

            var s = Size;

            var tBytes = this.Serialize(
                [
                    [s,               ..exclude[0] ], 
                    [s -= excludeLen, ..exclude[1] ]
                ]
            );

            var signBytes = new byte[32 + tBytes[1].Length];

            if (IsEmbedded) signBytes = tBytes[1];
            else
            {
                for (var x = 0; x < tBytes[1].Length; x++)
                {
                    signBytes[x + 32] = tBytes[1][x];

                    if (x < 32)
                        signBytes[x] = networkGenHash[x];
                }
            }

            var sig = NaclFast.SignDetached(msg: signBytes, signer.SecretKey.ToArray());

            if (!IsEmbedded)
            {
                for (int x = 0; x < 64; x++)
                    tBytes[0][x + 8] = sig[x];

                this.Signature = sig;
            }

            if (NaclFast.SignDetachedVerify(signBytes, sig, signer.PublicKey))
            {
                return new SignedTransaction()
                {
                    Signature = sig,
                    SignedBytes = signBytes, 
                    Signer = signer.PublicKeyString,
                    Payload = tBytes[0],
                    Hash = HashTransaction(this.Signature, signer.PublicKey, signBytes).ToHex()
                };
            }
            else throw new Exception("invalid signature");
        }

        internal byte[][] Serialize(uint[][] s)
        {
            lock (this)
            {
                DataSerializer serializer = new DataSerializer(s);

                var props = RetrieveProperties();

                for (var x = 0; x < props.Length; x++)
                        serializer.SerializeProperty(props[x].GetValue(this), props[x].PropertyType, (uint)x);

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
