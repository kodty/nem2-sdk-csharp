using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.MosaicRestrictions
{
    public class MosaicRestrictionTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(RestrictionKey);
            serializer.SerializeProperty(PreviousRestrictionValue);
            serializer.SerializeProperty(NewRestrictionValue);
        }

        public MosaicRestrictionTransaction(TransactionTypes.Types type, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue)
        {
            MosaicId = mosaicID.FromHex();
            RestrictionKey = restrictionKey.FromHex();
            PreviousRestrictionValue = previousRestrictionValue.FromHex();
            NewRestrictionValue = newRestrictionValue.FromHex();
        }

        public byte[] MosaicId { get; set; }

        public byte[] RestrictionKey { get; set; }

        public byte[] PreviousRestrictionValue { get; set; }

        public byte[] NewRestrictionValue { get; set; }

        internal override int AddSize()
        {
            return MosaicId.Length + RestrictionKey.Length + PreviousRestrictionValue.Length + NewRestrictionValue.Length;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION;
        }
    }

    public class MosaicAddressRestrictionTransaction : MosaicRestrictionTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(RestrictionKey);
            serializer.SerializeProperty(PreviousRestrictionValue);
            serializer.SerializeProperty(NewRestrictionValue);
            serializer.SerializeProperty(TargetAddress);
        }

        public MosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue) : base(TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue)
        {
            TargetAddress = targetAddress.IsBase32() ? AddressEncoder.DecodeAddress(targetAddress) : targetAddress.FromHex();
        }

        public byte[] TargetAddress { get; set; }

        internal override int AddSize()
        {
            return MosaicId.Length + RestrictionKey.Length + PreviousRestrictionValue.Length + NewRestrictionValue.Length + TargetAddress.Length;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION;
        }
    }

    public class MosaicGlobalRestrictionTransaction : MosaicRestrictionTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(RestrictionKey);
            serializer.SerializeProperty(PreviousRestrictionValue);
            serializer.SerializeProperty(NewRestrictionValue);
            serializer.SerializeProperty(PreviousRestrictionType);
            serializer.SerializeProperty(NewRestrictionType);
        }

        public MosaicGlobalRestrictionTransaction(string mosaicID, string referenceMosaicId, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, byte previousRestrictionType, byte newRestrictionType) : base(TransactionTypes.Types.MOSAIC_GLOBAL_RESTRICTION, mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue)
        {
            ReferenceMosaicId = referenceMosaicId.FromHex();
        }

        public byte[] ReferenceMosaicId { get; set; }

        public byte PreviousRestrictionType { get; set; }

        public byte NewRestrictionType { get; set; }

        internal override int AddSize()
        {
            return MosaicId.Length + RestrictionKey.Length + PreviousRestrictionValue.Length + NewRestrictionValue.Length + 2;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.MOSAIC_GLOBAL_RESTRICTION;
        }
    }
}
