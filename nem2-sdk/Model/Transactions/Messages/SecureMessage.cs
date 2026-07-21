using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Reflection;
using System.Security.Cryptography;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions.Messages
{
    public class SecureMessage : IMessage
    {
        private byte Type { get; }

        private byte Encoding { get; }

        private byte[] Payload { get; }

        private byte[] IV { get; }

        private byte[] Salt { get; }

        private byte[] Info { get; }

        public SecureMessage(byte type, byte encoding, byte[] payload)
        {
            Type = MessageType.Type.ENCRYPTED.GetValue();
            Encoding = encoding;
            Payload = payload;

        }

        public SecureMessage(byte type, byte encoding, byte[] payload, byte[] iv, byte[] salt = null, byte[] info = null)
        {
            Type = MessageType.Type.ENCRYPTED.GetValue();
            Encoding = encoding;
            Payload = payload;
            IV = iv;
            salt = salt;
            info = info;
        }

        public static SecureMessage Create(MessageType.Type cryption, MessageType.CipherEncoding encoding, byte[] cipherBytes)
        {
           return new SecureMessage(cryption.GetValue(), encoding.GetValue(), cipherBytes);
        }

        internal override byte GetMessageType()
        {
            return Type;
        }

        internal override byte GetEncodingType()
        {
            return Encoding;
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

        public static byte[] HKDF_Derive(byte[] sharedkey, byte[] info = null, byte[] salt = null)
        {
            var prk = HKDF.Extract(HashAlgorithmName.SHA256, ikm: sharedkey, salt: salt);

            return HKDF.Expand(HashAlgorithmName.SHA256, prk: prk, outputLength: 32, info: info);
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
            }

            scalar[0] &= 248;
            scalar[31] &= 127;
            scalar[31] |= 64;

            long[][] result = [(long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null]), (long[])Gf.Invoke(null, [null])];
            
            scalarmult.Invoke(null, [result, point, scalar]);

            CryptographicOperations.ZeroMemory(scalar);

            byte[] sharedSecret = new byte[32];

            pack.Invoke(null, [sharedSecret, result]);

            return sharedSecret;
        }

        public static byte[] AesGcmSivEncryptor(byte[] nonce, byte[] key, byte[] data, byte[] tag, byte[] info)
        {
            GcmSivBlockCipher blockCipher = new GcmSivBlockCipher();

            var parameters = new AeadParameters(new KeyParameter(key, 0, 32), 128, nonce, tag);

            blockCipher.Init(true, parameters);

            byte[] result = new byte[data.Length + tag.Length];

            blockCipher.ProcessBytes(data, 0, data.Length, result, 0);

            blockCipher.DoFinal(result);

            return result;
        }

        public static byte[] AesGcmSivDecryptor(byte[] nonce, byte[] key, byte[] input, byte[] tag)
        {
            GcmSivBlockCipher blockCipher = new GcmSivBlockCipher();

            var parameters = new AeadParameters(new KeyParameter(key, 0, 32), 128, nonce, tag);

            blockCipher.Init(false, parameters);

            byte[] result = new byte[input.Length - tag.Length];

            blockCipher.ProcessBytes(input, 0, input.Length, result, 0);

            blockCipher.DoFinal(result);

            return result;
        }

        /*
        public static byte[] AesGcmEncryptor_(byte[] nonce, byte[] key, string data, byte[] tag, byte[] info)
        {
            GcmBlockCipher blockCipher = new GcmBlockCipher(new AesLightEngine());

            var parameters = new AeadParameters(new KeyParameter(key, 0, 32), 128, nonce, tag);

            blockCipher.Init(true, parameters);

            byte[] input = data.FromHex();

            byte[] result = new byte[input.Length + tag.Length];

            blockCipher.ProcessBytes(input, 0, input.Length, result, 0);

            blockCipher.DoFinal(result);

            return result;
        }
        

        public static byte[] AesGcmDecryptor_(byte[] nonce, byte[] key, byte[] input, byte[] tag = null)
        {
            GcmBlockCipher blockCipher = new GcmBlockCipher(new AesEngine());

            var parameters = new AeadParameters(new KeyParameter(key, 0, 32), 128, nonce, tag);

            blockCipher.Init(false, parameters);

            byte[] result = new byte[input.Length - tag.Length];

            blockCipher.ProcessBytes(input, 0, input.Length, result, 0);

            blockCipher.DoFinal(result);

            return result;
        }
        */

        public static byte[] AesEncryptor(byte[] key, byte[] iv, string msg)
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
                        using (var swEncrypt = new StreamWriter(csEncrypt, System.Text.Encoding.UTF8))
                        {
                            swEncrypt.Write(msg);
                        }

                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        public static string AesDecryptor(byte[] key, byte[] iv, byte[] payload)
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
                        using (var srDecrypt = new StreamReader(csDecrypt, System.Text.Encoding.UTF8))
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
