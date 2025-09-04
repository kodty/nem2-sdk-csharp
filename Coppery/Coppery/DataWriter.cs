namespace Coppery
{
    public static class DataWriter
    {
        public static byte[] Write(this string[] value)
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

        public static byte[] Write(this ulong data)
        {
            byte[] Bytes = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                Bytes[i] = (byte)(data >> (/*8 - 1 - */ i) * 8);
            }

            return Bytes;
        }

        public static byte[] Write(this uint data)
        {
            byte[] Bytes = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                Bytes[i] = (byte)(data >> (/*4 - 1 - */ i) * 8);
            }

            return Bytes;
        }

        public static byte[] Write(this ushort data)
        {
            byte[] Bytes = new byte[2];

            for (int i = 0; i < 2; i++)
            {
                Bytes[i] = (byte)(data >> (/*2 - 1 - */ i) * 8); 
            }

            return Bytes;
        }
    }
}
