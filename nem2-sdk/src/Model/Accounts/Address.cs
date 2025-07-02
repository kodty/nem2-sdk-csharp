using System.Text.RegularExpressions;
using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;
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
                    address = AddressEncoder.EncodeAddress(address);

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
            return CreateFromEncoded(AddressEncoder.EncodeAddress(address));
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
            return CreateFromEncoded(AddressEncoder.EncodeAddress(stepSix));
        }
      
        public Address(string address, NetworkType.Types network)
        {
            _Address = Regex.Replace(address.Replace("-", ""), @"\s+", "").ToUpper();
            NetworkByte = network;
        }  
    }
}
