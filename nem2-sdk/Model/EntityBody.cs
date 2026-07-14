namespace io.nem2.sdk.Model
{
    public class EntityBody
    {
        [Order(5)]
        public byte[] Signer { get; set; }

        [Order(6)]
        public uint Entity_body_reserved_1 { get; set; }

        [Order(7)]
        public byte Version { get; set; }

        [Order(8)]
        public byte Network { get; set; }

        public EntityBody()
        {
            Entity_body_reserved_1 = 0;
        }
    }
}
