using Coppery;

namespace io.nem2.sdk.src.Model.Transactions.KeyLinkTransactions
{
    public class KeyLinkTransaction : Transaction
    {
        public KeyLinkTransaction(TransactionTypes.Types type, bool embedded) : base (type, embedded) { }

        public KeyLinkTransaction(string linkedPublicKey, byte linkAction, bool embedded) : base(embedded)
        {
            LinkedPublicKey = linkedPublicKey.FromHex();
            LinkAction = linkAction;
            Size += 33;
        }
        public byte[] LinkedPublicKey { get; set; }
        public byte LinkAction { get; set; }
    }
}
