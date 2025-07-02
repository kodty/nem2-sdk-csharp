using io.nem2.sdk.src.Model2.Transactions;

namespace io.nem2.sdk.src.Model2.Transactions.KeyLinkTransactions
{
    public class KeyLinkTransaction1 : Transaction1
    {
        public KeyLinkTransaction1(string linkedPublicKey, int linkAction)
        {
            LinkedPublicKey = linkedPublicKey;
            LinkAction = linkAction;
        }
        public string LinkedPublicKey { get; set; }
        public int LinkAction { get; set; }
    }
}
