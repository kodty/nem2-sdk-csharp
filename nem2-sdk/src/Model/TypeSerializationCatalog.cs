using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Model
{
    public static class TypeSerializationCatalog
    {
        public static object[] objects = [new string[] { }, typeof(TransactionData), typeof(SimpleTransfer)];

        public static object[] BaseTransactionTypes = [
            typeof(TransactionData),
            typeof(EmbeddedTransactionData)
            ];

        public static Type[] CustomTypes = [ // include non native, generic type arguments
            typeof(ushort),
            typeof(bool),
            typeof(byte),
            typeof(int),
            typeof(ulong),
            typeof(string),
            typeof(uint),
            typeof(Pagination),
            typeof(ExtendedBlockInfo),
            typeof(AccountData),
            typeof(ActivityBucket),
            typeof(VotingKeys),
            typeof(RestrictionData),
            typeof(MosaicTransfer),
            typeof(MessageGroup),
            typeof(Signature),
            typeof(Tree),
            typeof(LinkBit),
            typeof(RestrictionData),
            typeof(Restrictions),
            typeof(MosaicEvent),
            typeof(MosaicName),
            typeof(AccountName),
            typeof(ReceiptDatum),
            typeof(AddressDatum),
            typeof(MosaicDatum),
            typeof(Receipt),
            typeof(ResolutionEntry),
            typeof(MosaicRestrictionData),
            typeof(Cosignature),
            typeof(EmbeddedTransactionData),
            typeof(MosaicRestriction)
            ];
    }
}
