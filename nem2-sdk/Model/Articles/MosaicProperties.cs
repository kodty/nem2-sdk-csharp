namespace io.nem2.sdk.Model.Articles
{
    public class MosaicProperties
    {
        public bool IsSupplyMutable { get; }

        public bool IsTransferable { get; }

        public bool IsRestrictable { get; }

        public bool IsRevokable { get; set; }

        public byte Divisibility { get; }

        public ulong Duration { get; }

        public MosaicProperties(bool supplyMutable, bool transferable, bool restrictable, bool revokable, byte divisibility, ulong duration)
        {
            if (divisibility < 0 || divisibility > 6) throw new ArgumentException("Divisibility must be between 0 and 6");
            IsSupplyMutable = supplyMutable;
            IsTransferable = transferable;
            IsRestrictable = restrictable;
            IsRevokable = revokable;
            Divisibility = divisibility;
            Duration = duration;
        }

        internal byte GetFlags()
        {
            byte flags = 0;

            if (IsSupplyMutable)
            {
                flags += 1;
            }

            if (IsTransferable)
            {
                flags += 2;
            }

            if (IsRestrictable)
            {
                flags += 4;
            }

            if (IsRevokable)
            {
                flags += 8;
            }

            return flags;
        }
    }
}
