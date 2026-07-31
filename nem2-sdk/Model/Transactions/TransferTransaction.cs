using Coppery;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions
{
    public class TransferTransaction_V1 : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Address);
            serializer.SerializeProperty(MessegeSize);
            serializer.SerializeProperty(MosaicsCount);
            serializer.SerializeProperty(0x0);
            serializer.SerializeProperty(new byte[4]);
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(MosaicAmount);
            serializer.SerializeProperty(Message);
        }

        public byte[] Address { get; set; }

        public ushort MessegeSize { get; set; }

        public byte MosaicsCount { get; set; }

        public byte[] MosaicId { get; set; }

        public ulong MosaicAmount { get; set; }

        public byte[] Message { get; set; }

        public TransferTransaction_V1(Address address, IMessage messege, Mosaic mosaic, bool isEmbedded) : base(TransactionTypes.Types.TRANSFER, isEmbedded)
        {
            // extended transaction size excluding variable length fields
            Size += 48;

            Version = 0x01;
            Address = AddressEncoder.DecodeAddress(address.Plain);         
            MosaicId = DataConverter.ConvertFrom(mosaic.MosaicId.Id).Reverse().ToArray();
            MosaicAmount = mosaic.Amount;
            MosaicsCount = 1;
            Message = messege.GetPayload();    
            MessegeSize = (ushort)Message.Length;

            Size += MessegeSize;
        }

        public override TransferTransaction_V1 SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}
