using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Transactions
{
    public class AccountMetadataTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(TargetAddress);
            serializer.SerializeProperty(ScopedMetadataKey);
            serializer.SerializeProperty(ValueSizeDelta);
            serializer.SerializeProperty(ValueSize);
            serializer.SerializeProperty(Value);
        }

        public AccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value) 
        {
            TargetAddress = targetAddress.IsBase32()
                      ? Address.DecodeAddress(targetAddress)
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

        internal override int AddSize()
        {
            return 36 + Value.Length;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.ACCOUNT_METADATA;
        }
    }
}
