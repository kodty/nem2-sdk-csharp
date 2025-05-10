using System.Diagnostics;

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

        public DataSerializer(uint transactionSize)
        {
            Bytes = new byte[transactionSize];
        }

        public void WriteUlong(ulong data)
        {
            for (int i = 0; i < 8; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (8 - 1 - i) * 8);
            }

            _offset += 8;
        }

        public void WriteUInt(uint data)
        {
            for (int i = 0; i < 4; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (4 - 1 - i) * 8);
            }

            _offset += 4;
        }

        public void WriteUShort(ushort data)
        {
            for (int i = 0; i < 2; i++)
            {
                Bytes[_offset + i] = (byte)(data >> (2 - 1 - i) * 8);
            }

            _offset += 2;
        }

        public void WriteByte(byte data)
        {
            Bytes[_offset] = data;

            _offset += 1;
        }

        public void WriteBytes(byte[] data)
        {
            Debug.WriteLine(data.Length);
            Debug.WriteLine(_offset);
            for (var i = 0; i < data.Length; i++)
                Bytes[_offset + i] = data[i];

            _offset += data.Length;
        }

        public void Reserve(int reserved)
        {
            _offset += reserved;
        }

        public void WriteHexString(string hexString)
        {
            for (var i = 0; i < hexString.Length / 2; i++)
            {
                Bytes[_offset + i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            _offset += hexString.Length / 2;
        }

        public void WriteBase32(string encodedAddress)
        {
            var decoded = AddressEncoder.DecodeAddress(encodedAddress);

            for (var i = 0; i < decoded.Length; i++)
            {
                Bytes[_offset + i] = decoded[i];
            }

            _offset += decoded.Length;
        }
    }
}
