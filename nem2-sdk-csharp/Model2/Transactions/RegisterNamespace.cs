using io.nem2.sdk.Model.Namespace;
using System.Text;

namespace io.nem2.sdk.Model2.Transactions
{
    public class RegisterNamespace : Transaction1
    {
        public RegisterNamespace(ulong duration, NamespaceId parentId, NamespaceId id, NamespaceTypes.Types type, string name)
        {
            Duration = duration;
            ParentId = parentId;
            Id = id;
            RegistrationType = type;
            Name = name;
            NameSize = (byte)Encoding.UTF8.GetBytes(Name).Length;
        }

        public ulong Duration { get; internal set; }

        public NamespaceId ParentId { get; internal set; }

        public NamespaceId Id { get; internal set; }

        public NamespaceTypes.Types RegistrationType { get; internal set; } 

        public byte NameSize { get; internal set; }

        public string Name { get; internal set; }
    }
}
