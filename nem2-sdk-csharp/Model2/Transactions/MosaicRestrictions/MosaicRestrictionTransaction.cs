namespace io.nem2.sdk.Model2.Transactions.MosaicRestrictions
{
    public class MosaicAddressRestrictionTransaction : MosaicRestrictionTransaction1
    {
        public string TargetAddress { get; set; }
        public MosaicAddressRestrictionTransaction(string targetAddress, string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue) : base(mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue)
        {
            TargetAddress = targetAddress;
        }
    }
    public class MosaicGlobalRestrictionTransaction : MosaicRestrictionTransaction1
    {
        public string ReferenceMosaicId { get; set; }
        public MosaicGlobalRestrictionTransaction(string mosaicID, string referenceMosaicId, string restrictionKey, string previousRestrictionValue, string newRestrictionValue) : base(mosaicID, restrictionKey, previousRestrictionValue, newRestrictionValue)
        {
            ReferenceMosaicId = referenceMosaicId;
        }
    }
    public class MosaicRestrictionTransaction1 : Transaction1
    {
        public MosaicRestrictionTransaction1(string mosaicID, string restrictionKey, string previousRestrictionValue, string newRestrictionValue) 
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
