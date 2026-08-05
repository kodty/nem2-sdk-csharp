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
            serializer.SerializeProperty(_Duration);
            serializer.SerializeProperty(_ParentId);
            serializer.SerializeProperty(Id);
            serializer.SerializeProperty(RegistrationType);
            serializer.SerializeProperty(NameSize);
            serializer.SerializeProperty(Name);
        }

        public RegisterNamespace(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name) 
        {
            Duration = duration;
            ParentId = parentId;
            Id = id;
            RegistrationType = type.GetValue();
            Name = Encoding.UTF8.GetBytes(name);
            NameSize = (byte)Name.Length;
        }

        public ulong Duration { get; set; }

        internal byte[] _Duration
        {
            get
            {
                if (RegistrationType == 0x00)
                    return DataConverter.ConvertFrom(Duration);

                else if (RegistrationType == 0x01)
                    return new byte[] { };
                else
                    throw new Exception("invalid registration type");
            }
        }

        public ulong ParentId { get; set; }

        internal byte[] _ParentId
        {
            get
            {
                if (RegistrationType == 0x00)
                    return new byte[] { };
                else if (RegistrationType == 0x01)
                    return DataConverter.ConvertFrom(ParentId);
                else
                    throw new Exception("invalid registration type");
            }
        }

        public ulong Id { get; set; }

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
