using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class Transaction
    {
        public uint Size { get; set; }

        public byte[] Signer { get; set; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public ushort Type { get; set; }

        internal bool IsEmbedded { get; set; }

        public abstract Transaction SetSigner(string signer);

        public abstract void SetVersion(byte version);

        internal abstract byte[][] Serialize(uint size);

        internal abstract void Extend(DataSerializer serializer);

        public Transaction(TransactionTypes.Types type, bool isEmbedded)
        {
            IsEmbedded = isEmbedded;

            Size += 48;

            Type = type.GetValue();
        }

        internal virtual UnsignedTransaction Prepare()
        {
            byte[][] tBytes = new byte[2][];

            if (IsEmbedded && Size % 8 != 0)
                Size += (uint)((Math.Ceiling((decimal)Size / 8) * 8) - Size);
            
            tBytes = this.Serialize(Size);

            return new UnsignedTransaction()
            {
                Payload = tBytes[0],
                VerifiablePayload = tBytes[1]
            };
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
    public abstract class VerifiableTransaction : Transaction
    {
        public VerifiableTransaction(TransactionTypes.Types type, bool isEmbedded) : base(type, isEmbedded)
        {
            if(!isEmbedded)
                Size += 80;

            Signature = new byte[64];   
        }

        private byte[] _Signature { get; set; }
        private byte[] _Fee { get; set; }
        private byte[] _Deadline { get; set; }
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

        public SignedTransaction SignTransaction(SecretKeyPair keyPair, string networkGenHash) => SignTransaction(keyPair, networkGenHash.FromHex());

        protected SignedTransaction SignTransaction(SecretKeyPair signer, byte[] networkGenHash)
        {       
            var tBytes = Prepare();

            byte[] signBytes = [.. networkGenHash, .. tBytes.VerifiablePayload];
     
            this.Signature = NaclFast.SignDetached(msg: signBytes, signer.SecretKey.ToArray());

            for (int x = 0; x < 64; x++)
                tBytes.Payload[x + 8] = this.Signature[x];

            return new SignedTransaction()
            {
                Signature = this.Signature.ToHex(),
                VerifiablePayload = signBytes,
                Signer = signer.PublicKeyString,
                Payload = tBytes.Payload,
                Hash = HashTransaction(this.Signature, signer.PublicKey, signBytes).ToHex()
            };
        }

        internal override byte[][] Serialize(uint size)
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
    }
}
