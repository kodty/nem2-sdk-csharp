using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
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

        public TransactionExtension TransactionExtension { get; set; }

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

    public class SubTransaction<T> : Transaction where T : TransactionExtension
    {
        public SubTransaction(T extension, NetworkType.Types networkType)
        {
            TransactionExtension = extension;
            Size += (uint)TransactionExtension.AddSize();

            Version = TransactionExtension.SetVersion();
            Network = networkType.GetNetworkByte();
            Type = TransactionExtension.SetType().GetValue();
        }

        internal override void Extend(DataSerializer serializer) => TransactionExtension.Extend(serializer);

        public T TransactionExtension { get; set; }

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
}
