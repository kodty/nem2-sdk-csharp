

using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.MetadataTransactions
{
    public class NamespaceMetadataTransaction : Transaction
    {
        public NamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, short valueSizeDelta, short valueSize, byte[] value, bool embedded) : base(embedded) 
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey;
            ValueSizeDelta = valueSize;
            ValueSize = valueSize;
        }

        public byte[] TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public string TargetNamespaceId { get; set; }

        public short ValueSizeDelta { get; set; }

        public short ValueSize { get; set; }

        public byte[] Value { get; set; }
    }
}
