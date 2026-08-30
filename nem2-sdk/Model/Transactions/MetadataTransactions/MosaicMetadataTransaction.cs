using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Model.Articles;

namespace io.nem2.sdk.Model.Transactions
{
    public class MosaicMetadataTransaction : AccountMetadataTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(base.TargetAddress);
            serializer.SerializeProperty(base.ScopedMetadataKey);
            serializer.SerializeProperty(TargetMosaicId);
            serializer.SerializeProperty(base.ValueSizeDelta);
            serializer.SerializeProperty(base.ValueSize);
            serializer.SerializeProperty(base.Value);
        }

        public MosaicMetadataTransaction(string targetAddress, Mosaic targetMosaicId, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value) 
            : base(targetAddress,  scopedKey,  valueSizeDelta,  valueSize, value)
        {
            TargetMosaicId = targetMosaicId.MosaicId.HexId.FromHex();
        }

        public byte[] TargetMosaicId { get; set; }

        internal override int AddSize()
        {
            return 44 + Value.Length;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.MOSAIC_METADATA;
        }
    }
}
