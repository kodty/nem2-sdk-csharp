namespace io.nem2.sdk.Model2.Transactions.KeyLinkTransactions
{
    public class VotingKeyLinkTransaction1 : KeyLinkTransaction1
    {
        public VotingKeyLinkTransaction1(ulong startEpoch, ulong endEpoch, string linkedPublicKey, int linkAction ) : base (linkedPublicKey, linkAction)
        {
            StartEpoch = startEpoch;
            EndEpoch = endEpoch;
        }

        public ulong StartEpoch { get; set; }
        public ulong EndEpoch { get; set; }
    }
}
