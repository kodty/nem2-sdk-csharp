namespace io.nem2.sdk.src.Model.Transactions
{
    public class CosignatureTransaction
    {
        public CosignatureTransaction() { }
        public byte Version { get; set; }
        public string Signer { get; set; }
        public string Signature { get; set; }
        public string ParentHash { get; set; }
    }
}
