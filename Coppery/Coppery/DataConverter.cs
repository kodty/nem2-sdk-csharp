namespace Coppery
{
    public static class DataConverter
    {
        public static byte[] ConvertFrom(this string[] value)
        {
            int len = 0;

            foreach (var item in value)
            {
                if (item.IsHex()) len += item.Length / 2;
                if (item.IsBase32()) len += 24;
            }

            byte[] bitValues = new byte[len];

            int offset = 0;

            foreach (var item in value)
            {
                byte[] decoded = new byte[24];

                if (item.IsBase32())
                    decoded = AddressEncoder.DecodeAddress(item);

                if (item.IsHex())
                    decoded = item.FromHex();

                Buffer.BlockCopy(decoded, 0, bitValues, offset, decoded.Length);

                offset += decoded.Length;
            }

            return bitValues;
        }

        public static byte[] FromHex(this string hexString)
        {
            return Convert.FromHexString(hexString);
        }

        public static string ToHex(this byte[] data)
        {
            return Convert.ToHexString(data);
        }

        public static byte[] ConvertFrom(ulong value)
        {
            byte[] p = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                p[i] = (byte)(value >> (/*8 - 1 - */ i) * 8);
            }

            return p;
        }

        public static byte[] ConvertFrom(uint value)
        {
            byte[] p = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                p[i] = (byte)(value >> (/*8 - 1 - */i) * 8);
            }

            return p;
        }

        public static byte[] ConvertFrom(ushort value)
        {
            byte[] p = new byte[2];

            for (int i = 0; i < 2; i++)
            {
                p[i] = (byte)(value >> (/*8 - 1 - */i) * 8);
            }

            return p;
        }

        public static T ConvertTo<T>(this byte[] value)
        {
            ulong result = 0;

            for (int i = 0; i < value.Length; i++)
            {
                result <<= 8;
                result += value[value.Length - 1 - i];
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;

            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}
