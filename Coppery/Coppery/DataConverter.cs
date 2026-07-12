namespace Coppery
{
    public static class DataConverter
    {
        public static byte[] FromHex(this string hexString) => Convert.FromHexString(hexString); 

        public static string ToHex(this byte[] data) => Convert.ToHexString(data);

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
                p[i] = (byte)(value >> (/*4 - 1 - */i) * 8);
            }

            return p;
        }

        public static byte[] ConvertFrom(ushort value)
        {
            byte[] p = new byte[2];

            for (int i = 0; i < 2; i++)
            {
                p[i] = (byte)(value >> (/*2 - 1 - */i) * 8);
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
    }
}
