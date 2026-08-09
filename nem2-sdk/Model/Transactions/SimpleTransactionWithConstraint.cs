using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public class SimpleTransaction<T> : VerifiableTransaction where T : TransactionExtension
    {
        public SimpleTransaction(T extension, NetworkType.Types networkType, ulong fee, Deadline deadline)
            : base(extension.SetVersion(), networkType.GetNetworkByte(), extension.SetType().GetValue(), fee, deadline)
        {
            TransactionExtension = extension;
            Size += (uint)TransactionExtension.AddSize();
        }

        internal override void Extend(DataSerializer serializer) => TransactionExtension.Extend(serializer);

        public T TransactionExtension { get; set; }

        public override bool IsAggregate()
        {
            return typeof(AggregatePayload) == TransactionExtension.GetType();
        }
    }
}
