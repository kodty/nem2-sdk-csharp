using Coppery;
using System.Text;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class TransferTransaction_V1 : Transaction
    {

        public TransferTransaction_V1(string address, string messege, Tuple<string, ulong> mosaic, bool embedded) : base(embedded)
        {
            if (address.IsBase32(address.Length))
                Address = AddressEncoder.DecodeAddress(address);

            if(mosaic.Item1.IsHex(16))
                Mosaics = new Tuple<byte[], ulong>(mosaic.Item1.FromHex(), mosaic.Item2);
            MosaicsCount = 1;

            Message = Encoding.UTF8.GetBytes(messege);
            MessegeSize = (ushort)Message.Length;

            Reserved_1 = 0;
            Reserved_2 = 0;
            Size += 8;

            Size += (uint)Address.Length;
            if (mosaic != null)
                Size += 16;
            if (Message.Length > 0)
                Size += MessegeSize;
        }

        public byte[] Address { get; set; }

        public ushort MessegeSize { get; set; }

        public byte MosaicsCount { get; set; }

        public uint Reserved_1 { get; set; }

        public byte Reserved_2 { get; set; }

        public Tuple<byte[], ulong> Mosaics { get; set; }

        public byte[] Message { get; set; }
    }
}
