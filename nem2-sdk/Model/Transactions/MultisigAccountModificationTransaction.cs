using io.nem2.sdk.Infrastructure;

namespace io.nem2.sdk.Model.Transactions
{
    public class MultisigAccountModificationTransaction : TransactionExtension
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

        public MultisigAccountModificationTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions)
        {
            MinApprovalDelta = minApproval;
            MinRemovalDelta = minRemoval;
            AddressAdditionsCount = (byte)AddressAdditions.Length;
            AddressDeletionsCount = (byte)AddressDeletions.Length;
            AddressAdditions = addressAdditions;
            AddressDeletions = addressDeletions;    
        }

        internal override int AddSize()
        {
            return 00000000000;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION;
        }
    }
}
