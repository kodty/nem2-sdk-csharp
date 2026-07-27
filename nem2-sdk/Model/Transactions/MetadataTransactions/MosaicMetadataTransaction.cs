using Coppery;

namespace io.nem2.sdk.Model.Transactions.MetadataTransactions
{
    public class MosaicMetadataTransaction : AccountMetadataTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(TargetAddress, 10);
            serializer.SerializeProperty(ScopedMetadataKey, 11);
            serializer.SerializeProperty(TargetMosaicId, 12);
            serializer.SerializeProperty(ValueSizeDelta, 13);
            serializer.SerializeProperty(ValueSize, 14);
            serializer.SerializeProperty(Value, 15);
        }

        public MosaicMetadataTransaction(TransactionTypes.Types type) : base(TransactionTypes.Types.MOSAIC_METADATA) { }

        public MosaicMetadataTransaction(string targetAddress, string targetMosaicId, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value) : base(targetAddress,  scopedKey,  valueSizeDelta,  valueSize, value)
        {
            TargetMosaicId = targetMosaicId.FromHex();
        }

        public byte[] TargetMosaicId { get; set; }


        [Obsolete("This transaction is only available as an aggregate embedded transaction", true)]
        public new SignedTransaction WrapVerified(SecretKeyPair signer, string genHash)
        {
            return null;
        }

        public override AccountMetadataTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}
