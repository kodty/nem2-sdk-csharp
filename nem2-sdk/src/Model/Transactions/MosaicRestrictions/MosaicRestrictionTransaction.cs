using Coppery;

namespace io.nem2.sdk.src.Model.Transactions.MosaicRestrictions
{
    public class MosaicAddressRestrictionTransaction : MosaicRestrictionTransaction
    {
        public MosaicAddressRestrictionTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded) : base(mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
        {
            TargetAddress = targetAddress.IsBase32() ? AddressEncoder.DecodeAddress(targetAddress) : targetAddress.FromHex();
        }

        public byte[] TargetAddress { get; set; }
    }

    public class MosaicGlobalRestrictionTransaction : MosaicRestrictionTransaction
    {
        public MosaicGlobalRestrictionTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicGlobalRestrictionTransaction(string mosaicID, string referenceMosaicId, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded) : base(mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
        {
            ReferenceMosaicId = referenceMosaicId.FromHex();
        }

        public byte[] ReferenceMosaicId { get; set; }
    }

    public class MosaicRestrictionTransaction : Transaction
    {
        public MosaicRestrictionTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicRestrictionTransaction(string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded) : base(embedded)
        {
            MosaicID = mosaicID.FromHex();
            RestrictionKey = restrictionKey.FromHex();
            PreviousRestrictionValue = previousRestrictionValue.FromHex();
            NewRestrictionValue = newRestrictionValue.FromHex();
            
        }

        public byte[] MosaicID { get; set; }
        public byte[] RestrictionKey { get; set; }
        public byte[] PreviousRestrictionValue { get; set; }
        public byte[] NewRestrictionValue { get; set; }
    }
}
