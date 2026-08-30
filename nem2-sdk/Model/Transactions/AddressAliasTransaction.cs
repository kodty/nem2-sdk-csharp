using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Transactions
{
    public class AddressAliasTransaction : AliasTransaction
    {
        public AddressAliasTransaction(string address, ulong namespaceId, byte aliasAction) : base(TransactionTypes.Types.ADDRESS_ALIAS, namespaceId, aliasAction)
        {
            AddressToAlias = address.IsBase32()
                      ? Address.DecodeAddress(address)
                      : address.FromHex();
        }

        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(NamespaceId);
            serializer.SerializeProperty(AddressToAlias);
            serializer.SerializeProperty(AliasAction);
        }

        public byte[] AddressToAlias { get; set; }

        internal override int AddSize()
        {
            return 33;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.ADDRESS_ALIAS;
        }
    }
}
