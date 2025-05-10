using System.ComponentModel;
using System.Text;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model.Transactions
{
    public class RegisterNamespaceTransaction : Transaction
    {
        public NamespaceTypes.Types NamespaceType { get; set; }

        public ulong Duration { get; }

        public NamespaceId NamespaceId { get; }

        public NamespaceId ParentId { get; }

        public RegisterNamespaceTransaction(NetworkType.Types type, int version, Deadline deadline, ulong fee, byte namespaceType, ulong duration, NamespaceId parentId, NamespaceId namespaceName)
           : this(type, version, deadline, fee, namespaceType, duration, parentId, namespaceName, null, null, null){}

        public RegisterNamespaceTransaction(NetworkType.Types type, int version, Deadline deadline, ulong fee, byte namespaceType, ulong duration, NamespaceId parentId, NamespaceId namespaceName, PublicAccount signer, string signature, TransactionInfo transactionInfo)
        {
            if (parentId == null && namespaceName == null) throw new ArgumentNullException(nameof(parentId) + " and " + nameof(namespaceName) + " cannot both be null");
            if (!Enum.IsDefined(typeof(NetworkType.Types), type)) throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(NetworkType.Types));
            if (namespaceType != 0 && namespaceType != 1) throw new ArgumentOutOfRangeException(nameof(namespaceType));
            
            NetworkType = type;
            Version = version;
            Deadline = deadline;
            Fee = fee;
            NamespaceType = NamespaceTypes.GetRawValue(namespaceType);
            TransactionType = TransactionTypes.Types.NAMESPACE_REGISTRATION;
            Duration = duration;
            ParentId = parentId;
            NamespaceId = namespaceName;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
        }

        public static RegisterNamespaceTransaction CreateRootNamespace(NetworkType.Types type, Deadline deadline, string namespaceName, ulong duration)
        {
            return new RegisterNamespaceTransaction(type, 3, deadline, 0, 0x00, duration, null, NamespaceId.Create(namespaceName));
        }

        public static RegisterNamespaceTransaction CreateSubNamespace(NetworkType.Types type, Deadline deadline, string parentId, string namespaceName)
        {
            return new RegisterNamespaceTransaction(type, 3, deadline, 0, 0x01, 0, NamespaceId.Create(parentId), NamespaceId.Create(namespaceName));
        }

        internal override byte[] GenerateBytes()
        {
            var namespaceNameLength = (uint)Encoding.UTF8.GetBytes(NamespaceId.Name).Length;

            ushort size = (ushort)(138 + namespaceNameLength);

            var serializer = new DataSerializer(size);

            serializer.WriteUlong(size);

            serializer.Reserve(64);
            serializer.WriteBytes(GetSigner());
            serializer.Reserve(4);
            serializer.WriteByte((byte)Version);
            serializer.WriteByte(NetworkType.GetNetworkByte());
            serializer.WriteUShort(TransactionType.GetValue());
            serializer.WriteUlong(Fee);
            serializer.WriteUlong(Deadline.Ticks);          

            if (ParentId == null)
            {
                serializer.WriteUlong(Duration);
                serializer.WriteUlong(IdGenerator.GenerateId(0, NamespaceId.Name));
            }
            else
            {
                serializer.WriteUlong(ParentId.Id);
                serializer.WriteUlong(IdGenerator.GenerateId(IdGenerator.GenerateId(0, ParentId.Name), NamespaceId.Name));

            }

            serializer.WriteByte(NamespaceType.GetValue());
            serializer.WriteByte((byte)namespaceNameLength);
            serializer.WriteBytes(Encoding.UTF8.GetBytes(NamespaceId.Name));

            return serializer.Bytes;
        }
    }
}
