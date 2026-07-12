using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Articles
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
            
            MosaicName = identifierParts[1];

            FullName = string.Join(':', identifierParts);

            Id = IdGenerator.GenerateId(IdGenerator.GenerateId(0, identifierParts[0]), identifierParts[1]); //
          
            HexId = DataConverter.ConvertFrom(Id).ToHex();
        }
       
        public static MosaicId CreateFromHexMosaicIdentifier(string identifier)
        {
            return new MosaicId(identifier);
        } 
    }
}
