namespace io.nem2.sdk.src.Infrastructure.Buffers.NativeBuffer
{ 
    public class TransactionDataSerializer
    {
        public byte[] Bytes;
        public int _offset = 0;

        public TransactionDataSerializer(int transactionSize)
        {
            Bytes = new byte[transactionSize];
        }

        public void WriteUlong(ulong data)
        {
            for (int i = 0; i < 8; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (8 - 1 - i) * 8);
            }

            _offset += 8;
        }

        public void WriteUint(uint data)
        {
            for (int i = 0; i < 4; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (4 - 1 - i) * 8);
            }

            _offset += 4;
        }

        public void WriteUshort(ushort data)
        {
            for (int i = 0; i < 2; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (2 - 1 - i) * 8);
            }

            _offset += 2;
        }

        public void WriteByte(byte data)
        {
            Bytes[_offset] = data;

            _offset += 1;
        }

        public void WriteHexString(string data)
        {
            for (var i = 0; i < data.Length / 2; i++)
            {
                Bytes[_offset + i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            }

            _offset += data.Length / 2;
        }
    }
}
