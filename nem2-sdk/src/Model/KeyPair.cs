using Coppery;
using TweetNaclSharp;
using TweetNaclSharp.Core.Extensions;

namespace io.nem2.sdk.src.Model
{
    public class SecretKeyPair : IKeyPair
    {       
        internal byte[] SecretKey { get; set; }

        public byte[] PrivateKey { get; }

        public byte[] PublicKey { get; }

        public string PrivateKeyString => PrivateKey.ToHex();

        public string PublicKeyString => PublicKey.ToHex();

        public SecretKeyPair(byte[] secretKey)
        {
            if (secretKey == null) throw new ArgumentNullException(nameof(secretKey));
            if (secretKey.Length != 64) throw new ArgumentException(nameof(secretKey));

            SecretKey = secretKey;

            PrivateKey = secretKey.SubArray(0, 32);

            PublicKey = secretKey.SubArray(32, 32);

        }
        internal SecretKeyPair(string privateKey, string publicKey)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            if (publicKey.Length != 64) throw new ArgumentException(nameof(publicKey));

            PrivateKey = privateKey.FromHex();

            PublicKey = publicKey.FromHex();
        }

        public static SecretKeyPair CreateFromSecretKey(string secretKey)
        {
            if (secretKey == null) throw new ArgumentNullException(nameof(secretKey));
            if (secretKey.Length != 128) throw new ArgumentException(nameof(secretKey));

            var privateKeyArray = secretKey.FromHex();

            var keyPair = NaclFast.SignKeyPairFromSeed(privateKeyArray);

            return new SecretKeyPair(keyPair.SecretKey);

        }
        public static SecretKeyPair CreateFromPrivateKey(string privateKey)
        {
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            if (privateKey.Length != 64) throw new ArgumentException(nameof(privateKey));

            var privateKeyArray = privateKey.FromHex();

            var keyPair = NaclFast.SignKeyPairFromSeed(privateKeyArray);

            return new SecretKeyPair(keyPair.SecretKey);
        }

        public byte[] Sign(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return NaclFast.SignDetached(data, SecretKey);
        }

        public bool SignDetachedVerify(byte[] msg, byte[] signature)
        {
            return NaclFast.SignDetachedVerify(msg, signature, PublicKey);
        }

        public byte[] SignOpen(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return NaclFast.SignOpen(data, PublicKey);
        }
    }
}
