using io.nem2.sdk.src.Model.Accounts;

namespace io.nem2.sdk.src.Model2.Accounts
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
