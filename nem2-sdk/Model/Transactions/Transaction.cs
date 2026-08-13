using Coppery;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class Transaction
    {
        public uint Size { get; set; }

        public byte[] Signer { get; set; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public ushort Type { get; set; }

        internal byte[] Payload { get; set; }

        internal byte[] VerifiablePayload { get; set; }

        public Transaction(byte version, byte network, ushort type)
        {
            Size += 48;
            Version = version;
            Network = network;
            Type = type;
        }

        internal abstract void Extend(DataSerializer serializer);

        public static SubTransaction Create(TransactionExtension transaction, NetworkType.Types networkType)
        {
            return new SubTransaction(transaction, networkType);
        }

        public static SubTransaction<T> Create<T>(T transaction, NetworkType.Types networkType) where T : TransactionExtension
        {
            return new SubTransaction<T>(transaction, networkType);
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

        public virtual SignedTransaction Prepare()
        {
            if (Size % 8 != 0)
                Size += (uint)((Math.Ceiling((decimal)Size / 8) * 8) - Size);
            
            Serialize();

            return new SignedTransaction()
            {
                Payload = Payload,
                VerifiablePayload = VerifiablePayload,
                Signer = Signer.ToHex(),
                Hash = Hash(VerifiablePayload).ToHex()
            };
        }

        private static byte[] Hash(byte[] data3)
        {
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);
            sha3Hasher.BlockUpdate(data3, 0, data3.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }

        protected virtual void Serialize()
        {
            Payload = new byte[Size];
            
            lock (this)
            {
                DataSerializer serializer = new DataSerializer(Payload);

                serializer.SerializeProperty(Size);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(Signer);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(Version);
                serializer.SerializeProperty(Network);
                serializer.SerializeProperty(Type);

                Extend(serializer);

                VerifiablePayload = Payload.Take(new Range(44, Payload.Length)).ToArray();
            }
        }
    }   
}
