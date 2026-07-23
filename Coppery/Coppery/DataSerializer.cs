namespace Coppery
{
    public class DataSerializer
    {
        internal byte[][] _Buffer { get; set; }
        private int _offset1 = 0;
        private int _offset2 = 0;
        internal uint[] S1 { get; set; }
        internal uint[] S2 { get; set; }

        public DataSerializer(uint[][] s)
        {
            _Buffer = new byte[2][];
            _Buffer[0] = new byte[s[0][0]];
            _Buffer[1] = new byte[s[1][0]];

            S1 = s[0];
            S2 = s[1];     
        }

        public byte[][] GetBytes()
        {
            return _Buffer;
        }

        public void SerializeProperty(object value, Type type, uint ix)
        {
            if (type == typeof(byte))
            {
                if (!S1.Contains(ix))
                {
                    _Buffer[0][_offset1++] = (byte)value;

                    if (!S2.Contains(ix))
                        _Buffer[1][_offset2++] = (byte)value;
                }

                return;
            }      
            if (type == typeof(ushort))
            {
                var source = DataConverter.ConvertFrom((ushort)value);

                if (!S1.Contains(ix))
                {
                    for (var x = 0; x < 2; x++)
                        _Buffer[0][_offset1 + x] = source[x];

                    _offset1 += source.Length;

                    if (!S2.Contains(ix))
                    {
                        for (var x = 0; x < 2; x++)
                            _Buffer[1][_offset2 + x] = source[x];

                        _offset2 += source.Length;
                    }
                }

                return;
            }
            if (type == typeof(uint))
            {
                var source = DataConverter.ConvertFrom((uint)value);

                if (!S1.Contains(ix))
                {
                    for (var x = 0; x < 4; x++)
                        _Buffer[0][_offset1 + x] = source[x];

                    _offset1 += source.Length;

                    if (!S2.Contains(ix))
                    {
                        for (var x = 0; x < 4; x++)
                            _Buffer[1][_offset2 + x] = source[x];

                        _offset2 += source.Length;
                    }
                }
                 

                return;
            }
            if (type == typeof(ulong))
            {
                var source = DataConverter.ConvertFrom((ulong)value);

                if (!S1.Contains(ix))
                {
                    for (var x = 0; x < 8; x++)
                        _Buffer[0][_offset1 + x] = source[x];

                    _offset1 += source.Length;

                    if (!S2.Contains(ix))
                    {
                        for (var x = 0; x < 8; x++)
                            _Buffer[1][_offset2 + x] = source[x];

                        _offset2 += source.Length;
                    }
                }

                return;
            }
            if (type == typeof(bool))
            {
                if (!S1.Contains(ix))
                {
                    _Buffer[0][_offset1++] = (byte)value;

                    if (!S2.Contains(ix))
                    {
                        _Buffer[1][_offset2++] = (byte)value;
                    }
                }

                return;
            }
            if (type == typeof(byte[]))
            {
                var source = (byte[])value;

                if (!S1.Contains(ix))
                {
                    for (var x = 0; x < source.Length; x++)
                        _Buffer[0][_offset1 + x] = source[x];

                    _offset1 += source.Length;

                    if (!S2.Contains(ix))
                    {
                        for (var x = 0; x < source.Length; x++)
                            _Buffer[1][_offset2 + x] = source[x];

                        _offset2 += source.Length;
                    }
                }

                return;
            }
            else throw new NotImplementedException("Type " + type.ToString() + "unsupported");
        }
    }
}
