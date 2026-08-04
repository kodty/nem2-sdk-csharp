using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class TransactionExtension
    {
        internal abstract void Extend(DataSerializer serializer);
        internal abstract int AddSize();
        internal abstract byte SetVersion();
        internal abstract TransactionTypes.Types SetType();
    }

    public class SimpleTransaction : VerifiableTransaction
    {
        public SimpleTransaction(TransactionExtension extension, NetworkType.Types networkType, ulong fee)
        {
            TransactionExtension = extension;
            Size += (uint)TransactionExtension.AddSize();

            base.Version = TransactionExtension.SetVersion();
            base.Network = networkType.GetNetworkByte();
            base.Type = TransactionExtension.SetType().GetValue();
            
            base.Fee = DataConverter.ConvertFrom(fee);
        }

        internal override void Extend(DataSerializer serializer) => TransactionExtension.Extend(serializer);

        public TransactionExtension TransactionExtension { get; set; }

        
    }

    public class SubTransaction : Transaction
    {
        public SubTransaction(TransactionExtension extension, NetworkType.Types networkType)
        {
            TransactionExtension = extension;
            Size += (uint)TransactionExtension.AddSize();

            Version = TransactionExtension.SetVersion();
            Network = networkType.GetNetworkByte();
            Type = TransactionExtension.SetType().GetValue();  
        }

        internal override void Extend(DataSerializer serializer) => TransactionExtension.Extend(serializer);

        public TransactionExtension TransactionExtension { get; set;}

        internal override byte[][] Serialize(uint size)
        {
            lock (this)
            {
                DataSerializer serializer = new DataSerializer(size, 44);

                serializer.SerializeProperty(Size);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(Signer);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(Version);
                serializer.SerializeProperty(Network);
                serializer.SerializeProperty(Type);

                Extend(serializer);

                return serializer.GetBytes();
            }
        }
    }

    public abstract class Transaction
    {
        public static Transaction Create(TransactionExtension transaction, NetworkType.Types networkType)
        {
            return new SubTransaction(transaction, networkType);
        }

        public Transaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        public void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }

        public uint Size { get; set; }

        public byte[] Signer { get; set; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public ushort Type { get; set; }

        internal abstract byte[][] Serialize(uint size);

        internal abstract void Extend(DataSerializer serializer);

        public Transaction()
        {
            Size += 48;
        }

        internal virtual UnsignedTransaction Prepare()
        {
            byte[][] tBytes = new byte[2][];

            if (Size % 8 != 0)
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

        public static SimpleTransaction Create(TransactionExtension transaction, NetworkType.Types networkType, ulong fee)
        {
            return new SimpleTransaction(transaction, networkType, fee);
        }

        public VerifiableTransaction()
        {
            Size += 80;

            Signature = new byte[64];   
        }

        public byte[] Signature { get; set; }
        public byte[] Fee { get; set; }
        public byte[] Deadline { get; set; }

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

        internal override UnsignedTransaction Prepare()
        {
            byte[][] tBytes = new byte[2][];

            tBytes = this.Serialize(Size);
            
            return new UnsignedTransaction()
            {
                Payload = tBytes[0],
                VerifiablePayload = tBytes[1]
            };
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

        internal override byte[][] Serialize(uint size)
        {
            lock (this)
            {
                DataSerializer serializer = new DataSerializer(size, 108);

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
    }
}
