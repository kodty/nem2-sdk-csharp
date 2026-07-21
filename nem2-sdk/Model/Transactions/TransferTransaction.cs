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
        public byte Reserved_1 { get; }

        [Order(16)]
        public uint Reserved_2 { get; }

        [Order(17)]
        public byte[] MosaicId { get; set; }

        [Order(18)]
        public ulong MosaicAmount { get; set; }

        [Order(19)]
        public byte[] Message { get; set; }

        public TransferTransaction_V1(Address address, IMessage messege, Mosaic mosaic, bool isEmbedded) : base(TransactionTypes.Types.TRANSFER, isEmbedded)
        {
            // extended transaction size excluding variable length fields
            Size += 24; 

            Address = AddressEncoder.DecodeAddress(address.Plain);         
            MosaicId = DataConverter.ConvertFrom(mosaic.MosaicId.Id).Reverse().ToArray();
            MosaicAmount = mosaic.Amount;
            MosaicsCount = 1;
            Message = messege.GetPayload();    
            MessegeSize = (ushort)Message.Length;

            Size += (uint)Address.Length;
            Size += MessegeSize; 
        }

        public override TransferTransaction_V1 SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }
    }
}
