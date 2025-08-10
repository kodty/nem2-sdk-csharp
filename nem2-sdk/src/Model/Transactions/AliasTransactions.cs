using CopperCurve;
using io.nem2.sdk.src.Model.Articles;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class AliasTransaction : Transaction
    {
        public AliasTransaction(ulong namespaceId, byte aliasAction, bool embedded) : base(embedded)
        {
            AliasAction = aliasAction;
            NamespaceId = namespaceId;
        }

        public ulong NamespaceId { get; set; }

        public byte AliasAction { get; set; }
    }

    public class AddressAliasTransaction : AliasTransaction
    {
        public AddressAliasTransaction(string address, ulong namespaceId, byte aliasAction, bool embedded) : base(namespaceId, aliasAction, embedded)
        {
            Address = address.IsBase32()
                      ? AddressEncoder.DecodeAddress(address)
                      : address.FromHex();  
        }

        public byte[] Address { get; set; }      
    }

    public class MosaicAliasTransaction : AliasTransaction
    {
        public MosaicAliasTransaction(string mosaicId, ulong namespaceId, byte aliasAction, bool embedded) : base(namespaceId, aliasAction, embedded)
        {
            MosaicId = DataConverter.ConvertToUInt64(mosaicId.FromHex());

        }

        public ulong MosaicId { get; set; }
    }
}
