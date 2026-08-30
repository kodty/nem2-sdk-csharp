using io.nem2.sdk.Infrastructure;

namespace io.nem2.sdk.Model.Transactions
{
    public class SubTransaction<T> : Transaction where T : TransactionExtension
    {
        public SubTransaction(T extension, NetworkType.Types networkType) 
            : base(extension.SetVersion(), networkType.GetNetworkByte(), extension.SetType().GetValue())
        {         
            TransactionExtension = extension;
            Size += (uint)TransactionExtension.AddSize();
        }

        internal override void Extend(DataSerializer serializer) => TransactionExtension.Extend(serializer);

        public T TransactionExtension { get; set; }
    }
}
