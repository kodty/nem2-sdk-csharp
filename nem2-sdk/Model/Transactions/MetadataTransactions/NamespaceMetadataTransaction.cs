using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.MetadataTransactions
{
    public class NamespaceMetadataTransaction : AccountMetadataTransaction
    {
        public NamespaceMetadataTransaction(string targetAddress, string scopedKey, string targetNamespaceId, ushort valueSizeDelta, ushort valueSize, byte[] value) : base(TransactionTypes.Types.NAMESPACE_METADATA) 
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey.FromHex();
            TargetNamespaceId = targetNamespaceId.FromHex();
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            Value = value;
            
            VerifiableEntity.Size += 24;
            VerifiableEntity.Size += (uint)Value.Length;
            VerifiableEntity.Size += (uint)TargetAddress.Length;
        }

        [Order(14)]
        public byte[] TargetNamespaceId { get; set; }

        public override NamespaceMetadataTransaction SetSigner(string signer)
        {
            EntityBody.Signer = signer.FromHex();

            return this;
        }

        [Obsolete("This transaction is only available as an aggregate embedded transaction", true)]
        public SignedTransaction WrapVerified(SecretKeyPair signer, string genHash)
        {
            return null;
        }
    }
}
