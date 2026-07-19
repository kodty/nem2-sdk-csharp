using Coppery;
using Org.BouncyCastle.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using TweetNaclSharp;
using TweetNaclSharp.Core.Extensions;

namespace io.nem2.sdk.Model.Transactions.Messages
{
    public class SecureMessage : IMessage
    {
        private byte Type { get; }

        private byte[] Payload { get; }

        public SecureMessage(byte[] payload)
        {
            Type = MessageType.Type.ENCRYPTED.GetValue();
            Payload = payload;
        }
        public static SecureMessage Create(string msg, string senderPrivateKey, string receiverPublicKey)
        {
            return new SecureMessage(Encode(msg, senderPrivateKey, receiverPublicKey));
        }

        public string GetDecodedPayload(string privateKey, string publicKey)
        {
            return Decode(privateKey.FromHex(), publicKey.FromHex(), Payload, 16);
        }

        internal override byte GetMessageType()
        {
            return Type;
        }

        public override byte[] GetPayload()
        {
            return Payload;
        }

        public override ushort GetLength()
        {
            return (ushort)Payload.Length;
        }

        // publicKey is canonical if the y coordinate is smaller than 2^255 - 19
        // note: this version is based on server version and should be constant-time
        // note 2: don't touch it, you'll break it
        // SharedKey.js#L18C1-L20C43
        private static bool IsCanonicalKey(byte[] publicKey)
        {
            var buffer = publicKey;
            int a = (buffer[31] & 0x7F) ^ 0x7F;
            for (int i = 30; 0 < i; --i)
                a |= buffer[i] ^ 0xFF;

            a = (a - 1) >>> 8;

            int b = (0xED - 1 - buffer[0]) >>> 8;
            return 0 != 1 - (a & b & 1);
        }

        public static byte[] HKDFExpand(byte[] sharedsecret, byte[] info)
        {
            return HKDF.Expand(HashAlgorithmName.SHA256, prk: sharedsecret, 32, info: info);
        }

        public static byte[] DeriveSharedKey(byte[] privateKey, byte[] otherPublicKey)
        {
            var Gf = typeof(NaclFast).GetMethod("Gf", BindingFlags.Static | BindingFlags.NonPublic)!;
            var unpackneg = typeof(NaclFast).GetMethod("Unpackneg", BindingFlags.Static | BindingFlags.NonPublic)!;
            var Z = typeof(NaclFast).GetMethod("Z", BindingFlags.Static | BindingFlags.NonPublic)!;
            var scalarmult = typeof(NaclFast).GetMethod("Scalarmult", BindingFlags.Static | BindingFlags.NonPublic)!;
            var pack = typeof(NaclFast).GetMethod("Pack", BindingFlags.Static | BindingFlags.NonPublic)!;

            long[][] point = [(long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null])];
           
            if (!IsCanonicalKey(otherPublicKey) || 0 != (int)unpackneg.Invoke(null, [point, otherPublicKey])){
                throw new CryptographicException("invalid point");
            }
           
            Z.Invoke(null, [point[0], Gf.Invoke(null, [null]), point[0]]);
            Z.Invoke(null, [point[3], Gf.Invoke(null, [null]), point[3]]);

            byte[] scalar = new byte[64];

            using (SHA512 sha = SHA512.Create())
            {
                scalar = sha.ComputeHash(privateKey, 0, 32);
                CryptographicOperations.ZeroMemory(privateKey);
            }

            scalar[0] &= 248;
            scalar[31] &= 127;
            scalar[31] |= 64;

            long[][] result = [(long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null])];
            
            scalarmult.Invoke(null, [result, point, scalar]);

            byte[] sharedSecret = new byte[32];

            pack.Invoke(null, [sharedSecret, result]);

            return sharedSecret;
        }

        private static byte[] Encode(string text, string secretKey, string publicKey)
        {
            var random = new SecureRandom();

            var ivData = new byte[16];
            random.NextBytes(ivData);

            var shared = DeriveSharedKey(
                Convert.FromHexString(secretKey),
                Convert.FromHexString(publicKey));

            return AesEncryptor(shared, ivData, text);

        }

        public static string Decode(byte[] privateKey, byte[] publicKey, byte[] data, int ivLen = 16)
        {
            var iv = data.SubArray(0, ivLen);
            var payload = data.SubArray(ivLen, data.Length - ivLen);
            var shared = DeriveSharedKey(privateKey, publicKey);

            return AesDecryptor(shared, iv, payload);
        }

        private static byte[] AesEncryptor(byte[] key, byte[] iv, string msg)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.BlockSize = 128;

                aesAlg.KeySize = 256;

                aesAlg.Key = key;

                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;

                aesAlg.Padding = PaddingMode.PKCS7;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                        {
                            swEncrypt.Write(msg);
                        }

                        return iv.Concat(msEncrypt.ToArray()).ToArray();
                    }
                }
            }
        }

        private static string AesDecryptor(byte[] key, byte[] iv, byte[] payload)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.BlockSize = 128;

                aesAlg.KeySize = 256;

                aesAlg.Key = key;

                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;

                aesAlg.Padding = PaddingMode.PKCS7;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(payload))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                        {
                            var a = srDecrypt.ReadToEnd();

                            return a;
                        }
                    }
                }
            }
        }
    }
}
