using Coppery;
using System.Security.Cryptography;
using TweetNaclSharp;

namespace io.nem2.sdk.src.Model
{
    public class SecretKeyPair : IKeyPair
    {       
        public byte[] PrivateKey { get; }

        public byte[] PublicKey { get; }

        public string PrivateKeyString => PrivateKey.ToHex();

        public string PublicKeyString => PublicKey.ToHex();

        internal SecretKeyPair(string privateKey, string publicKey)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            if (publicKey.Length != 64) throw new ArgumentException(nameof(publicKey));

            PrivateKey = privateKey.FromHex();

            PublicKey = publicKey.FromHex();
        }

        public static SecretKeyPair CreateFromPrivateKey(string privateKey)
        {
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            if (privateKey.Length != 64) throw new ArgumentException(nameof(privateKey));

            var privateKeyArray = privateKey.FromHex();

            var keyPair = NaclFast.SignKeyPairFromSeed(privateKeyArray);

            return new SecretKeyPair(keyPair.SecretKey.ToHex(), keyPair.PublicKey.ToHex());
        }

        public byte[] Sign(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return NaclFast.SignDetached(data, PrivateKey.Concat(PublicKey).ToArray());
        }
    }
}
