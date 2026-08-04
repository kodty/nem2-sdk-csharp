using Coppery;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;
using System.Text;

namespace io.nem2.sdk.Model.Transactions
{
    public class RegisterNamespace : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Duration);
            serializer.SerializeProperty(ParentId);
            serializer.SerializeProperty(Id);
            serializer.SerializeProperty(RegistrationType);
            serializer.SerializeProperty(NameSize);
            serializer.SerializeProperty(Name);
        }

        public RegisterNamespace(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name) 
        {
            _Duration = duration;
            _ParentId = parentId;
            Id = DataConverter.ConvertFrom(id).Reverse().ToArray();
            RegistrationType = type.GetValue();
            Name = Encoding.UTF8.GetBytes(name);
            NameSize = (byte)Name.Length;
        }

        internal ulong _Duration { get; set; }

        public byte[] Duration
        {
            get
            {
                if (RegistrationType == 0x00)
                    return DataConverter.ConvertFrom(_Duration);

                else if (RegistrationType == 0x01)
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
                if (RegistrationType == 0x00)
                    return new byte[] { };
                else if (RegistrationType == 0x01)
                    return DataConverter.ConvertFrom(_ParentId).Reverse().ToArray();
                else
                    throw new Exception("invalid registration type");
            }
        }

        public byte[] Id { get; set; }

        public byte RegistrationType { get; internal set; }

        public byte NameSize { get; internal set; }

        public byte[] Name { get; internal set; }

        internal override int AddSize()
        {
            return 18 + Name.Length;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.NAMESPACE_REGISTRATION;
        }
    }
}
