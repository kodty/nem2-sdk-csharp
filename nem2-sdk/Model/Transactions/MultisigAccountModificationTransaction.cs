using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public class MultisigAccountModificationTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MinApprovalDelta);
            serializer.SerializeProperty(MinRemovalDelta);
            serializer.SerializeProperty(AddressAdditionsCount);
            serializer.SerializeProperty(AddressDeletionsCount);
            serializer.SerializeProperty(new byte[4]);
            // serializer.SerializeProperty(AddressAdditions);
            // serializer.SerializeProperty(AddressDeletions);
            throw new Exception("not implimented");
        }

        public byte MinApprovalDelta { get; set; }
        public byte MinRemovalDelta { get; set; }
        public byte AddressAdditionsCount { get; set; }
        public byte AddressDeletionsCount { get; set; }
        public string[] AddressAdditions { get; set; }
        public string[] AddressDeletions { get; set; }

        public MultisigAccountModificationTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions) : base(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION, true)
        {

            Version = 0x01;
            MinApprovalDelta = minApproval;
            MinRemovalDelta = minRemoval;
            AddressAdditionsCount = (byte)AddressAdditions.Length;
            AddressDeletionsCount = (byte)AddressDeletions.Length;
            AddressAdditions = addressAdditions;
            AddressDeletions = addressDeletions;    
        }

        public override MultisigAccountModificationTransaction SetSigner(string signer)
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
