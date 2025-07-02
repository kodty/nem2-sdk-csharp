namespace io.nem2.sdk.Model.Mosaics
{
    public class Mosaic1
    {
        public MosaicId MosaicId { get; }

        public ulong Amount { get; }

        public Mosaic1(MosaicId id, ulong amount)
        {
            MosaicId = id ?? throw new ArgumentNullException(nameof(id));
            Amount = amount;
        }

        public static Mosaic1 CreateFromIdentifierParts(string[] identifierParts, ulong amount)
        {
            return new Mosaic1(new MosaicId(identifierParts), amount);
        }

        public static Mosaic1 CreateFromHexIdentifier(string hexId, ulong amount)
        {

            return new Mosaic1(new MosaicId(hexId), amount);
        }
    }
}
