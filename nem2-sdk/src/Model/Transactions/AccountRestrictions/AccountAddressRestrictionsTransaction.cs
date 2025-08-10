using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.AccountRestrictions
{
    //AccountMosaic, AccountAddress, AccountOperation
    public class AccountRestrictionsTransaction : Transaction
    {
        public AccountRestrictionsTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded)
        {

        }

        public AccountRestrictionsTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] restrictionAdditions, string[] restrictionsDeletions, bool embedded) : base(type, embedded)
        {
            RestrictionFlags = restrictionFlags;
            _RestrictionAdditions = [];
            _RestrictionDeletions = [];
            RestrictionAdditions = restrictionAdditions;
            RestrictionsAdditionsCount = (byte)restrictionAdditions.Count();
            RestrictionDeletions = restrictionsDeletions;
            RestrictionsDeletionsCount = (byte)restrictionsDeletions.Count();
            Account_​restriction_​transaction_​body_​reserved_​1 = 0;
        }

        public ushort RestrictionFlags { get; set; }
        public byte RestrictionsAdditionsCount { get; set; }
        public byte RestrictionsDeletionsCount { get; set; }
        public uint Account_​restriction_​transaction_​body_​reserved_​1 { get; set; }
        public byte[] _RestrictionAdditions{ get; set; }
        public byte[] _RestrictionDeletions{ get; set; }       

        private string[]? RestrictionDeletions { get { return null; } set => _RestrictionDeletions = DataSerializer.CompileValues(value); }
        private string[]? RestrictionAdditions { get { return null; } set => _RestrictionAdditions = DataSerializer.CompileValues(value); }

        
    }
}
