namespace CopperCurve
{
    public static class AddressEncoder
    {
        private readonly static char[] Base32Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

        public static string EncodeAddress(byte[] input)
        {
            if (input.Length != 25)
                throw new Exception("padding missing");

            char[] chunks = new char[input.Length / 5 * 8];

            for (int i = 0; i < input.Length / 5; i++)
                ReturnAddressChunk(input, i * 5, chunks, i * 8);

            return string.Concat(chunks.Take(39));
        }

        public static string EncodeAddress(string hexString)
        {
            if (hexString.Length != 48 && hexString.Length != 50)
                throw new Exception("decoded address is invalid length, must be 48 or 50 with padding.");

            byte[] input = FromHex(hexString);

            return EncodeAddress(input);
        }

        private static byte[] FromHex(string hexString)
        {
            var bytes = new byte[25];

            for (int i = 0; i < hexString.Length / 2; i++)
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return bytes;
        }

        public static byte[] DecodeAddress(string encodedAddress)
        {
            byte[] input = new byte[40];

            for (int i = 0; i < encodedAddress.Length; i++)
                input[i] = Convert.ToByte(Base32Characters.ToList().IndexOf(encodedAddress[i]));

            byte[] output = new byte[25];

            for (int i = 0; i < input.Length / 8; i++)
                DecodeCharBlock(input, i * 8, output, i * 5);

            return output.Take(24).ToArray();
        }

        private static void DecodeCharBlock(byte[] input, int inputOffset, byte[] output, int outputOffset)
        {
            output[outputOffset + 0] = (byte)(input[inputOffset + 0] << 3 | input[inputOffset + 1] >> 2);
            output[outputOffset + 1] = (byte)((input[inputOffset + 1] & 0x03) << 6 | input[inputOffset + 2] << 1 | input[inputOffset + 3] >> 4);
            output[outputOffset + 2] = (byte)((input[inputOffset + 3] & 0x0F) << 4 | input[inputOffset + 4] >> 1);
            output[outputOffset + 3] = (byte)((input[inputOffset + 4] & 0x01) << 7 | input[inputOffset + 5] << 2 | input[inputOffset + 6] >> 3);
            output[outputOffset + 4] = (byte)((input[inputOffset + 6] & 0x07) << 5 | input[inputOffset + 7]);
        }

        private static char[] ReturnAddressChunk(byte[] input, int inputOffset, char[] chunk, int outputOffset)
        {
            chunk[outputOffset + 0] = Base32Characters[input[inputOffset + 0] >> 3];
            chunk[outputOffset + 1] = Base32Characters[(input[inputOffset + 0] & 0x07) << 2 | input[inputOffset + 1] >> 6];
            chunk[outputOffset + 2] = Base32Characters[(input[inputOffset + 1] & 0x3E) >> 1];
            chunk[outputOffset + 3] = Base32Characters[(input[inputOffset + 1] & 0x01) << 4 | input[inputOffset + 2] >> 4];
            chunk[outputOffset + 4] = Base32Characters[(input[inputOffset + 2] & 0x0F) << 1 | input[inputOffset + 3] >> 7];
            chunk[outputOffset + 5] = Base32Characters[(input[inputOffset + 3] & 0x7F) >> 2];
            chunk[outputOffset + 6] = Base32Characters[(input[inputOffset + 3] & 0x03) << 3 | input[inputOffset + 4] >> 5];
            chunk[outputOffset + 7] = Base32Characters[input[inputOffset + 4] & 0x1F];

            return chunk;
        }
    }
}
