using Coppery;
using io.nem2.sdk.src.Core.Utils;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Articles;
using io.nem2.sdk.src.Model.Transactions.Messages;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class TransferTransaction_V1 : Transaction
    {
        public TransferTransaction_V1(Address address, IMessage messege, Mosaic mosaic, bool embedded) : base(embedded)
        {
            Address = AddressEncoder.DecodeAddress(address.Plain);
            Size += (uint)Address.Length;

            Size += 16;
            MosaicId = DataConverter.ConvertFrom(mosaic.MosaicId.Id);
            MosaicAmount = mosaic.Amount;
            MosaicsCount = 1;
            Size += 1;

            Message = messege.GetPayload();    
            MessegeSize = messege.GetLength();
            Size += MessegeSize;
            Size += 2;

            Reserved_1 = 0;
            Reserved_2 = 0;
            Size += 5;
        }

        public byte[] Address { get; set; }
        public ushort MessegeSize { get; set; }
        public byte MosaicsCount { get; set; }
        public byte Reserved_1 { get; set; }
        public uint Reserved_2 { get; set; }
        public byte[] MosaicId { get; set; }
        public ulong MosaicAmount { get; set; }
        public byte[] Message { get; set; }
    }
}
