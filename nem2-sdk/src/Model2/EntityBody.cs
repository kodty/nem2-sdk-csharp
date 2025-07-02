using io.nem2.sdk.src.Model.Network;


namespace io.nem2.sdk.src.Model2
{
    public class EntityBody
    {
        public string Signer { get; set; }

        public uint Entity_body_reserved_1 { get; set; }

        public byte Version { get; set; }

        public NetworkType.Types Network { get; set; }

        public EntityBody()
        {
            Entity_body_reserved_1 = 0;
        }
    }

    




}
