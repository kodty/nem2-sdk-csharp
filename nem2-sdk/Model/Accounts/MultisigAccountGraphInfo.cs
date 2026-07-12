namespace io.nem2.sdk.Model.Accounts
{
    public class MultisigAccountGraphInfo
    {
        public Dictionary<int, List<MultisigAccountInfo>> MultisigAccounts { get; }

        public MultisigAccountGraphInfo(Dictionary<int, List<MultisigAccountInfo>> multisigAccounts)
        {
            MultisigAccounts = multisigAccounts;
        }

        public Dictionary<int, List<MultisigAccountInfo>>.KeyCollection GetLevelsNumber()
        {
            return MultisigAccounts.Keys;
        }
    }
}
