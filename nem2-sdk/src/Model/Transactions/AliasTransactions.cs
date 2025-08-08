using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class AliasTransaction : Transaction
    {
        public AliasTransaction(string namespaceId, byte aliasAction, bool embedded) : base(embedded)
        {
            AliasAction = aliasAction;
            NamespaceId = namespaceId.FromHex();
        }

        public byte[] NamespaceId { get; set; }

        public byte AliasAction { get; set; }
    }

    public class AddressAliasTransaction : AliasTransaction
    {
        public AddressAliasTransaction(string address, string namespaceId, byte aliasAction, bool embedded) : base(namespaceId, aliasAction, embedded)
        {
            Address = address;          
        }

        public string Address { get; set; }      
    }

    public class MosaicAliasTransaction : AliasTransaction
    {
        public MosaicAliasTransaction(string mosaicId, string namespaceId, byte aliasAction, bool embedded) : base(namespaceId, aliasAction, embedded)
        {
            MosaicId = mosaicId;

        }

        public string MosaicId { get; set; }
    }
}
