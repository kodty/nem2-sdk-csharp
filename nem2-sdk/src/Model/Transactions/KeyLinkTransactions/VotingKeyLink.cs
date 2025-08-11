using Coppery;

namespace io.nem2.sdk.src.Model.Transactions.KeyLinkTransactions
{
    public class VotingKeyLinkTransaction : Transaction
    {
        public VotingKeyLinkTransaction(ulong startEpoch, ulong endEpoch, string linkedPublicKey, byte linkAction, bool embedded) : base (embedded)
        {
            LinkedPublicKey = linkedPublicKey.FromHex();      
            StartEpoch = startEpoch;
            EndEpoch = endEpoch;
            LinkAction = linkAction;
            Size += 16 + 33;
        }

        public byte[] LinkedPublicKey { get; set; }
        public ulong StartEpoch { get; set; }
        public ulong EndEpoch { get; set; }
        public byte LinkAction { get; set; }
    }
}



       
        
