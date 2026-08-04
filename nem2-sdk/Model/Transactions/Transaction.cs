using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class Transaction
    {
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
    }   
}
