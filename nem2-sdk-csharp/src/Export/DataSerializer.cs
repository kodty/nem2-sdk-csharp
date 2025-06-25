using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model2;
using io.nem2.sdk.src.Model.Network;
using System.Diagnostics;
using io.nem2.sdk.src.Export;
using System.Reflection;

namespace io.nem2.sdk.src.Export
{
    public static class DataConverter
    {
        public static byte[] ConvertFromUInt64(this ulong value)
        {
            byte[] p = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                p[i] = (byte)(value >> i * 8);
            }

            return p;
        }

        public static byte[] ConvertFromUInt32(this uint value)
        {
            byte[] p = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                p[i] = (byte)(value >> i * 8);
            }

            return p;
        }

        public static ulong ConvertToUInt64(this byte[] value)
        {
            ulong result = 0;

            for (int i = 0; i < value.Length; i++)
            {
                result <<= 8;
                result += value[value.Length - 1 - i];
            }

            return result;
        }

        public static uint ConvertToUInt32(this byte[] value)
        {
            uint result = 0;

            for (int i = 0; i < value.Length; i++)
            {
                result <<= 8;
                result += value[value.Length - 1 - i];
            }

            return result;
        }
    }

    public class DataSerializer
    {
        public byte[] Bytes;
        public int _offset = 0;

        public DataSerializer()
        {
            Bytes = new byte[0];
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

            else if (op.PropertyType == typeof(EntityBody))
            {
                Serialize<EntityBody>(op.GetValue(obj));
            }
        }
        public void Serialize<T>(object obj)
        {
            foreach (var item in typeof(T).BaseType.GetProperties())
            {
                FilterProperties(obj, item);
            }
            foreach (var item in typeof(T).GetProperties().Where(e => e.DeclaringType != typeof(T).BaseType))
            {
                FilterProperties(obj, item);
            }
        }
        
        internal bool IsNativeProperty(PropertyInfo op)
        {
            if (op.PropertyType == typeof(byte)
             || op.PropertyType == typeof(uint)
             || op.PropertyType == typeof(ushort)
             || op.PropertyType == typeof(ulong)
             || op.PropertyType == typeof(string)
             || op.PropertyType == typeof(bool)
             || op.PropertyType == typeof(byte[])
             || op.PropertyType == typeof(Tuple<string, ulong>)
             || op.PropertyType == typeof(NetworkType.Types)
             || op.PropertyType == typeof(TransactionTypes.Types)
             || op.PropertyType == typeof(Deadline))
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
            if (type == typeof(string))
            {
                var str = (string)ob;
                
                if (str.IsHex(str.Length))
                    this.WriteBytes(str.FromHex());
                if (str.IsBase32(str.Length))
                    this.WriteBytes(AddressEncoder.DecodeAddress(str));
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
            if (type == typeof(Tuple<string, ulong>))
            {
                this.WriteBytes(((Tuple<string, ulong>)ob).Item1.FromHex());
                this.WriteUlong(((Tuple<string, ulong>)ob).Item2);
                return;
            }
            if (type == typeof(NetworkType.Types))
            {
                this.WriteByte(((NetworkType.Types)ob).GetNetworkByte());
                return;
            }              
            if (type == typeof(TransactionTypes.Types))
            {
                this.WriteUShort(((TransactionTypes.Types)ob).GetValue());
                return;
            }
            if (type == typeof(Deadline))
            {
                this.WriteUlong(((Deadline)ob).Ticks);
                return;
            }
            else throw new NotImplementedException("type " + type.ToString());
        }
    }
}
