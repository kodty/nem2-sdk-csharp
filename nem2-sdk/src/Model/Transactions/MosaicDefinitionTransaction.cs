using CopperCurve;
using System.Text;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model.Transactions
{
    public class MosaicDefinitionTransaction : Transaction
    {
        public  string MosaicName { get; }

        public NamespaceId NamespaceId { get; }

        public MosaicId MosaicId { get; }

        public MosaicProperties Properties { get; }

        public MosaicDefinitionTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, string mosaicName, NamespaceId namespaceId, MosaicId mosaicId, MosaicProperties properties)
            : this(networkType, version, deadline, fee, mosaicName, namespaceId, mosaicId, properties, null, null){}

        public MosaicDefinitionTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, string mosaicName, NamespaceId namespaceId, MosaicId mosaicId, MosaicProperties properties,  string signature, PublicAccount signer)
        {
            Deadline = deadline;
            NetworkType = networkType;
            Version = version;
            Properties = properties;
            MosaicId = mosaicId;
            NamespaceId = namespaceId;
            MosaicName = mosaicName;
            Fee = fee;
            TransactionType = TransactionTypes.Types.MOSAIC_DEFINITION;
            Signature = signature;
            Signer = signer;
            //TransactionInfo = transactionInfo;
        }

        public static MosaicDefinitionTransaction Create(NetworkType.Types networkType, Deadline deadline, string namespaceId,  string mosaicName, MosaicProperties properties)
        {
            return new MosaicDefinitionTransaction(
                networkType,
                3,
                deadline,
                0,             
                mosaicName,
                NamespaceId.Create(namespaceId),
                MosaicId.CreateFromHexMosaicIdentifier(namespaceId + ":" + mosaicName),              
                properties
            );
        }

        internal override byte[] GenerateBytes()
        {
            byte flags = 0;

            if (Properties.IsSupplyMutable)
            {
                flags += 1;
            }

            if (Properties.IsTransferable)
            {
                flags += 2;
            }

            if (Properties.IsLevyMutable)
            {
                flags += 4;
            }

            ushort size = 137;

            var serializer = new DataSerializer();

            serializer.WriteUlong(size);

            serializer.Reserve(64);
            serializer.WriteBytes(GetSigner());
            serializer.Reserve(4);
            serializer.WriteByte((byte)Version);
            serializer.WriteByte(NetworkType.GetNetworkByte());
            serializer.WriteUShort(TransactionType.GetValue());
            serializer.WriteUlong(Fee);
            serializer.WriteUlong(Deadline.Ticks);
            serializer.WriteUlong(NamespaceId.Id);
            serializer.WriteUlong(MosaicId.Id);
            serializer.WriteBytes(Encoding.UTF8.GetBytes(MosaicId.MosaicName));
            serializer.WriteUlong(Properties.Duration);

            return serializer.Bytes;
        }
    }
}
