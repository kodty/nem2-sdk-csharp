using io.nem2.sdk.Infrastructure;

namespace io.nem2.sdk.Model.Transactions
{
    public class KeyLinkTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(LinkedPublicKey);
            serializer.SerializeProperty(LinkAction);        
        }

        public KeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, byte linkAction)
        {
            LinkedPublicKey = linkedPublicKey.FromHex();
            LinkAction = linkAction;
            Type = type;
        }

        public byte[] LinkedPublicKey { get; set; }

        public byte LinkAction { get; set; }

        private TransactionTypes.Types Type { get; set; }

        internal override int AddSize()
        {
            return 33;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return Type;
        }
    }
}
