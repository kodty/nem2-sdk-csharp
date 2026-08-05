namespace Coppery
{
    public class DataSerializer
    {
        internal int[] Exclude = new int[] { 0, 4, 8, 40, 72, 104 };
        internal byte[] _BufferOne { get; set; }
        internal byte[] _BufferTwo { get; set; }

        private int _offset1 = 0;
        private int dif = 0;


        public DataSerializer(uint size, int reduceBy)
        {
            _BufferOne = new byte[size];
            _BufferTwo = new byte[size - reduceBy];
        }

        public byte[][] GetBytes()
        {
            return [_BufferOne, _BufferTwo];
        }

        public void SerializeProperty(byte value)
        {
            _BufferOne[_offset1] = value;
            _BufferTwo[_offset1++ - dif] = value;
        }

        public void SerializeProperty(ushort value)
        {
            for (var x = 0; x < 2; x++)
            {
                byte v = (byte)(value >> x * 8);

                _BufferOne[_offset1 + x] = v;
                _BufferTwo[_offset1 - dif + x] = v;
            }

            _offset1 += 2;
        }

        public void SerializeProperty(uint value)
        {
            if (!Exclude.Contains(_offset1))
            {
                for (var x = 0; x < 4; x++)
                {
                    byte v = (byte)(value >> x * 8);

                    _BufferOne[_offset1 + x] = v;
                    _BufferTwo[_offset1 - dif + x] = v;
                }
            }
            else
            {
                for (var x = 0; x < 4; x++)
                {
                    _BufferOne[_offset1 + x] = (byte)(value >> x * 8);
                }

                dif += 4;
            }

            _offset1 += 4;
        }

        public void SerializeProperty(ulong value)
        {
            if (!Exclude.Contains(_offset1))
            {
                for (var x = 0; x < 8; x++)
                {
                    byte v = (byte)(value >> x * 8);

                    _BufferOne[_offset1 + x] = v; 
                    _BufferTwo[_offset1 - dif + x] = v;
                }
            }
            else
            {
                for (var x = 0; x < 8; x++)
                {
                    _BufferOne[_offset1 + x] = (byte)(value >> x * 8);
                }

                dif += 8;
            }

            _offset1 += 8;
        }

        public void SerializeProperty(byte[] value)
        {
            if (!Exclude.Contains(_offset1))
            {
                for (var x = 0; x < value.Length; x++)
                {
                    _BufferOne[_offset1 + x] = value[x];
                    _BufferTwo[_offset1 - dif + x] = value[x];
                }
            }
            else 
            {
                for (var x = 0; x < value.Length; x++)
                {
                    _BufferOne[_offset1 + x] = value[x];
                }

                dif += value.Length; 
            }

            _offset1 += value.Length;
        }
    }
}
