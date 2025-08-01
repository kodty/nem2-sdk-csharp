namespace io.nem2.sdk.src.Model.Transactions
{
    public class AliasTransaction1 : Transaction
    {
        public AliasTransaction1(string namespaceId, byte aliasAction, bool embedded) : base(embedded)
        {
            AliasAction = aliasAction;
            NamespaceId = namespaceId;
        }

        public string NamespaceId { get; set; }

        public byte AliasAction { get; set; }
    }

    public class AddressAliasTransaction1 : AliasTransaction1
    {
        public AddressAliasTransaction1(string address, string namespaceId, byte aliasAction, bool embedded) : base(namespaceId, aliasAction, embedded)
        {
            Address = address;          
        }

        public string Address { get; set; }      
    }

    public class MosaicAliasTransaction1 : AliasTransaction1
    {
        public MosaicAliasTransaction1(string mosaicId, string namespaceId, byte aliasAction, bool embedded) : base(namespaceId, aliasAction, embedded)
        {
            MosaicId = mosaicId;

        }

        public string MosaicId { get; set; }
    }
}
