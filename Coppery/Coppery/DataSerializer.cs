using System.Reflection;

namespace Coppery
{
    public class DataSerializer
    {
        internal byte[] _Buffer { get; set; }
        private int _offset = 0;

        public DataSerializer(uint size)
        {
            _Buffer = new byte[size];
        }

        public byte[] GetBytes()
        {
            return _Buffer;
        }

        public void Serialize(object obj, Type type)
        {
            foreach (var item in type.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                FilterProperties(obj, item);

            foreach (var item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(e => e.DeclaringType != type.BaseType))
                FilterProperties(obj, item);
        }

        private void FilterProperties(object obj, PropertyInfo op)
        {
            if (op.PropertyType.IsPrimitive || op.PropertyType == typeof(byte[]))
                SerializeProperty(op.GetValue(obj), op.PropertyType);
            
            if (!op.PropertyType.IsPrimitive && op.PropertyType != typeof(byte[]))
                Serialize(op.GetValue(obj), op.PropertyType);
        }
    
        private void SerializeProperty(object value, Type type)
        {
            if (type == typeof(byte))
            {
                _Buffer[_offset++] = (byte)value;
                
                return;
            }      
            if (type == typeof(ushort))
            {
                var source = DataConverter.ConvertFrom((ushort)value);

                for (var x = 0; x < 2; x++)
                    _Buffer[_offset + x] = source[x];

                _offset += source.Length;

                return;
            }
            if (type == typeof(uint))
            {
                var source = DataConverter.ConvertFrom((uint)value);

                for (var x = 0; x < 4; x++)
                    _Buffer[_offset + x] = source[x];

                _offset += source.Length;

                return;
            }
            if (type == typeof(ulong))
            {
                var source = DataConverter.ConvertFrom((ulong)value);

                for(var x = 0; x < 8; x++)
                    _Buffer[_offset + x] = source[x];
                
                _offset += source.Length;

                return;
            }
            if (type == typeof(bool))
            {
                _Buffer[_offset++] = (byte)value;

                return;
            }
            if (type == typeof(byte[]))
            {
                var source = (byte[])value;

                for (var x = 0; x < source.Length; x++)
                    _Buffer[_offset + x] = source[x];

                _offset += source.Length;

                return;
            }
            else throw new NotImplementedException("Type " + type.ToString() + "unsupported");
        }
    }
}
