
using CopperCurve;
using io.nem2.sdk.Core.Crypto.Chaos.NaCl.Internal.Ed25519ref10;
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

            GroupElementP3 A;
            int i;

            byte[] h = new byte[64];
            byte[] pk = new byte[32];

            SHA512.HashData(privateKeyArray, h);

            ScalarOperations.sc_clamp(h, 0);
            GroupOperations.ge_scalarmult_base(out A, h, 0);
            GroupOperations.ge_p3_tobytes(pk, 0, ref A);

            Array.Clear(h, 0, h.Length);

            return new SecretKeyPair(privateKey, pk.ToHex());
        }

        internal byte[] Sign(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return NaclFast.SignDetached(data, PrivateKey.Concat(PublicKey).ToArray());
        }
    }
}
