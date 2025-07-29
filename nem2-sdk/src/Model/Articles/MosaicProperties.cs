namespace io.nem2.sdk.src.Model.Articles
{
    public class MosaicProperties
    {
        public bool IsSupplyMutable { get; }

        public bool IsTransferable { get; }

        public bool IsLevyMutable { get; }

        public int Divisibility { get; }

        public ulong Duration { get; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="supplyMutable">The mosaic supply mutability.</param>
        /// <param name="transferable">The mosaic transferability.</param>
        /// <param name="levyMutable">The mosaic levy mutability.</param>
        /// <param name="divisibility">The mosaic divisibility.</param>
        /// <param name="duration">The number of blocks the mosaic will be active.</param>
        /// <exception cref="Exception">Duration must be set</exception>
        /// <exception cref="ArgumentException">Divisibility must be between 0 and 6</exception>
        public MosaicProperties(bool supplyMutable, bool transferable, bool levyMutable, int divisibility, ulong duration)
        {
            if (divisibility < 0 || divisibility > 6) throw new ArgumentException("Divisibility must be between 0 and 6");
            IsSupplyMutable = supplyMutable;
            IsTransferable = transferable;
            IsLevyMutable = levyMutable;
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

            if (IsLevyMutable)
            {
                flags += 4;
            }

            return flags;
        }
    }
}
