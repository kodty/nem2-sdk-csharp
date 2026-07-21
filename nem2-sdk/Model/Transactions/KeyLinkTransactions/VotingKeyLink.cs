using Coppery;

namespace io.nem2.sdk.Model.Transactions.KeyLinkTransactions
{
    public class VotingKeyLinkTransaction : VerifiableTransaction
    {
        public VotingKeyLinkTransaction(ulong startEpoch, ulong endEpoch, string linkedPublicKey, byte linkAction, bool embedded) : base (embedded)
        {
            LinkedPublicKey = linkedPublicKey.FromHex();      
            StartEpoch = startEpoch;
            EndEpoch = endEpoch;
            LinkAction = linkAction;
            Size += 16 + 33;

            
            Type = TransactionTypes.Types.VOTING_KEY_LINK.GetValue();
        }

        public byte[] LinkedPublicKey { get; set; }
        public ulong StartEpoch { get; set; }
        public ulong EndEpoch { get; set; }
        public byte LinkAction { get; set; }

        public override VotingKeyLinkTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }
    }
}



       
        
