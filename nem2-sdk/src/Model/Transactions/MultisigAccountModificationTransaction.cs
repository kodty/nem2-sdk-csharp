namespace io.nem2.sdk.src.Model.Transactions
{
    public class MultisigAccountModificationTransaction1 : Transaction
    {
        public byte MinApprovalDelta { get; set; }
        public byte MinRemovalDelta { get; set; }
        public byte AddressAdditionsCount { get; set; }
        public byte AddressDeletionsCount { get; set; }
        public int Multisig_​account_​modification_​transaction_​body_​reserved_​1 { get; set; }
        public string[] AddressAdditions { get; set; }
        public string[] AddressDeletions { get; set; }
        public MultisigAccountModificationTransaction1(byte minApproval, byte minRemoval, string[] addressAdditions, string[] addressDeletions, bool embedded) : base(embedded)
        {
        
            MinApprovalDelta = minApproval;
            MinRemovalDelta = minRemoval;
            AddressAdditionsCount = (byte)AddressAdditions.Length;
            AddressDeletionsCount = (byte)AddressDeletions.Length;
            Multisig_account_modification_transaction_body_reserved_1 = 0;
            AddressAdditions = addressAdditions;
            AddressDeletions = addressDeletions;
            
        }
    }
}
