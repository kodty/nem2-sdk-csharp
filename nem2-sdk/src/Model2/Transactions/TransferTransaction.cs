using System.Text;

namespace io.nem2.sdk.src.Model2.Transactions
{
    public class TransferTransaction_V1 : Transaction1
    {
        public TransferTransaction_V1(string address, string messege, Tuple<string, ulong> mosaic)
        {
            Address = address;
            MosaicsCount = 1;
            Mosaics = mosaic;
            Message = Encoding.UTF8.GetBytes(messege);
            MessegeSize = (ushort)Message.Length;
            Reserved_1 = 0;
            Reserved_2 = 0;
        }

        public string Address { get; }

        public ushort MessegeSize { get; set; }

        public byte MosaicsCount { get; set; }

        public uint Reserved_1 { get; set; }

        public byte Reserved_2 { get; set; }

        public Tuple<string, ulong> Mosaics { get; set; }

        public byte[] Message { get; set; }
    }
}
