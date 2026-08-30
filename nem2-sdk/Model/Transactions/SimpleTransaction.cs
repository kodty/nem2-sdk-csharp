using io.nem2.sdk.Infrastructure;

namespace io.nem2.sdk.Model.Transactions
{
    public class SimpleTransaction : VerifiableTransaction
    {
        public SimpleTransaction(TransactionExtension extension, NetworkType.Types networkType, ulong fee, Deadline deadline)
            : base(extension.SetVersion(), networkType.GetNetworkByte(), extension.SetType().GetValue(), fee, deadline)
        {
            TransactionExtension = extension;
            Size += (uint)TransactionExtension.AddSize();
        }

        internal override void Extend(DataSerializer serializer) => TransactionExtension.Extend(serializer);

        public TransactionExtension TransactionExtension { get; set; }

        public override bool IsAggregate()
        {
            return typeof(AggregatePayload) == TransactionExtension.GetType();
        }
    }
}
