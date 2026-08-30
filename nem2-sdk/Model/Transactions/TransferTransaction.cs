using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;

namespace io.nem2.sdk.Model.Transactions
{
    public class TransferTransaction_V1 : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Recipient);
            serializer.SerializeProperty(MessegeSize);
            serializer.SerializeProperty(MosaicsCount);
            serializer.SerializeProperty(0x0);
            serializer.SerializeProperty(new byte[4]);
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(MosaicAmount);

            if(Message != null)
                serializer.SerializeProperty(Message);
        }

        public byte[] Recipient { get; set; }

        public ushort MessegeSize { get; set; }

        public byte MosaicsCount { get; set; }

        public ulong MosaicId { get; set; }

        public ulong MosaicAmount { get; set; }

        public byte[] Message { get; set; }

        public TransferTransaction_V1(Address address, IMessage messege, Mosaic mosaic)
        {
            Recipient = Address.DecodeAddress(address.Plain);         
            MosaicId = mosaic.MosaicId.Id;
            MosaicAmount = mosaic.Amount;
            MosaicsCount = 1;
            Message = messege.GetPayload();

            if (Message != null)
            {
                Message = messege.GetPayload();
                MessegeSize = (ushort)Message.Length;
            }
            else MessegeSize = 0;

        }

        internal override int AddSize() 
        {
            return 48 + MessegeSize;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.TRANSFER;
        }
    }
}
