using Coppery;

namespace io.nem2.sdk.Model.Transactions.MetadataTransactions
{
    public class MosaicMetadataTransaction : AccountMetadataTransaction
    {
        public MosaicMetadataTransaction(TransactionTypes.Types type) : base(TransactionTypes.Types.MOSAIC_METADATA) { }

        public MosaicMetadataTransaction(string targetAddress, string targetMosaicId, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value) : base(targetAddress,  scopedKey,  valueSizeDelta,  valueSize, value)
        {
            TargetMosaicId = targetMosaicId.FromHex();
        }

        [Order(14)]
        public byte[] TargetMosaicId { get; set; }

        [Obsolete("This transaction is only available as an aggregate embedded transaction", true)]
        public SignedTransaction WrapVerified(SecretKeyPair signer, string genHash)
        {
            return null;
        }
    }
}
