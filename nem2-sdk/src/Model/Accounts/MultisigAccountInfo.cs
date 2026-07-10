namespace io.nem2.sdk.src.Model.Accounts
{

    public class MultisigAccountInfo
    {
       
        public PublicAccount Account { get; }
       
        public int MinApproval { get; }
        
        public int MinRemoval { get; }
       
        public List<PublicAccount> Cosignatories { get; }
       
        public List<PublicAccount> MultisigAccounts { get; }
        
        public bool HasCosigners(PublicAccount account) => Cosignatories.Contains(account);
        
        public bool IsCosignerOfMultisigAccount(PublicAccount account) => MultisigAccounts.Contains(account);
       
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
