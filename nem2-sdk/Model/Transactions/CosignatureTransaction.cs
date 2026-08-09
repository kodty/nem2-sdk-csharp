namespace io.nem2.sdk.Model.Transactions
{
    public class Cosignature1 : SignedTransaction
    {
        public Cosignature1() 
        {
            Version = 0;
        }

        public ulong Version { get; set; } 
    }  

    public class DetachedCosignature : Cosignature1
    {
        public DetachedCosignature() { }
        public byte[] ParentHash { get; set; }
    }
}
