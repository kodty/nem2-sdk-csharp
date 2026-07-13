using Coppery;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;
using System.Text;

namespace io.nem2.sdk.Model.Transactions
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

            
            Type = TransactionTypes.Types.NAMESPACE_REGISTRATION.GetValue();
        }

        public RegisterNamespace(ulong duration, string parentId, string id, NamespaceTypes.Types type, string name, bool embedded) : base(embedded)
        {
            _Duration = duration;

            if (type.GetValue() == 0x01)
                _ParentId = DataConverter.ConvertTo<ulong>(parentId.FromHex());

            Id = DataConverter.ConvertTo<ulong>(id.FromHex()); ;
            RegistrationType = type.GetValue();
            Name = Encoding.UTF8.GetBytes(name);
            NameSize = (byte)Name.Length;
            Size += 18 + (uint)Name.Length;
        }

        internal ulong _Duration { get; set; }

        public byte[] Duration
        {
            get
            {
                if (RegistrationType == 0)
                    return DataConverter.ConvertFrom(_Duration);

                else if (RegistrationType == 0x1)
                    return new byte[] { };
                else
                    throw new Exception("invalid registration type");
            }

            set => DataConverter.ConvertFrom(_Duration);
        }

        internal ulong _ParentId { get; set; }

        public byte[] ParentId
        {
            get
            {
                if (RegistrationType == 0)
                    return new byte[] { };
                else if (RegistrationType == 0x1) 
                    return DataConverter.ConvertFrom(_ParentId);
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
