using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions
{
    public class AliasTransaction : VerifiableTransaction
    {
        public AliasTransaction(TransactionTypes.Types type, ulong namespaceId, byte aliasAction, bool embedded) : base(type, embedded)
        {
            AliasAction = aliasAction;
            NamespaceId = namespaceId;
            Size += 9;
        }

        [Order(12)]
        public ulong NamespaceId { get; set; }

        [Order(13)]
        public byte AliasAction { get; set; }

        public override AliasTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();
            Version = 0x01;
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
        public AddressAliasTransaction(string address, ulong namespaceId, byte aliasAction, bool embedded) : base(TransactionTypes.Types.ADDRESS_ALIAS, namespaceId, aliasAction, embedded)
        {
            Address = address.IsBase32()
                      ? AddressEncoder.DecodeAddress(address)
                      : address.FromHex();
            Size += 8;
        }

        [Order(14)]
        public byte[] Address { get; set; }
    }

    public class MosaicAliasTransaction : AliasTransaction
    {
        public MosaicAliasTransaction(string mosaicId, ulong namespaceId, byte aliasAction, bool embedded) : base(TransactionTypes.Types.MOSAIC_ALIAS, namespaceId, aliasAction, embedded)
        {
            MosaicId = DataConverter.ConvertTo<ulong>(mosaicId.FromHex().Reverse().ToArray());
           
            Size += 8;
        }

        [Order(14)]
        public ulong MosaicId { get; set; }
    }
}
