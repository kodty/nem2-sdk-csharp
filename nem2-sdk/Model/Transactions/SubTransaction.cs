using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public class SubTransaction : Transaction
    {
        public SubTransaction(TransactionExtension extension, NetworkType.Types networkType) 
            : base(extension.SetVersion(), networkType.GetNetworkByte(), extension.SetType().GetValue())
        {
            TransactionExtension = extension;
            Size += (uint)TransactionExtension.AddSize();
        }

        internal override void Extend(DataSerializer serializer) => TransactionExtension.Extend(serializer);

        public TransactionExtension TransactionExtension { get; set; }
    }
}
