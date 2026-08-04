using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.MetadataTransactions
{
    public class NamespaceMetadataTransaction : AccountMetadataTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(TargetAddress);
            serializer.SerializeProperty(ScopedMetadataKey);
            serializer.SerializeProperty(TargetNamespaceId);
            serializer.SerializeProperty(ValueSizeDelta);
            serializer.SerializeProperty(ValueSize);
            serializer.SerializeProperty(Value);
        }

        public NamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value) : base(targetAddress, scopedKey, valueSizeDelta, valueSize, value) 
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey.FromHex();
            TargetNamespaceId = targetNamespaceId.FromHex();
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            Value = value;
        }

        public byte[] TargetNamespaceId { get; set; }

        internal override int AddSize()
        {
            return TargetAddress.Length + ScopedMetadataKey.Length + TargetNamespaceId.Length + 4 + Value.Length;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.NAMESPACE_METADATA;
        }
    }
}
