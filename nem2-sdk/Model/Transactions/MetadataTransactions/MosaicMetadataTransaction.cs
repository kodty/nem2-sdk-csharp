using Coppery;
using System.Reflection;

namespace io.nem2.sdk.Model.Transactions.MetadataTransactions
{
    public class MosaicMetadataTransaction : AccountMetadataTransaction
    {
        public override PropertyInfo[] RetrieveProperties()
        {
            return
            [
                .. BaseProperties,
                GetType().GetProperty("TargetAddress"),
                GetType().GetProperty("ScopedMetadataKey"),
                GetType().GetProperty("TargetMosaicId"),
                GetType().GetProperty("ValueSizeDelta"),
                GetType().GetProperty("ValueSize"),
                GetType().GetProperty("Value")
            ];
        }

        public MosaicMetadataTransaction(TransactionTypes.Types type) : base(TransactionTypes.Types.MOSAIC_METADATA) { }

        public MosaicMetadataTransaction(string targetAddress, string targetMosaicId, string scopedKey, ushort valueSizeDelta, ushort valueSize, byte[] value) : base(targetAddress,  scopedKey,  valueSizeDelta,  valueSize, value)
        {
            TargetMosaicId = targetMosaicId.FromHex();
        }

        public byte[] TargetMosaicId { get; set; }


        [Obsolete("This transaction is only available as an aggregate embedded transaction", true)]
        public SignedTransaction WrapVerified(SecretKeyPair signer, string genHash)
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
