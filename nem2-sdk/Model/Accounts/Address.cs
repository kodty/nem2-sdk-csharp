using System.Text.RegularExpressions;
using Coppery;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Model.Accounts
{
    public class Address
    {
        internal struct Constants
        {
            internal static int Ripemd160 = 20;

            internal static int AddressDecoded = 24;
 
            internal static int AddressEncoded = 39;

            internal static int Key = 32;

            internal static int LongKey = 64;

            internal static int Checksum = 3;

            internal static int NetworkByte = 1;
        }

        private string _Address { get; }

        public NetworkType.Types NetworkByte { get; }

        public string Plain => _Address;      

        public string Pretty => Regex.Replace(_Address, ".{6}", "$0-");
      
        public static NetworkType.Types GetNetworkType(string address)
        {
            if (address.Length == Constants.AddressDecoded) 
                    address = Address.EncodeAddress(address);

            switch (address.ToCharArray()[0])
            {
                case 'S':
                    return NetworkType.Types.MIJIN_TEST;
                    
                case 'M':
                    return NetworkType.Types.MIJIN;
                    
                case 'T':
                    return NetworkType.Types.TEST_NET;
                    
                case 'N':
                    return NetworkType.Types.MAIN_NET;
                   
                default:
                    throw new Exception("Address Network unsupported");
            }
        }

        public static NetworkType.Types GetNetworkType(byte[] address)
        {
            switch ((int)address[0])
            {
                case 144:
                    return NetworkType.Types.MIJIN_TEST;

                case 96:
                    return NetworkType.Types.MIJIN;

                case 152:
                    return NetworkType.Types.TEST_NET;

                case 104:
                    return NetworkType.Types.MAIN_NET;

                default:
                    throw new Exception("Address Network unsupported");
            }
        }

        public static Address CreateFromEncoded(string encodedAddress)
        {
            var addressTrimAndUpperCase = encodedAddress
                .Trim()
                .ToUpper()
                .Replace("-", "");

            return new Address(addressTrimAndUpperCase, GetNetworkType(addressTrimAndUpperCase));
        }

        public static Address CreateFromHex(string address)
        {
            return CreateFromEncoded(Address.EncodeAddress(address));
        }
        
        public static Address CreateFromPublicKey(string publicKey, NetworkType.Types networkType)
        {
            // step 1) sha-3(256) public key
            var digestSha3 = new Sha3Digest(256);
            var stepOne = new byte[Constants.Key];

            digestSha3.BlockUpdate(publicKey.FromHex(), 0, Constants.Key);
            digestSha3.DoFinal(stepOne, 0);

            // step 2) perform ripemd160 on previous step
            var digestRipeMd160 = new RipeMD160Digest();
            var stepTwo = new byte[Constants.Ripemd160];
            digestRipeMd160.BlockUpdate(stepOne, 0, Constants.Key);
            digestRipeMd160.DoFinal(stepTwo, 0);

            // step3) prepend network byte    
            var stepThree = new []{networkType.GetNetworkByte()}.Concat(stepTwo).ToArray();

            // step 4) perform sha3 on previous step
            var stepFour = new byte[Constants.Key];
            digestSha3.BlockUpdate(stepThree, 0, Constants.NetworkByte + Constants.Ripemd160);
            digestSha3.DoFinal(stepFour, 0);

            // step 5) retrieve checksum
            var stepFive = new byte[Constants.Checksum];
            Array.Copy(stepFour, 0, stepFive, 0, Constants.Checksum);

            // step 6) append stepFive to resulst of stepThree
            var stepSix = new byte[Constants.AddressDecoded + 1];
            Array.Copy(stepThree, 0, stepSix, 0, Constants.NetworkByte + Constants.Ripemd160);
            Array.Copy(stepFive, 0, stepSix, Constants.NetworkByte + Constants.Ripemd160, Constants.Checksum);

            // step 7) return base 32 encode address byte array         
            return CreateFromEncoded(Address.EncodeAddress(stepSix));
        }
      
        public Address(string address, NetworkType.Types network)
        {
            _Address = Regex.Replace(address.Replace("-", ""), @"\s+", "").ToUpper();
            NetworkByte = network;
        }

        private readonly static char[] Base32Characters
            = ['A', 'B', 'C', 'D', 'E', 'F', 'G',
               'H', 'I', 'J', 'K', 'L', 'M', 'N',
               'O', 'P', 'Q', 'R', 'S', 'T', 'U',
               'V', 'W', 'X', 'Y', 'Z',
               '2', '3', '4', '5', '6', '7'];

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

            var bytes = new byte[25];

            for (int i = 0; i < hexString.Length / 2; i++)
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return EncodeAddress(bytes);
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
