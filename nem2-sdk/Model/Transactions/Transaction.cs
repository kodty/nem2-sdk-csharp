using Coppery;
using Org.BouncyCastle.Crypto.Digests;
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

        internal abstract void Extend(DataSerializer serializer);

        public abstract VerifiableTransaction SetSigner(string signer);      

        public abstract void SetVersion(byte version);

        public uint Size { get; set; }

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

        public UnsignedTransaction PrepareEmbedded(string signer) => Prepare(signer);

        public SignedTransaction SignTransaction(SecretKeyPair keyPair, string networkGenHash) => SignTransaction(keyPair, networkGenHash.FromHex());

        internal UnsignedTransaction Prepare(string signer)
        {
            if (Signer != null && Signer.ToHex() != signer)
                throw new Exception("signer not set or mismatch");

            Signer = signer.FromHex();

            var tBytes = this.Serialize(Size);

            return new UnsignedTransaction()
            {
                Payload = tBytes[0],
                VerifiablePayload = tBytes[1]
            };
        }

        protected SignedTransaction SignTransaction(SecretKeyPair signer, byte[] networkGenHash)
        {       
            var tBytes = Prepare(signer.PublicKeyString);

            var signBytes = networkGenHash.Concat(tBytes.VerifiablePayload).ToArray();
     
            this.Signature = NaclFast.SignDetached(msg: signBytes, signer.SecretKey.ToArray());

            for (int x = 0; x < 64; x++)
                tBytes.Payload[x + 8] = this.Signature[x];

            if (NaclFast.SignDetachedVerify(signBytes, this.Signature, signer.PublicKey))
            {
                return new SignedTransaction()
                {
                    Signature = this.Signature.ToHex(),
                    VerifiablePayload = signBytes, 
                    Signer = signer.PublicKeyString,
                    Payload = tBytes.Payload,
                    Hash = HashTransaction(this.Signature, signer.PublicKey, signBytes).ToHex()
                };
            }
            else throw new Exception("invalid signature");
        }

        protected SignedTransaction SignAnyTransaction(SecretKeyPair signer, byte[] networkGenHash = null)
        {
            var tBytes = Prepare(signer.PublicKeyString);

            var signBytes = new byte[32 + tBytes.VerifiablePayload.Length];

            if (IsEmbedded) signBytes = tBytes.VerifiablePayload;
            else
            {
                signBytes = networkGenHash.Concat(tBytes.VerifiablePayload).ToArray();
            }        

            var sig = NaclFast.SignDetached(msg: signBytes, signer.SecretKey.ToArray());

            if (!IsEmbedded)
            {
                for (int x = 0; x < 64; x++)
                    tBytes.Payload[x + 8] = sig[x];

                this.Signature = sig;
            }

            if (NaclFast.SignDetachedVerify(signBytes, sig, signer.PublicKey))
            {
                return new SignedTransaction()
                {
                    Signature = sig.ToHex(),
                    VerifiablePayload = signBytes,
                    Signer = signer.PublicKeyString,
                    Payload = tBytes.Payload,
                    Hash = HashTransaction(this.Signature, signer.PublicKey, signBytes).ToHex()
                };
            }
            else throw new Exception("invalid signature");
        }

        internal byte[][] Serialize(uint size)
        {
            lock (this)
            {
                DataSerializer serializer = new DataSerializer(size, IsEmbedded ? 44 : 108);

                serializer.SerializeProperty(Size);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(Signature);
                serializer.SerializeProperty(Signer);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(Version);
                serializer.SerializeProperty(Network);
                serializer.SerializeProperty(Type);
                serializer.SerializeProperty(Fee);
                serializer.SerializeProperty(Deadline);

                Extend(serializer);

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

        internal static byte[] Hash(byte[] data)
        {
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);
            sha3Hasher.BlockUpdate(data, 0, data.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }
    }
}
