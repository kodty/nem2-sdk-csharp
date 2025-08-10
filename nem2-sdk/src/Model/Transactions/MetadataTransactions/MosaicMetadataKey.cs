using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.MetadataTransactions
{
    public class MosaicMetadataTransaction : AccountMetadataTransaction
    {
        public MosaicMetadataTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicMetadataTransaction(string targetAddress, string targetMosaicId, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value, bool embedded) : base(targetAddress,  scopedKey,  valueSizeDelta,  valueSize, value,  embedded)
        {
            TargetMosaicId = targetMosaicId.FromHex();
        }

        public byte[] TargetMosaicId { get; set; }
    }
}
