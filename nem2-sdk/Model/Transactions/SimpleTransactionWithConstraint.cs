using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public class SimpleTransaction<T> : VerifiableTransaction where T : TransactionExtension
    {
        public SimpleTransaction(T extension, NetworkType.Types networkType, ulong fee, Deadline deadline)
        {
            TransactionExtension = extension;
            Size += (uint)TransactionExtension.AddSize();

            base.Version = TransactionExtension.SetVersion();
            base.Network = networkType.GetNetworkByte();
            base.Type = TransactionExtension.SetType().GetValue();

            base.Fee = fee;
            base.Deadline = deadline.Ticks;
        }

        internal override void Extend(DataSerializer serializer) => TransactionExtension.Extend(serializer);

        public T TransactionExtension { get; set; }

        public override bool IsAggregate()
        {
            return typeof(AggregatePayload) == TransactionExtension.GetType();
        }
    }
}
