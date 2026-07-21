using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions
{
    public class AliasTransaction : VerifiableTransaction
    {
        public AliasTransaction(ulong namespaceId, byte aliasAction, bool embedded) : base(embedded)
        {
            AliasAction = aliasAction;
            NamespaceId = namespaceId;

            
        }

        public ulong NamespaceId { get; set; }

        public byte AliasAction { get; set; }

        public override AliasTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

    }

    public class AddressAliasTransaction : AliasTransaction
    {
        public AddressAliasTransaction(string address, ulong namespaceId, byte aliasAction, bool embedded) : base(namespaceId, aliasAction, embedded)
        {
            Address = address.IsBase32()
                      ? AddressEncoder.DecodeAddress(address)
                      : address.FromHex();

            Type = TransactionTypes.Types.ADDRESS_ALIAS.GetValue();
        }

        public byte[] Address { get; set; }

        public override AddressAliasTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }
    }

    public class MosaicAliasTransaction : AliasTransaction
    {
        public MosaicAliasTransaction(string mosaicId, ulong namespaceId, byte aliasAction, bool embedded) : base(namespaceId, aliasAction, embedded)
        {
            MosaicId = DataConverter.ConvertTo<ulong>(mosaicId.FromHex());

            Type = TransactionTypes.Types.MOSAIC_ALIAS.GetValue();
        }

        public ulong MosaicId { get; set; }

        public override MosaicAliasTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }
    }
}
