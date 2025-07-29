namespace io.nem2.sdk.src.Model.Articles
{
    public class Mosaic
    {
        public MosaicId MosaicId { get; }

        public ulong Amount { get; }

        public Mosaic(MosaicId id, ulong amount)
        {
            MosaicId = id ?? throw new ArgumentNullException(nameof(id));
            Amount = amount;
        }

        public static Mosaic CreateFromIdentifierParts(string[] identifierParts, ulong amount)
        {
            return new Mosaic(new MosaicId(identifierParts), amount);
        }

        public static Mosaic CreateFromHexIdentifier(string hexId, ulong amount)
        {

            return new Mosaic(new MosaicId(hexId), amount);
        }
    }
}
