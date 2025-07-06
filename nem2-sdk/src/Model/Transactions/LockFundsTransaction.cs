using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model.Transactions
{
    public class LockFundsTransaction : Transaction
    {
        public Mosaic1 Mosaic { get; }

        public ulong Duration { get; }

        public SignedTransaction Transaction { get; }

        public static LockFundsTransaction Create(NetworkType.Types netowrkType, Deadline deadline, ulong fee, Mosaic1 mosaic, ulong duration, SignedTransaction transaction)
        {
            return new LockFundsTransaction(netowrkType, 3, deadline, fee, mosaic, duration, transaction);
        }

        public LockFundsTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Mosaic1 mosaic, ulong duration, SignedTransaction transaction )
            : this(networkType, version, deadline, fee, mosaic, duration, transaction, null, null) {}

        public LockFundsTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee,  Mosaic1 mosaic, ulong duration, SignedTransaction transaction, string signature, PublicAccount signer)
        {
            if (transaction.TransactionType != TransactionTypes.Types.AGGREGATE_BONDED) throw new ArgumentException("Cannot lock non-aggregate-bonded transaction");
            Deadline = deadline;
            Version = version;
            Duration = duration;
            Mosaic = mosaic;
            NetworkType = networkType;
            Transaction = transaction;
            TransactionType = TransactionTypes.Types.HASH_LOCK;
            Signer = signer;
            Signature = signature;
            //TransactionInfo = transactionInfo;
            Fee = fee;
        }

        internal override byte[] GenerateBytes()
        {
            ushort size = 176;

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
            serializer.WriteUlong(Mosaic.MosaicId.Id);
            serializer.WriteUlong(Mosaic.Amount);
            serializer.WriteBytes(Transaction.Hash.FromHex());

            return serializer.Bytes;
        }
    }
}
