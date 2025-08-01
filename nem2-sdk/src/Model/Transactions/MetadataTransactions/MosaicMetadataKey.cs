
using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.MetadataTransactions
{
    public class MosaicMetadataTransaction1 : Transaction
    {
        public MosaicMetadataTransaction1(string targetAddress, string targetMosaicId, string scopedKey, short valueSizeDelta, short valueSize, byte[] value, bool embedded) : base(embedded)
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey;
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            Value = value;
            TargetMosaicId = targetMosaicId;
        }
        public byte[] TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }
        public string TargetMosaicId { get; set; }

        public short ValueSizeDelta { get; set; }

        public short ValueSize { get; set; }

        public byte[] Value { get; set; }
    }
}
