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
            if (RegistrationType == 0x00)
                serializer.SerializeProperty(Duration);

            if (RegistrationType == 0x01)
                serializer.SerializeProperty(ParentId);
           
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

        public ulong ParentId { get; set; }

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
