namespace io.nem2.sdk.src.Model.Transactions.KeyLinkTransactions
{
    public class KeyLinkTransaction1 : Transaction
    {
        public KeyLinkTransaction1(string linkedPublicKey, int linkAction, bool embedded) : base(embedded)
        {
            LinkedPublicKey = linkedPublicKey;
            LinkAction = linkAction;
        }
        public string LinkedPublicKey { get; set; }
        public int LinkAction { get; set; }
    }
}
