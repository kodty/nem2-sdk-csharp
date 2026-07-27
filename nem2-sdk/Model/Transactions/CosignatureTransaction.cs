namespace io.nem2.sdk.Model.Transactions
{
    public class Cosignature
    {
        public Cosignature() { }
        public byte Version { get; set; }
        public byte[] Signer { get; set; }
        public byte[] Signature { get; set; }
        

    }  

    public class DetachedCosignature : Cosignature
    {
        public DetachedCosignature() { }
        public byte[] ParentHash { get; set; }
    }
}
