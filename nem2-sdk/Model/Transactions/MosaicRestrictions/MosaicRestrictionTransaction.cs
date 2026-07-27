using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.MosaicRestrictions
{
    public class MosaicRestrictionTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId, 10);
            serializer.SerializeProperty(RestrictionKey, 11);
            serializer.SerializeProperty(PreviousRestrictionValue, 12);
            serializer.SerializeProperty(NewRestrictionValue, 13);
        }

        public MosaicRestrictionTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicRestrictionTransaction(TransactionTypes.Types type, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded) : base(type, embedded)
        {
            Version = 0x01;
            MosaicId = mosaicID.FromHex();
            RestrictionKey = restrictionKey.FromHex();
            PreviousRestrictionValue = previousRestrictionValue.FromHex();
            NewRestrictionValue = newRestrictionValue.FromHex();
        }

        public byte[] MosaicId { get; set; }

        public byte[] RestrictionKey { get; set; }

        public byte[] PreviousRestrictionValue { get; set; }

        public byte[] NewRestrictionValue { get; set; }

        public override MosaicRestrictionTransaction SetSigner(string signer)
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

    public class MosaicAddressRestrictionTransaction : MosaicRestrictionTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId, 10);
            serializer.SerializeProperty(RestrictionKey, 11);
            serializer.SerializeProperty(PreviousRestrictionValue, 12);
            serializer.SerializeProperty(NewRestrictionValue, 13);
            serializer.SerializeProperty(TargetAddress, 14);
        }

        public MosaicAddressRestrictionTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded) : base(TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
        {
            TargetAddress = targetAddress.IsBase32() ? AddressEncoder.DecodeAddress(targetAddress) : targetAddress.FromHex();
        }

        public byte[] TargetAddress { get; set; }
    }

    public class MosaicGlobalRestrictionTransaction : MosaicRestrictionTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId, 10);
            serializer.SerializeProperty(RestrictionKey, 11);
            serializer.SerializeProperty(PreviousRestrictionValue, 12);
            serializer.SerializeProperty(NewRestrictionValue, 13);
            serializer.SerializeProperty(PreviousRestrictionType, 14);
            serializer.SerializeProperty(NewRestrictionType, 15);
        }

        public MosaicGlobalRestrictionTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicGlobalRestrictionTransaction(string mosaicID, string referenceMosaicId, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, byte previousRestrictionType, byte newRestrictionType, bool embedded) : base(TransactionTypes.Types.MOSAIC_GLOBAL_RESTRICTION, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
        {
            ReferenceMosaicId = referenceMosaicId.FromHex();
        }

        public byte[] ReferenceMosaicId { get; set; }

        public byte PreviousRestrictionType { get; set; }

        public byte NewRestrictionType { get; set; }
    }
}
