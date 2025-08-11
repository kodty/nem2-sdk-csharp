using Coppery;
using io.nem2.sdk.src.Model.Articles;
using System.Text;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class RegisterNamespace : Transaction
    {
        public RegisterNamespace(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name, bool embedded) : base(embedded)
        {
            _Duration = duration;

            if(type.GetValue() == 0x01)
                _ParentId = parentId;

            Id = id;
            RegistrationType = type.GetValue();
            Name = Encoding.UTF8.GetBytes(name);
            NameSize = (byte)Name.Length;
            Size += 18 +  (uint)Name.Length;
        }

        internal ulong _Duration { get; set; }

        public byte[] Duration
        {
            get
            {
                if (RegistrationType == 0)
                    return DataConverter.ConvertFromUInt64(_Duration);
                
                else if (RegistrationType == 0x1) 
                    return new byte[] { };
                else
                    throw new Exception("invalid registration type");
            }
        }

        internal ulong _ParentId { get; set; }

        public byte[] ParentId
        {
            get
            {
                if (RegistrationType == 0)
                    return new byte[] { };
                else if (RegistrationType == 0x1) 
                    return DataConverter.ConvertFromUInt64(_ParentId);
                else 
                    throw new Exception("invalid registration type");
            }
        }

        public ulong Id { get; internal set; }

        public byte RegistrationType { get; internal set; } 

        public byte NameSize { get; internal set; }

        public byte[] Name { get; internal set; }
    }
}
