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
            serializer.SerializeProperty(Address, typeof(byte[]), 10);
            serializer.SerializeProperty(MessegeSize, typeof(ushort), 11);
            serializer.SerializeProperty(MosaicsCount, typeof(byte), 12);
            serializer.SerializeProperty((byte)0x0, typeof(byte), 13);
            serializer.SerializeProperty(new byte[4], typeof(byte[]), 14);
            serializer.SerializeProperty(MosaicId, typeof(byte[]), 15);
            serializer.SerializeProperty(MosaicAmount, typeof(ulong), 16);
            serializer.SerializeProperty(Message, typeof(byte[]), 17);
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
            Size += 24;

            Version = 0x01;
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

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}
