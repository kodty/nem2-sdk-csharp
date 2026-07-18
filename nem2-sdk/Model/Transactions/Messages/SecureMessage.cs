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
            return Decode(privateKey.FromHex(), publicKey.FromHex(), Payload, 32, 16);
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

        public static byte[] HKDFDeriveSharedKey256(byte[] privateKey, byte[] otherPublicKey, byte[] info = null)
        {
            var sharedsecret = DeriveSharedSecret(privateKey, otherPublicKey, (byte[] key) =>
            {
                using(SHA512 sha = SHA512.Create())
                {
                    var hash = sha.ComputeHash(key, 0, 32);

                    return hash;
                }
            });

            return HKDF.Expand(HashAlgorithmName.SHA256, prk: sharedsecret, 32, info: info);
        }

        public static byte[] DeriveSharedSecret(byte[] privateKey, byte[] otherPublicKey, Func<byte[], byte[]> hashFunc)
        {
            var Gf = typeof(NaclFast).GetMethod("Gf", BindingFlags.Static | BindingFlags.NonPublic)!;

            long[][] point = [(long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null])];

            var unpackneg = typeof(NaclFast).GetMethod("Unpackneg", BindingFlags.Static | BindingFlags.NonPublic)!;

            if(!IsCanonicalKey(otherPublicKey) || 0 != (int)unpackneg.Invoke(null, [point, otherPublicKey])){
                throw new CryptographicException("invalid point");
            }

            var Z = typeof(NaclFast).GetMethod("Z", BindingFlags.Static | BindingFlags.NonPublic)!;

            Z.Invoke(null, [point[0], Gf.Invoke(null, [null]), point[0]]);
            Z.Invoke(null, [point[3], Gf.Invoke(null, [null]), point[3]]);

            byte[] scalar = hashFunc(privateKey);

            scalar[0] &= 248;
            scalar[31] &= 127;
            scalar[31] |= 64;

            long[][] result = [(long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null])];

            var scalarmult = typeof(NaclFast).GetMethod("Scalarmult", BindingFlags.Static | BindingFlags.NonPublic)!;

            scalarmult.Invoke(null, [result, point, scalar]);

            var pack = typeof(NaclFast).GetMethod("Pack", BindingFlags.Static | BindingFlags.NonPublic)!;

            byte[] sharedSecret = new byte[32];

            pack.Invoke(null, [sharedSecret, result]);

            return sharedSecret;
        }

        private static byte[] Encode(string text, string secretKey, string publicKey)
        {
            var random = new SecureRandom();

            var salt = new byte[32];
            random.NextBytes(salt);

            var ivData = new byte[16];
            random.NextBytes(ivData);

            var shared = HKDFDeriveSharedKey256(
                Convert.FromHexString(secretKey),
                Convert.FromHexString(publicKey), 
                salt);

            return salt.Concat(AesEncryptor(shared, ivData, text)).ToArray();

        }

        public static string Decode(byte[] privateKey, byte[] publicKey, byte[] data, int saltLen = 32, int ivLen = 16)
        {
            var salt = data.SubArray(0, saltLen).ToArray();
            var iv = data.SubArray(saltLen, ivLen);
            var payload = data.SubArray(saltLen + ivLen, data.Length - saltLen - ivLen);
            var shared = HKDFDeriveSharedKey256(privateKey, publicKey, data.Take(saltLen).ToArray());

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
