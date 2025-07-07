namespace CopperCurve
{
    public static class DataConverter
    {
        public static byte[] FromHex(this string hexString)
        {
            return Convert.FromHexString(hexString);
        }

        public static string ToHex(this byte[] data)
        {
            return Convert.ToHexString(data);
        }

        public static byte[] ConvertFromUInt64(this ulong value)
        {
            byte[] p = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                p[i] = (byte)(value >> (/*8 - 1 - */ i) * 8);
            }

            return p;
        }

        public static byte[] ConvertFromUInt32(this uint value)
        {
            byte[] p = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                p[i] = (byte)(value >> (/*8 - 1 - */i) * 8);
            }

            return p;
        }

        public static byte[] ConvertFromUInt16(this ushort value)
        {
            byte[] p = new byte[2];

            for (int i = 0; i < 2; i++)
            {
                p[i] = (byte)(value >> (/*8 - 1 - */i) * 8);
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
}
