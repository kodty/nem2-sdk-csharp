using System.ComponentModel;
using CopperCurve;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model.Transactions
{
    public class MosaicSupplyChangeTransaction : Transaction
    {
        public ulong Delta { get; }

        public MosaicId MosaicId { get; }

        public MosaicSupplyType.Type SupplyType { get; }

        public static MosaicSupplyChangeTransaction Create(NetworkType.Types networkType, Deadline deadline, MosaicId mosaicId,  MosaicSupplyType.Type direction, ulong delta)
        {
            return new MosaicSupplyChangeTransaction(networkType, 3, deadline, 0, mosaicId, direction, delta);
        }

        public MosaicSupplyChangeTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, MosaicId mosaicId, MosaicSupplyType.Type direction, ulong delta) 
            : this(networkType, version, deadline, fee, mosaicId, direction, delta, null, null) {}

        public MosaicSupplyChangeTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, MosaicId mosaicId, MosaicSupplyType.Type direction, ulong delta, string signature, PublicAccount signer)
        {
            if (direction.GetValue() >= 2) throw new ArgumentOutOfRangeException(nameof(direction));
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType,
                    typeof(NetworkType.Types));

            MosaicId = mosaicId ?? throw new ArgumentNullException(nameof(mosaicId));
            Delta = delta;
            SupplyType = direction;
            Version = version;
            Deadline = deadline;
            Fee = fee;
            NetworkType = networkType;
            TransactionType = TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE;
            Signer = signer;
            Signature = signature;
            //TransactionInfo = transactionInfo;
        }

        internal override byte[] GenerateBytes()
        {

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
            serializer.WriteUlong(MosaicId.Id);
            serializer.WriteUlong(Delta);

            return serializer.Bytes;
        }
    }
}
