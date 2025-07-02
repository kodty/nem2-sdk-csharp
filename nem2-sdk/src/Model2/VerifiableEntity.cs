namespace io.nem2.sdk.src.Model2
{
    public class VerifiableEntity
    {
        public uint Size { get; set; }

        public uint VerifiableEntityHeaderReserved { get; set; }

        public byte[] Signature { get; set; }

        public VerifiableEntity()
        {
            VerifiableEntityHeaderReserved = 0;
        }
    }
}
