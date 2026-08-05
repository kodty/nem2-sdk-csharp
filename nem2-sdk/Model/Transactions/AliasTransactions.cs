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
}
