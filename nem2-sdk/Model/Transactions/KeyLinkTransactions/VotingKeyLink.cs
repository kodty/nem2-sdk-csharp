using Coppery;

namespace io.nem2.sdk.Model.Transactions.KeyLinkTransactions
{
    public class VotingKeyLinkTransaction : KeyLinkTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(LinkedPublicKey);
            serializer.SerializeProperty(StartEpoch);   
            serializer.SerializeProperty(EndEpoch);
            serializer.SerializeProperty(LinkAction);
        }

        public VotingKeyLinkTransaction(uint startEpoch, uint endEpoch, string linkedPublicKey, byte linkAction, bool embedded) : base (TransactionTypes.Types.VOTING_KEY_LINK, embedded)
        {
            Version = 0x01;
           
            LinkedPublicKey = linkedPublicKey.FromHex();      
            StartEpoch = startEpoch;
            EndEpoch = endEpoch;
            LinkAction = linkAction;

            Size += 33 + 8;  
        }

        public uint StartEpoch { get; set; }

        public uint EndEpoch { get; set; }

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



       
        
