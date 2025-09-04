using System.Reflection;

namespace Coppery.Coppery
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
            if (IsNativeProperty(op))
            {
                SerializeProperty(op.GetValue(obj), op.PropertyType);
            }

            else if (!IsNativeProperty(op))
            {
                Serialize(op.PropertyType, op.GetValue(obj), embedded);
            }
        }

        public void Serialize(Type type, object obj, bool embedded)
        {
            foreach (var item in type.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                FilterProperties(obj, item, embedded);
            }

            foreach (var item in type.GetProperties().Where(e => e.DeclaringType != type.BaseType))
            {
                FilterProperties(obj, item, embedded);
            }
        }

        internal bool IsNativeProperty(PropertyInfo op)
        {
            if (op.PropertyType.IsPrimitive
             || op.PropertyType == typeof(byte[])
             || op.PropertyType == typeof(Tuple<byte[], ulong>))
            { return true; }
            else return false;
        }

        private void SerializeProperty(object ob, Type type)
        {
            if (type == typeof(byte))
            {
                var source = new byte[1] { (byte)ob };

                _offset += BlockCopy(ref source, _offset);

                return;
            }
            if (type == typeof(uint))
            {
                var source = DataWriter.Write((uint)ob);

                _offset += BlockCopy(ref source, _offset);

                return;
            }
            if (type == typeof(ushort))
            {
                var source = DataWriter.Write((ushort)ob);

                _offset += BlockCopy(ref source, _offset);

                return;
            }
            if (type == typeof(ulong))
            {
                var source = DataWriter.Write((ulong)ob);

                _offset += BlockCopy(ref source, _offset);

                return;
            }
            if (type == typeof(bool))
            {
                var source = new byte[1] { (byte)ob };

                _offset += BlockCopy(ref source, _offset);

                return;
            }
            if (type == typeof(byte[]))
            {
                var source = (byte[])ob;

                _offset += BlockCopy(ref source, _offset);

                return;
            }
            if (type == typeof(Tuple<byte[], ulong>))
            {
                var source = ((Tuple<byte[], ulong>)ob).Item1;

                _offset += BlockCopy(ref source, _offset);

                var source2 = DataWriter.Write(((Tuple<byte[], ulong>)ob).Item2);

                _offset += BlockCopy(ref source2, _offset);

                return;
            }
            else throw new NotImplementedException("Type " + type.ToString() + "unsupported");
        }

        public int BlockCopy(ref byte[] src, int offset)
        {
            Buffer.BlockCopy(src, 0, _Buffer, offset, src.Length);

            return src.Length;
        }
    }
}
