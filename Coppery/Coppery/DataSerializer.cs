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

        private void FilterProperties(object obj, PropertyInfo op, bool embedded)
        {
            if (op.PropertyType.IsPrimitive || op.PropertyType == typeof(byte[]))
                SerializeProperty(op.GetValue(obj), op.PropertyType);
            
            if (!op.PropertyType.IsPrimitive && op.PropertyType != typeof(byte[]))
                Serialize(op.PropertyType, op.GetValue(obj), embedded);
        }

        public void Serialize(Type type, object obj, bool embedded)
        {
            foreach (var item in type.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                FilterProperties(obj, item, embedded);

            foreach (var item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(e => e.DeclaringType != type.BaseType))
                FilterProperties(obj, item, embedded);
        }

        private void SerializeProperty(object ob, Type type)
        {
            if (type == typeof(byte))
            {
                _Buffer[_offset++] = (byte)ob;
                
                return;
            }      
            if (type == typeof(ushort))
            {
                var source = DataConverter.ConvertFrom((ushort)ob);

                for (var x = 0; x < 2; x++)
                    _Buffer[_offset + x] = source[x];

                _offset += source.Length;

                return;
            }
            if (type == typeof(uint))
            {
                var source = DataConverter.ConvertFrom((uint)ob);

                for (var x = 0; x < 4; x++)
                    _Buffer[_offset + x] = source[x];

                _offset += source.Length;

                return;
            }
            if (type == typeof(ulong))
            {
                var source = DataConverter.ConvertFrom((ulong)ob);

                for(var x = 0; x < 8; x++)
                    _Buffer[_offset + x] = source[x];
                
                _offset += source.Length;

                return;
            }
            if (type == typeof(bool))
            {
                _Buffer[_offset++] = (byte)ob;

                return;
            }
            if (type == typeof(byte[]))
            {
                var source = (byte[])ob;

                for (var x = 0; x < source.Length; x++)
                    _Buffer[_offset + x] = source[x];

                _offset += source.Length;

                return;
            }
            else throw new NotImplementedException("Type " + type.ToString() + "unsupported");
        }
    }
}
