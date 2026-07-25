using Coppery;

namespace io.nem2.sdk.Model.Transactions.KeyLinkTransactions
{
    public class KeyLinkTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(LinkedPublicKey, typeof(byte[]), 10);
            serializer.SerializeProperty(LinkAction, typeof(byte), 11);
            
        }

        public KeyLinkTransaction(TransactionTypes.Types type, bool embedded) : base (type, embedded) { }

        public KeyLinkTransaction(TransactionTypes.Types type, string linkedPublicKey, byte linkAction, bool embedded) : base(type, embedded)
        {
            Version = 0x01;
            LinkedPublicKey = linkedPublicKey.FromHex();
            LinkAction = linkAction;
            Size += 33;
        }

        public byte[] LinkedPublicKey { get; set; }

        public byte LinkAction { get; set; }

        public override KeyLinkTransaction SetSigner(string signer)
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
