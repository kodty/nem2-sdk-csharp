using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class Transaction
    {
        public uint Size { get; set; }

        public byte[] Signer { get; set; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public ushort Type { get; set; }

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

        public virtual UnsignedTransaction Prepare()
        {
            byte[][] tBytes = new byte[2][];

            if (Size % 8 != 0)
                Size += (uint)((Math.Ceiling((decimal)Size / 8) * 8) - Size);
            
            tBytes = this.Serialize(Size);

            return new UnsignedTransaction()
            {
                Payload = tBytes[0],
                VerifiablePayload = tBytes[1],
                Signer = Signer.ToHex()
            };
        }

        protected virtual byte[][] Serialize(uint size)
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
}
