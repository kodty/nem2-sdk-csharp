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

        public NamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value) : base(TransactionTypes.Types.NAMESPACE_METADATA) 
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey.FromHex();
            TargetNamespaceId = targetNamespaceId.FromHex();
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            Value = value;
            
            Size += 24;
            Size += (uint)Value.Length;
            Size += (uint)TargetAddress.Length;
        }

        public byte[] TargetNamespaceId { get; set; }

        public override NamespaceMetadataTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        [Obsolete("This transaction is only available as an aggregate embedded transaction", true)]
        public new SignedTransaction WrapVerified(SecretKeyPair signer, string genHash)
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
