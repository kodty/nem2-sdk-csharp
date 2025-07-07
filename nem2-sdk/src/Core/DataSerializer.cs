using System.Reflection;

namespace io.nem2.sdk.src.Export
{
    public class DataSerializer
    {
        public byte[] Bytes;
        public int _offset = 0;
        public DataSerializer()
        {
            Bytes = Array.Empty<byte>();
        }

        public void WriteUlong(ulong data)
        {
            Array.Resize(ref Bytes, Bytes.Length + 8);

            for (int i = 0; i < 8; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (/*8 - 1 - */ i) * 8);
            }

            _offset += 8;
        }

        public void WriteUInt(uint data)
        {
            Array.Resize(ref Bytes, Bytes.Length + 4);

            for (int i = 0; i < 4; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (/*4 - 1 - */ i) * 8);
            }

            _offset += 4;
        }

        public void WriteUShort(ushort data)
        {
            Array.Resize(ref Bytes, Bytes.Length + 2);

            for (int i = 0; i < 2; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (/*2 - 1 - */ i) * 8); 
            }

            _offset += 2;
        }

        public void WriteByte(byte data)
        {
            Array.Resize(ref Bytes, Bytes.Length + 1);

            Bytes[_offset] = data;

            _offset += 1;
        }

        public void WriteBytes(byte[] data)
        {
            Array.Resize(ref Bytes, Bytes.Length + data.Length);

            for (var i = 0; i < data.Length; i++)
                Bytes[_offset + i] = data[i];

            _offset += data.Length;
        }

        public void Reserve(int reserved)
        {
            Array.Resize(ref Bytes, Bytes.Length + 4);

            _offset += reserved;
        }

        public void WriteHexString(string hexString)
        {
            Array.Resize(ref Bytes, Bytes.Length + (hexString.Length / 2));

            for (var i = 0; i < hexString.Length / 2; i++)
            {
                Bytes[_offset + i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            _offset += hexString.Length / 2;
        }

        public void WriteBase32(string encodedAddress)
        {
            var decoded = AddressEncoder.DecodeAddress(encodedAddress);

            Array.Resize(ref Bytes, Bytes.Length + (decoded.Length / 2));

            for (var i = 0; i < decoded.Length; i++)
            {
                Bytes[_offset + i] = decoded[i];
            }

            _offset += decoded.Length;
        }

        private void FilterProperties(object obj, PropertyInfo op)
        {
            if (IsNativeProperty(op))
            {
                SerializeProperty(op.GetValue(obj), op.PropertyType);
            }

            else if (!IsNativeProperty(op))
            {
                Serialize(op.PropertyType, op.GetValue(obj));
            }
        }

        public void Serialize(Type type, object obj)
        {
            foreach (var item in type.BaseType.GetProperties())
            {
                FilterProperties(obj, item);
            }
            foreach (var item in type.GetProperties().Where(e => e.DeclaringType != type.BaseType))
            {
                FilterProperties(obj, item);
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
                this.WriteByte((byte)ob);
                return;
            }
            if (type == typeof(uint))
            {
                this.WriteUInt((uint)ob); 
                return;
            }              
            if (type == typeof(ushort))
            {
                this.WriteUShort((ushort)ob);
                return;
            }              
            if (type == typeof(ulong))
            {
                this.WriteUlong((ulong)ob);
                return;
            }                           
            if (type == typeof(bool))
            {
                this.WriteByte((byte)ob);
                return;
            }
            if (type == typeof(byte[]))
            {
                this.WriteBytes((byte[])ob);
                return;
            }
            if (type == typeof(Tuple<byte[], ulong>))
            {
                this.WriteBytes(((Tuple<byte[], ulong>)ob).Item1);
                this.WriteUlong(((Tuple<byte[], ulong>)ob).Item2);
                return;
            }
            else throw new NotImplementedException("Type " + type.ToString() + "unsupported");
        }
    }
}
