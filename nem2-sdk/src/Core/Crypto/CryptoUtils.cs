using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using TweetNaclSharp;
using TweetNaclSharp.Core.Extensions;

namespace io.nem2.sdk.Core.Crypto
{
    public static class CryptoUtils
    {
        public static string Encode(string text, string secretKey, string publicKey)
        {
            var random = new SecureRandom();

            var salt = new byte[32];
            random.NextBytes(salt);

            var ivData = new byte[16];
            random.NextBytes(ivData);

            return _Encode(
                Convert.FromHexString(secretKey),
                Convert.FromHexString(publicKey),
                text,
                ivData,
                salt);
        }

        public static string Decode(byte[] text, string secretKey, string publicKey)
        {
            return _Decode(
                Convert.FromHexString(secretKey),
                Convert.FromHexString(publicKey),
                text);
        }

        internal static string DerivePassSha(byte[] password, int count)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if(count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "must be positive");
            
            var sha3Hasher = new KeccakDigest(256);
            var hash = new byte[32];

            sha3Hasher.BlockUpdate(password, 0, password.Length);
            sha3Hasher.DoFinal(hash, 0);

            for (var i = 0; i < count - 1; ++i)
            {
                sha3Hasher.Reset();
                sha3Hasher.BlockUpdate(hash, 0, hash.Length);
                sha3Hasher.DoFinal(hash, 0);
            }

            return Convert.ToHexString(hash);
        }

        internal static string _Encode(byte[] privateKey, byte[] publicKey, string msg, byte[] iv, byte[] salt)
        {
            var shared = new byte[32];

            var longKeyHash = new byte[64];
            var shortKeyHash = new byte[32];

            Array.Reverse(privateKey);

            // compute  Sha3(512) hash of secret key (as in prepareForScalarMultiply)
            var digestSha3 = new KeccakDigest(512);
            digestSha3.BlockUpdate(privateKey, 0, 32);
            digestSha3.DoFinal(longKeyHash, 0);

            longKeyHash[0] &= 248;
            longKeyHash[31] &= 127;
            longKeyHash[31] |= 64;

            Array.Copy(longKeyHash, 0, shortKeyHash, 0, 32);

            shortKeyHash[0 + 0] &= 248;
            shortKeyHash[0 + 31] &= 127;
            shortKeyHash[0 + 31] |= 64;

            var p = new[] { new long[16], new long[16], new long[16], new long[16] };
            var q = new[] { new long[16], new long[16], new long[16], new long[16] };

            var unpackneg = typeof(NaclFast).GetMethod("Unpackneg", BindingFlags.Static | BindingFlags.NonPublic)!;

            var unpacknegResult = (int)unpackneg.Invoke(null, [ q, publicKey ])!;

            var scalarmult = typeof(NaclFast).GetMethod("Scalarmult", BindingFlags.Static | BindingFlags.NonPublic)!;

            scalarmult.Invoke(null, [ p, q, shortKeyHash ]);

            var packMethod = typeof(NaclFast).GetMethod("Pack", BindingFlags.Static | BindingFlags.NonPublic)!;

            packMethod.Invoke(null, [ shared, p ]);

            //NaclFast.Unpackneg(q, publicKey); // returning -1 invalid signature
            //NaclFast.Scalarmult(p, q, shortKeyHash);
            //NaclFast.Pack(shared, p);

            // for some reason the most significant bit of the last byte needs to be flipped.
            // doesnt seem to be any corrosponding action in nano/nem.core, so this may be an issue in one of the above 3 functions. i have no idea.
            shared[31] ^= (1 << 7);

            // salt
            for (var i = 0; i < salt.Length; i++)
            {
                shared[i] ^= salt[i];
            }

            // hash salted shared key
            var digestSha3Two = new KeccakDigest(256);
            digestSha3Two.BlockUpdate(shared, 0, 32);
            digestSha3Two.DoFinal(shared, 0);

            return Convert.ToHexString(salt) + AesEncryptor(shared, iv, msg);        
        }

        internal static string _Decode(byte[] privateKey, byte[] publicKey, byte[] data)
        {
            var salt = data.SubArray(0, 32).ToArray();
            var iv = data.SubArray(32, 16);
            var payload = data.SubArray(48, data.Length - 48);
            var shared = new byte[32];
           
            return AesDecryptor(shared, iv, payload);
        }

        internal static string AesEncryptor(byte[] key, byte[] iv, string msg)
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

                        return Convert.ToHexString(iv) + Convert.ToHexString(msEncrypt.ToArray());
                    }
                }
            }
        }

        internal static string AesDecryptor(byte[] key, byte[] iv, byte[] payload)
        {
            using (var aesAlg = Aes.Create())
            {
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
