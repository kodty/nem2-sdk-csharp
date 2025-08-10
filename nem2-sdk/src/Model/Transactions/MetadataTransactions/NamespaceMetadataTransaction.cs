

using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.MetadataTransactions
{
    public class NamespaceMetadataTransaction : Transaction
    {
        public NamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded) : base(embedded) 
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey.FromHex();
            ValueSizeDelta = valueSize;
            ValueSize = valueSize;
        }

        public byte[] TargetAddress { get; set; }

        public byte[] ScopedMetadataKey { get; set; }

        public byte[] TargetNamespaceId { get; set; }

        public ushort ValueSizeDelta { get; set; }

        public ushort ValueSize { get; set; }

        public byte[] Value { get; set; }
    }
}
