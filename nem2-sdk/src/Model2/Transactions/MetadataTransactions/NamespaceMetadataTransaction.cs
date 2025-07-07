using io.nem2.sdk.src.Export;

namespace io.nem2.sdk.src.Model2.Transactions.MetadataTransactions
{
    public class NamespaceMetadataTransaction1 : Transaction1
    {
        public NamespaceMetadataTransaction1(string targetAddress, string scopedKey, string targetNamespaceId, short valueSizeDelta, short valueSize, byte[] value) 
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
