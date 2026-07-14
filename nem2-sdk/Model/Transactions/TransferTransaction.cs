using Coppery;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions
{
    public class TransferTransaction_V1 : VerifiableTransaction
    {
        [Order(12)]
        public byte[] Address { get; set; }

        [Order(13)]
        public ushort MessegeSize { get; set; }

        [Order(14)]
        public byte MosaicsCount { get; set; }

        [Order(15)]
        public byte Reserved_1 { get; set; }

        [Order(16)]
        public uint Reserved_2 { get; set; }

        [Order(17)]
        public byte[] MosaicId { get; set; }

        [Order(18)]
        public ulong MosaicAmount { get; set; }

        [Order(19)]
        public byte[] Message { get; set; }

        public TransferTransaction_V1(Address address, IMessage messege, Mosaic mosaic, bool embedded) : base(TransactionTypes.Types.TRANSFER, embedded)
        {
            Address = AddressEncoder.DecodeAddress(address.Plain);
            VerifiableEntity.Size += (uint)Address.Length;
            
            MosaicId = DataConverter.ConvertFrom(mosaic.MosaicId.Id).Reverse().ToArray();
            VerifiableEntity.Size += 8;

            MosaicAmount = mosaic.Amount;
            VerifiableEntity.Size += 8;

            MosaicsCount = 1;
            VerifiableEntity.Size += 1;

            Message = messege.GetPayload();    
            MessegeSize = (ushort)Message.Length;
            VerifiableEntity.Size += MessegeSize;
            VerifiableEntity.Size += 2;

            Reserved_1 = 0;
            Reserved_2 = 0;
            VerifiableEntity.Size += 5;
        }
    }
}
