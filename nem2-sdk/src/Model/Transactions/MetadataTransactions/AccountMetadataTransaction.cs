

using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.MetadataTransactions
{
    public class AccountMetadataTransaction1 : Transaction
    {
        public AccountMetadataTransaction1(string targetAddress, string scopedKey, short valueSizeDelta, short valueSize, byte[] value, bool embedded) : base(embedded)
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey;
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            Value = value;
        }
        public byte[] TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public short ValueSizeDelta { get; set; }

        public short ValueSize { get; set; }

        public byte[] Value { get; set; }
    }
}
