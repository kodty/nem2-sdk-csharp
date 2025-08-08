using CopperCurve;
using io.nem2.sdk.src.Core.Utils;

namespace io.nem2.sdk.src.Model.Articles
{
    public class NamespaceId
    {
        public ulong Id { get; }

        public string Name { get; }

        public string HexId { get; }

        public NamespaceId(string id)
        {     
            if (id == null) throw new ArgumentNullException(nameof(id) + " cannot be null");

            Id = IdGenerator.GenerateId(0, id);
            Name = id;
            HexId = DataConverter.ConvertFromUInt64(Id).ToHex();
        }

        public NamespaceId(ulong id)
        {
            Id = id;
            HexId = DataConverter.ConvertFromUInt64(Id).ToHex();
        }
        
        public static NamespaceId Create(string id)
        {
            return new NamespaceId(id);
        }
    }
}
