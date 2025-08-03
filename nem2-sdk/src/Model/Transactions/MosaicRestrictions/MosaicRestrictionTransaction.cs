namespace io.nem2.sdk.src.Model.Transactions.MosaicRestrictions
{
    public class MosaicAddressRestrictionTransaction : MosaicRestrictionTransaction
    {
        public string TargetAddress { get; set; }
        public MosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded) : base(mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
        {
            TargetAddress = targetAddress;
        }
    }
    public class MosaicGlobalRestrictionTransaction : MosaicRestrictionTransaction
    {
        public string ReferenceMosaicId { get; set; }
        public MosaicGlobalRestrictionTransaction(string mosaicID, string referenceMosaicId, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded) : base(mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue, embedded)
        {
            ReferenceMosaicId = referenceMosaicId;
        }
    }

    public class MosaicRestrictionTransaction : Transaction
    {
        public MosaicRestrictionTransaction(string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue, bool embedded) : base(embedded)
        {
            MosaicID = mosaicID;
            RestrictionKey = restrictionKey;
            PreviousRestrictionValue = previousRestrictionValue;
            NewRestrictionValue = newRestrictionValue;
            
        }

        public string MosaicID { get; set; }
        public string RestrictionKey { get; set; }
        public string PreviousRestrictionValue { get; set; }
        public string NewRestrictionValue { get; set; }
    }
}
