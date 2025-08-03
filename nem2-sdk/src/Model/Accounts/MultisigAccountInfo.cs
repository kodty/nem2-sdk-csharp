namespace io.nem2.sdk.src.Model.Accounts
{

    public class MultisigAccountInfo
    {
       
        public PublicAccount Account { get; }
       
        public int MinApproval { get; }
        
        public int MinRemoval { get; }
       
        public List<PublicAccount> Cosignatories { get; }
       
        public List<PublicAccount> MultisigAccounts { get; }
        /// <summary>
        /// Checks if an account is cosignatory of the multisig account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns><c>true</c> if the specified account has cosigners; otherwise, <c>false</c>.</returns>
        public bool HasCosigners(PublicAccount account) => Cosignatories.Contains(account);
        /// <summary>
        /// Checks if the multisig account is cosignatory of an account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns><c>true</c> if [is cosigner of multisig account] [the specified account]; otherwise, <c>false</c>.</returns>
        public bool IsCosignerOfMultisigAccount(PublicAccount account) => MultisigAccounts.Contains(account);
        /// <summary>
        /// Checks if the account is a multisig account.
        /// </summary>
        /// <value><c>true</c> if this instance is multisig; otherwise, <c>false</c>.</value>
        public bool IsMultisig => MinApproval != 0 && MinRemoval != 0;

        
        public MultisigAccountInfo(PublicAccount account, int minApproval, int minRemoval, List<PublicAccount> cosignatories, List<PublicAccount> multisigAccounts)
        {
            Account = account;
            MinApproval = minApproval;
            MinRemoval = minRemoval;
            Cosignatories = cosignatories;
            MultisigAccounts = multisigAccounts;
        }
    }
}
