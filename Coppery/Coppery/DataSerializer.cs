namespace Coppery
{
    public class DataSerializer
    {
        internal int[] Exclude = new int[] { 0, 4, 8, 72, 104 };
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

        public void SerializeProperty(uint value)
        {
            var source = DataConverter.ConvertFrom(value);
         
            for (var x = 0; x < 4; x++)
                _BufferOne[_offset1 + x] = source[x];  

            if (!Exclude.Contains(_offset1))
            {
                for (var x = 0; x < 4; x++)
                    _BufferTwo[_offset1 - dif + x] = source[x];
            }
            else dif += source.Length;

            _offset1 += source.Length;
        }

        public void SerializeProperty(ushort value)
        {
            var source = DataConverter.ConvertFrom(value);
        
            for (var x = 0; x < 2; x++)
            {
                _BufferOne[_offset1 + x] = source[x];
                _BufferTwo[_offset1 - dif + x] = source[x];
            }
               
            _offset1 += source.Length;
        }

        public void SerializeProperty(ulong value)
        {
            var source = DataConverter.ConvertFrom(value);
           
            for (var x = 0; x < 8; x++)
            {
                _BufferOne[_offset1 + x] = source[x];
                _BufferTwo[_offset1 - dif + x] = source[x];
            }

            _offset1 += source.Length;
        }

        public void SerializeProperty(byte[] value)
        {
            for (var x = 0; x < value.Length; x++)
                _BufferOne[_offset1 + x] = value[x];    

            if (!Exclude.Contains(_offset1))
            {
                for (var x = 0; x < value.Length; x++)
                    _BufferTwo[_offset1 - dif + x] = value[x];

            }
            else dif += value.Length;

            _offset1 += value.Length;
        }
    }
}
