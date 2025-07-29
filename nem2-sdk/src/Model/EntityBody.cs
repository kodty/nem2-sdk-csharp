namespace io.nem2.sdk.src.Model
{
    public class EntityBody
    {
        public byte[] Signer { get; set; }

        public uint Entity_body_reserved_1 { get; set; }

        public byte Version { get; set; }

        public byte Network { get; set; }

        public EntityBody()
        {
            Entity_body_reserved_1 = 0;
        }
    }

    




}
