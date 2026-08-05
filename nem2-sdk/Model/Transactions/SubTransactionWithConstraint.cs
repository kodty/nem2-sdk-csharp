using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
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
    }
}
