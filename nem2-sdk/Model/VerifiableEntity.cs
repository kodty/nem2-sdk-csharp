namespace io.nem2.sdk.Model
{
    public class VerifiableEntity
    {
        [Order(1)]
        public uint Size { get; set; }

        [Order(2)]
        public uint VerifiableEntityHeaderReserved { get; set; }

        [Order(3)]
        public byte[] Signature { get; set; }

        public VerifiableEntity()
        {
            Size = 0;
            VerifiableEntityHeaderReserved = 0;
            Signature = new byte[64];
        }
    }
}
