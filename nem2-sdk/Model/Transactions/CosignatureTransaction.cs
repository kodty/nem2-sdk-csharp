namespace io.nem2.sdk.Model.Transactions
{
    public class Cosignature : SignedTransaction
    {
        public Cosignature() 
        {
            Version = 0;
        }

        public ulong Version { get; set; } 
    }  

    public class DetachedCosignature : Cosignature
    {
        public DetachedCosignature() { }
        public byte[] ParentHash { get; set; }
    }
}
