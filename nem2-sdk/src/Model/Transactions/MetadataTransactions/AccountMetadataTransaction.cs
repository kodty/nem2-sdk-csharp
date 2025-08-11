using Coppery;

namespace io.nem2.sdk.src.Model.Transactions.MetadataTransactions
{
    public class AccountMetadataTransaction : Transaction
    {
        public AccountMetadataTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public AccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded) : base(embedded)
        {
            TargetAddress = targetAddress.IsBase32()
                      ? AddressEncoder.DecodeAddress(targetAddress)
                      : targetAddress.FromHex();
            ScopedMetadataKey = scopedKey.FromHex();
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            Value = value;
        }

        public byte[] TargetAddress { get; set; }

        public byte[] ScopedMetadataKey { get; set; }

        public ushort ValueSizeDelta { get; set; }

        public ushort ValueSize { get; set; }

        public byte[] Value { get; set; }
    }
}
