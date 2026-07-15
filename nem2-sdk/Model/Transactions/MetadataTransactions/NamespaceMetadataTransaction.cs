

using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.MetadataTransactions
{
    public class NamespaceMetadataTransaction : VerifiableTransaction
    {
        public NamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded) : base(embedded) 
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey.FromHex();
            ValueSizeDelta = valueSize;
            ValueSize = valueSize;

            
            Type = TransactionTypes.Types.NAMESPACE_METADATA.GetValue();
        }

        public byte[] TargetAddress { get; set; }

        public byte[] ScopedMetadataKey { get; set; }

        public byte[] TargetNamespaceId { get; set; }

        public ushort ValueSizeDelta { get; set; }

        public ushort ValueSize { get; set; }

        public byte[] Value { get; set; }

        public override NamespaceMetadataTransaction SetSigner(string signer)
        {
            EntityBody.Signer = signer.FromHex();

            return this;
        }
    }
}
