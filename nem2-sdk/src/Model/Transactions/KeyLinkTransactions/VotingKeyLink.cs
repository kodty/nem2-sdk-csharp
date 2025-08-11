namespace io.nem2.sdk.src.Model.Transactions.KeyLinkTransactions
{
    public class VotingKeyLinkTransaction : KeyLinkTransaction
    {
        public VotingKeyLinkTransaction(ulong startEpoch, ulong endEpoch, string linkedPublicKey, byte linkAction, bool embedded) : base (linkedPublicKey, linkAction, embedded)
        {
            StartEpoch = startEpoch;
            EndEpoch = endEpoch;
        }

        public ulong StartEpoch { get; set; }
        public ulong EndEpoch { get; set; }
    }
}
