using io.nem2.sdk.Infrastructure;

namespace io.nem2.sdk.Model.Transactions
{
    public class MosaicAliasTransaction : AliasTransaction
    {
        public MosaicAliasTransaction(ulong mosaicId, ulong namespaceId, byte aliasAction) : base(TransactionTypes.Types.MOSAIC_ALIAS, namespaceId, aliasAction)
        {
            MosaicId = mosaicId;
        }

        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(NamespaceId);
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(AliasAction);
        }

        public ulong MosaicId { get; set; }

        internal override int AddSize()
        {
            return 17;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.MOSAIC_ALIAS;
        }
    }
}
