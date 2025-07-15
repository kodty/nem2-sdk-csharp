using CopperCurve;

namespace io.nem2.sdk.Model.Mosaics
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

            HexId = DataConverter.ConvertFromUInt64(id).ToHex();
        }

        public MosaicId(string hexId)
        {
            Id = DataConverter.ConvertToUInt64(hexId.FromHex());

            HexId = hexId;
        }

        public MosaicId(string[] identifierParts)
        {
            if (identifierParts.Count() > 3)
                throw new Exception("too many parts");

            var namespaceName = identifierParts[0];
            MosaicName = identifierParts[1];
            FullName = String.Join(':', identifierParts);

            Id = 0;     
            HexId = "";
        }

        
        public static MosaicId CreateFromHexMosaicIdentifier(string identifier)
        {
            return new MosaicId(identifier);
        } 
        
        public override bool Equals(object obj)
        {
            return Id == ((MosaicId) obj)?.Id;
        }
    }
}
