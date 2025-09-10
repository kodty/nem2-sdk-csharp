using Coppery;

namespace io.nem2.sdk.src.Model.Articles
{
    public class MosaicId
    {
        public ulong Id { get; }


        public string MosaicName { get; }


        public string FullName { get; }


        public string HexId { get; }

        
        public MosaicId(ulong id)
        {
            Id = id;

            HexId = DataConverter.ConvertFrom(Id).ToHex();
        }

        public MosaicId(string hexId)
        {
            Id = hexId.FromHex().ConvertTo<ulong>();

            HexId = hexId;
        }

        public MosaicId(string[] identifierParts)
        {
            if (identifierParts.Count() > 3)
                throw new Exception("too many parts");

            var namespaceName = identifierParts[0];
            MosaicName = identifierParts[1];
            FullName = string.Join(':', identifierParts);

            Id = 0;     
            HexId = "";
        }

        
        public static MosaicId CreateFromHexMosaicIdentifier(string identifier)
        {
            return new MosaicId(identifier);
        } 
    }
}
