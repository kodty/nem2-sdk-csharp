using Coppery;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions;
using System.Text;

namespace io.nem2.sdk.Model.Transactions
{
    public class RegisterNamespace : VerifiableTransaction
    {
        public RegisterNamespace(ulong duration, ulong parentId, ulong id, NamespaceTypes.Types type, string name, bool embedded) : base(TransactionTypes.Types.NAMESPACE_REGISTRATION, embedded)
        {         
            _Duration = duration;
            Id = DataConverter.ConvertFrom(id).Reverse().ToArray();
            RegistrationType = type.GetValue();
            Name = Encoding.UTF8.GetBytes(name);
            NameSize = (byte)Name.Length;
            VerifiableEntity.Size += 18 + (uint)Name.Length;
        }

        internal ulong _Duration { get; set; }

        [Order(12)]
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

        [Order(13)]
        public byte[] ParentId
        {
            get
            {
                if (RegistrationType == 0x00)
                    return new byte[] { };
                else if (RegistrationType == 0x01)
                    return DataConverter.ConvertFrom(_ParentId);
                else
                    throw new Exception("invalid registration type");
            }
        }

        [Order(14)]
        public byte[] Id { get; set; }

        [Order(15)]
        public byte RegistrationType { get; internal set; }

        [Order(16)]
        public byte NameSize { get; internal set; }

        [Order(17)]
        public byte[] Name { get; internal set; }

        public override RegisterNamespace SetSigner(string signer)
        {
            EntityBody.Signer = signer.FromHex();

            return this;
        }
    }
}
