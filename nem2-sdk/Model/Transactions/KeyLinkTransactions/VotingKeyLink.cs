using io.nem2.sdk.Infrastructure;

namespace io.nem2.sdk.Model.Transactions
{
    public class VotingKeyLinkTransaction : KeyLinkTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(base.LinkedPublicKey);
            serializer.SerializeProperty(StartEpoch);   
            serializer.SerializeProperty(EndEpoch);
            serializer.SerializeProperty(base.LinkAction);
        }

        public VotingKeyLinkTransaction(uint startEpoch, uint endEpoch, string linkedPublicKey, byte linkAction) : base(TransactionTypes.Types.VOTING_KEY_LINK, linkedPublicKey, linkAction)
        {     
            StartEpoch = startEpoch;
            EndEpoch = endEpoch;
        }

        public uint StartEpoch { get; set; }

        public uint EndEpoch { get; set; }

        internal override int AddSize()
        {
            return 41;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.VOTING_KEY_LINK;
        }
    }
}



       
        
