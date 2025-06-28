using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.Core.Utils;

namespace io.nem2.sdk.Model.Namespace
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

            HexId = BitConverter.GetBytes(Id).ToHexUpper();
        }

        public NamespaceId(ulong id)
        {
            Id = id;
            HexId = BitConverter.GetBytes(id).ToHexUpper();
        }
        
        public static NamespaceId Create(string id)
        {
            return new NamespaceId(id);
        }
    }
}
