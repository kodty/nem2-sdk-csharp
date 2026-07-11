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

            Size += (uint)Address.Length;


            if (mosaic.Item1.IsHex(16))
            {
                Size += 16;
                Mosaics = new Tuple<byte[], ulong>(mosaic.Item1.FromHex(), mosaic.Item2);
                MosaicsCount = 1;
                Size += 1;
            }
            else if (mosaic != null && !mosaic.Item1.IsHex(16))
                throw new Exception("mosaic error");
            else if(mosaic == null)
            {
                MosaicsCount = 0;
                Size += 0;
            }

            Message = Encoding.UTF8.GetBytes(messege);

            if (Message.Length > 0)
            {           
                Size += (ushort)Message.Length;
                MessegeSize = (ushort)Message.Length;
                Size += 2;
            }
            else
            {
                MessegeSize = 0;
                Size += 2;
            }

            Reserved_1 = 0;
            Reserved_2 = 0;
            Size += 5;
        }

        public byte[] Address { get; set; }
        public ushort MessegeSize { get; set; }
        public byte MosaicsCount { get; set; }
        public byte Reserved_1 { get; set; }
        public uint Reserved_2 { get; set; }
        public Tuple<byte[], ulong> Mosaics { get; set; }
        public byte[] Message { get; set; }
    }
}
