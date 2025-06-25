namespace io.nem2.sdk.Model2.Transactions.KeyLinkTransactions
{
    public class KeyLinkTransaction1 : Transaction1
    {
        public KeyLinkTransaction1(string linkedPublicKey, int linkAction, byte linkType)
        {
            LinkedPublicKey = linkedPublicKey;
            LinkAction = linkAction;
            LinkType = linkType;
        }

        public byte LinkType { get; set; }
        public string LinkedPublicKey { get; set; }
        public int LinkAction { get; set; }
    }
}
