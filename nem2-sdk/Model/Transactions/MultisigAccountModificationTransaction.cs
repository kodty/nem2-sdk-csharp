using Coppery;

namespace io.nem2.sdk.Model.Transactions
{
    public class MultisigAccountModificationTransaction : VerifiableTransaction
    {
        public byte MinApprovalDelta { get; set; }
        public byte MinRemovalDelta { get; set; }
        public byte AddressAdditionsCount { get; set; }
        public byte AddressDeletionsCount { get; set; }
        public int Multisig_​account_​modification_​transaction_​body_​reserved_​1 { get; set; }
        public string[] AddressAdditions { get; set; }
        public string[] AddressDeletions { get; set; }
        public MultisigAccountModificationTransaction(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions) : base(TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION, true)
        {

            Version = 0x01;
            MinApprovalDelta = minApproval;
            MinRemovalDelta = minRemoval;
            AddressAdditionsCount = (byte)AddressAdditions.Length;
            AddressDeletionsCount = (byte)AddressDeletions.Length;
            Multisig_account_modification_transaction_body_reserved_1 = 0;
            AddressAdditions = addressAdditions;
            AddressDeletions = addressDeletions;

            
            Type = TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION.GetValue();        
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
