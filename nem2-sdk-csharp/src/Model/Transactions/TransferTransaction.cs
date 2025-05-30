using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model.Transactions
{
    public class TransferTransaction : Transaction
    {
        public Address Address { get; }

        public IMessage Message { get; private set; }

        public List<Mosaic1> Mosaics { get; }

        internal TransferTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Address recipient, List<Mosaic1> mosaics, IMessage message)
        {
            if (mosaics == null) throw new ArgumentNullException(nameof(mosaics));
            Address = recipient ?? throw new ArgumentNullException(nameof(recipient));
            TransactionType = TransactionTypes.Types.TRANSFER;
            Version = version;
            mosaics.Sort((c1, c2) => string.CompareOrdinal(c1.MosaicId.MosaicName, c2.MosaicId.MosaicName));
            Deadline = deadline;
            Message = message ?? EmptyMessage.Create();
            Mosaics = mosaics;
            NetworkType = networkType;
            Fee = fee;
        }

        internal TransferTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Address recipient, List<Mosaic1> mosaics, IMessage message, string signature, PublicAccount signer)
        {
            if (mosaics == null) throw new ArgumentNullException(nameof(mosaics));
            Address = recipient ?? throw new ArgumentNullException(nameof(recipient));
            mosaics.Sort((c1, c2) => string.CompareOrdinal(c1.MosaicId.MosaicName, c2.MosaicId.MosaicName));
            TransactionType = TransactionTypes.Types.TRANSFER;
            Version = version;
            Deadline = deadline;
            Message = message ?? EmptyMessage.Create();
            Mosaics = mosaics;
            NetworkType = networkType;
            Fee = fee;
            Signature = signature;
            Signer = signer;
            //TransactionInfo = transactionInfo;
        }

        public static TransferTransaction Create(NetworkType.Types netowrkType, Deadline deadline, Address address, List<Mosaic1> mosaics, IMessage message)
        {
            return new TransferTransaction(netowrkType, 1, deadline, 100, address, mosaics, message);
        }

        public static TransferTransaction Create(NetworkType.Types netowrkType, Deadline deadline, ulong fee, Address address, List<Mosaic1> mosaics, IMessage message)
        {
            return new TransferTransaction(netowrkType, 1, deadline, fee, address, mosaics, message);
        }

        internal override byte[] GenerateBytes()
        {
            if (Message == null) Message = EmptyMessage.Create();

            ushort size = (ushort)(160 + (16 * Mosaics.Count) + Message.GetLength());

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
            serializer.WriteBytes(AddressEncoder.DecodeAddress(Address.Plain));
            serializer.WriteUShort(Message.GetLength());
            serializer.WriteByte((byte)Mosaics.Count);
            serializer.Reserve(4);
            serializer.Reserve(1);

            for (var i= 0; i < Mosaics.Count; i++)
            {
                serializer.WriteBytes(Mosaics[i].MosaicId.HexId.FromHex()); 
                serializer.WriteUlong(Mosaics[i].Amount);
            }

            serializer.WriteBytes(Message.GetPayload());

            return serializer.Bytes;
        }
    }  
}
