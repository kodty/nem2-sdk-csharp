using Coppery;
using System.Reflection;

namespace io.nem2.sdk.Model.Transactions.KeyLinkTransactions
{
    public class VotingKeyLinkTransaction : KeyLinkTransaction
    {
        public override PropertyInfo[] RetrieveProperties()
        {
            return
            [
                .. BaseProperties,
                GetType().GetProperty("StartEpoch"),
                GetType().GetProperty("LinkedPublicKey"),
                GetType().GetProperty("LinkAction​"),
                GetType().GetProperty("EndEpoch")
            ];
        }

        public VotingKeyLinkTransaction(ulong startEpoch, ulong endEpoch, string linkedPublicKey, byte linkAction, bool embedded) : base (TransactionTypes.Types.VOTING_KEY_LINK, embedded)
        {
            Version = 0x01;
            LinkedPublicKey = linkedPublicKey.FromHex();      
            StartEpoch = startEpoch;
            EndEpoch = endEpoch;
            LinkAction = linkAction;
            Size += 16 + 33;  
        }

        public ulong StartEpoch { get; set; }

        public ulong EndEpoch { get; set; }

        public override VotingKeyLinkTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}



       
        
