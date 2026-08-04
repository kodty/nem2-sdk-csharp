using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class TransactionExtension
    {
        internal abstract void Extend(DataSerializer serializer);
        internal abstract int AddSize();
        internal abstract byte SetVersion();
        internal abstract TransactionTypes.Types SetType();
    }
}
