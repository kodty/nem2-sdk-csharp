using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.KeyLinkTransactions
{
    public class KeyLinkTransaction : Transaction
    {
        public KeyLinkTransaction(TransactionTypes.Types type, bool embedded) : base (type, embedded) { }

        public KeyLinkTransaction(string linkedPublicKey, int linkAction, bool embedded) : base(embedded)
        {
            LinkedPublicKey = linkedPublicKey.FromHex();
            LinkAction = linkAction;
        }
        public byte[] LinkedPublicKey { get; set; }
        public int LinkAction { get; set; }
    }
}
