using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.src.Model2.Transactions.AccountRestrictions
{
    //AccountMosaic, AccountAddress, AccountOperation
    public class AccountRestrictionsTransaction1 : Transaction1
    {
        public AccountRestrictionsTransaction1(TransactionTypes.Types type, int restrictionFlags, string[] restrictionAdditions, string[] restrictionsDeletions) : base(type)
        {
            RestrictionFlags = restrictionFlags;
            RestrictionAdditions = restrictionAdditions;
            RestrictionDeletions = restrictionsDeletions;
        }

        public int RestrictionFlags { get; set; }
        public int RestrictionsAdditionsCount { get; set; }
        public int RestrictionsDeletionsCount { get; set; }
        public int Account_​restriction_​transaction_​body_​reserved_​1 { get; set; }
        public string[] RestrictionAdditions { get; set; }
        public string[] RestrictionDeletions { get; set; }
    }
}
