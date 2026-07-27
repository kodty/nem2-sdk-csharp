namespace Coppery
{
    public class DataSerializer
    {
        internal uint[] Exclude { get; set; }
        internal byte[][] _Buffer { get; set; }

        private int _offset1 = 0;
        private int _offset2 = 0;

        public DataSerializer(uint size, uint[] exclude)
        {
            _Buffer = new byte[2][];
            _Buffer[0] = new byte[size];
            _Buffer[1] = new byte[size - exclude[0]];

            Exclude = exclude;
        }

        public byte[][] GetBytes()
        {
            return _Buffer;
        }

        public void SerializeProperty(byte value, uint ix)
        {
            
            _Buffer[0][_offset1++] = value;

            if (!Exclude.Contains(ix))
                _Buffer[1][_offset2++] = value;    
        }

        public void SerializeProperty(uint value, uint ix)
        {
            var source = DataConverter.ConvertFrom(value);
         
            for (var x = 0; x < 4; x++)
                _Buffer[0][_offset1 + x] = source[x];

            _offset1 += source.Length;

            if (!Exclude.Contains(ix))
            {
                for (var x = 0; x < 4; x++)
                    _Buffer[1][_offset2 + x] = source[x];

                _offset2 += source.Length;
            }      
        }

        public void SerializeProperty(ushort value, uint ix)
        {
            var source = DataConverter.ConvertFrom(value);
        
            for (var x = 0; x < 2; x++)
                _Buffer[0][_offset1 + x] = source[x];

            _offset1 += source.Length;

            if (!Exclude.Contains(ix))
            {
                for (var x = 0; x < 2; x++)
                    _Buffer[1][_offset2 + x] = source[x];

                _offset2 += source.Length;
            }         
        }

        public void SerializeProperty(ulong value, uint ix)
        {
            var source = DataConverter.ConvertFrom(value);
           
            for (var x = 0; x < 8; x++)
                _Buffer[0][_offset1 + x] = source[x];

            _offset1 += source.Length;

            if (!Exclude.Contains(ix))
            {
                for (var x = 0; x < 8; x++)
                    _Buffer[1][_offset2 + x] = source[x];

                _offset2 += source.Length;
            }          
        }

        public void SerializeProperty(byte[] value, uint ix)
        {        
            for (var x = 0; x < value.Length; x++)
                _Buffer[0][_offset1 + x] = value[x];

            _offset1 += value.Length;

            if (!Exclude.Contains(ix))
            {
                for (var x = 0; x < value.Length; x++)
                    _Buffer[1][_offset2 + x] = value[x];

                _offset2 += value.Length;
            }       
        }
    }
}
