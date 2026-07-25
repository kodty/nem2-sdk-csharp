using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public class MultisigAccountModificationTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MinApprovalDelta, typeof(byte), 10);
            serializer.SerializeProperty(MinRemovalDelta, typeof(byte), 11);
            serializer.SerializeProperty(AddressAdditionsCount, typeof(byte), 12);
            serializer.SerializeProperty(AddressDeletionsCount, typeof(byte), 13);
            serializer.SerializeProperty(new byte[4], typeof(byte[]), 14);
            // serializer.SerializeProperty(AddressAdditions, typeof(ulong), 15);
            // serializer.SerializeProperty(AddressDeletions, typeof(ulong), 16);
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
