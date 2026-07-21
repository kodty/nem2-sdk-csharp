using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions
{
    public class AliasTransaction : VerifiableTransaction
    {
        public AliasTransaction(TransactionTypes.Types type, string namespaceId, byte aliasAction, bool embedded) : base(type, embedded)
        {
            Version = 0x01;
            AliasAction = aliasAction;
            NamespaceId = namespaceId.FromHex().Reverse().ToArray();
            Size += 9;
        }

        [Order(12)]
        public byte[] NamespaceId { get; set; }

        [Order(14)]
        public byte AliasAction { get; set; }

        public override AliasTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();
           
            return this;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }

    public class AddressAliasTransaction : AliasTransaction
    {
        public AddressAliasTransaction(string address, string namespaceId, byte aliasAction, bool embedded) : base(TransactionTypes.Types.ADDRESS_ALIAS, namespaceId, aliasAction, embedded)
        {
            Address = address.IsBase32()
                      ? AddressEncoder.DecodeAddress(address)
                      : address.FromHex();
            Size += 8;
        }

        [Order(13)]
        public byte[] Address { get; set; }
    }

    public class MosaicAliasTransaction : AliasTransaction
    {
        public MosaicAliasTransaction(string mosaicId, string namespaceId, byte aliasAction, bool embedded) : base(TransactionTypes.Types.MOSAIC_ALIAS, namespaceId, aliasAction, embedded)
        {
            MosaicId = mosaicId.FromHex().Reverse().ToArray();
           
            Size += 8;
        }

        [Order(13)]
        public byte[] MosaicId { get; set; }
    }
}
