using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.MetadataTransactions
{
    public class AccountMetadataTransaction : VerifiableTransaction
    {
        public AccountMetadataTransaction(TransactionTypes.Types type) : base(type, true) { }

        public AccountMetadataTransaction(string targetAddress, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value) : base(TransactionTypes.Types.ACCOUNT_METADATA, true)
        {
            Version = 0x01;
            TargetAddress = targetAddress.IsBase32()
                      ? AddressEncoder.DecodeAddress(targetAddress)
                      : targetAddress.FromHex();

            Size += 12;

            ScopedMetadataKey = scopedKey.FromHex();
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            Value = value;

            Size += (uint)TargetAddress.Length;
            Size += (uint)Value.Length;
        }

        [Order(12)]
        public byte[] TargetAddress { get; set; }

        [Order(13)]
        public byte[] ScopedMetadataKey { get; set; }

        [Order(15)]
        public ushort ValueSizeDelta { get; set; }

        [Order(16)]
        public ushort ValueSize { get; set; }

        [Order(17)]
        public byte[] Value { get; set; }

        public override AccountMetadataTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        [Obsolete("This transaction is only available as an aggregate embedded transaction", true)]
        public SignedTransaction WrapVerified(SecretKeyPair signer, string genHash)
        {
            return null;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}
