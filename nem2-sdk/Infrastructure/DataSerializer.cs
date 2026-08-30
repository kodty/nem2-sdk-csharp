namespace io.nem2.sdk.Infrastructure
{
    public class DataSerializer
    {
        internal byte[] _BufferOne { get; set; }

        private int offset = 0;

        public DataSerializer(byte[] BufferOne)
        {
            _BufferOne = BufferOne;
        }

        public byte[] GetBytes()
        {
            return _BufferOne;
        }

        public void SerializeProperty(byte value)
        {
            _BufferOne[offset++] = value;    
        }

        public void SerializeProperty(ushort value)
        {
            for (var x = 0; x < 2; x++)
            {
                _BufferOne[offset + x] = (byte)(value >> x * 8);
            }

            offset += 2;
        }

        public void SerializeProperty(uint value)
        {
            for (var x = 0; x < 4; x++)
                _BufferOne[offset + x] = (byte)(value >> x * 8);
            
            offset += 4;
        }

        public void SerializeProperty(ulong value)
        {
            for (var x = 0; x < 8; x++)
            {
                _BufferOne[offset + x] = (byte)(value >> x * 8);
            }

            offset += 8;
        }

        public void SerializeProperty(byte[] value)
        {
            for (var x = 0; x < value.Length; x++)
            {
                _BufferOne[offset + x] = value[x];
            }

            offset += value.Length;
        }
    }
}
