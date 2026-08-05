using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class AliasTransaction : TransactionExtension
    {
        public AliasTransaction(TransactionTypes.Types type, ulong namespaceId, byte aliasAction) 
        {         
            NamespaceId = namespaceId;
            AliasAction = aliasAction;
        }

        public ulong NamespaceId { get; set; }

        public byte AliasAction { get; set; }

        internal override byte SetVersion()
        {
            return 0x01;
        }
    }

    public class AddressAliasTransaction : AliasTransaction
    {
        public AddressAliasTransaction(string address, ulong namespaceId, byte aliasAction) : base(TransactionTypes.Types.ADDRESS_ALIAS, namespaceId, aliasAction)
        {
            Address = address.IsBase32()
                      ? AddressEncoder.DecodeAddress(address)
                      : address.FromHex();
        }

        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(NamespaceId);
            serializer.SerializeProperty(Address);
            serializer.SerializeProperty(AliasAction);
        }

        public byte[] Address { get; set; }

        internal override int AddSize()
        {
            return 33;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.ADDRESS_ALIAS;
        }
    }

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
