using Coppery;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model.Transactions;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;
using TweetNaclSharp;
using TweetNaclSharp.Core.Extensions;

namespace io.nem2.sdk.Model
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

        public static SecretKeyPair CreateFromSeed(byte[] seed)
        {
            var keyPair = NaclFast.SignKeyPairFromSeed(seed);

            return new SecretKeyPair(keyPair.SecretKey);
        }

        public static SecretKeyPair GenerateNewKeyPair()
        {
            var s = SecureRandom.GetInstance("SHA256PRNG");

            var digestSha3 = new Sha3Digest(256);
            var stepOne = new byte[32];
            digestSha3.BlockUpdate(s.GenerateSeed(2048), 0, 2048);
            digestSha3.DoFinal(stepOne, 0);
            digestSha3.Reset();

            return CreateFromSeed(stepOne);
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

        public SimpleTransaction<T> SignTransaction<T>(SimpleTransaction<T> transaction, byte[] networkGenHash) where T : TransactionExtension
        {
            if (transaction.Signer.ToHex() == PublicKeyString)
            {
                transaction.Prepare();

                byte[] signBytes = [.. networkGenHash, .. transaction.VerifiablePayload];

                var signature = NaclFast.SignDetached(msg: signBytes, SecretKey);

                transaction.Signature = signature;
                transaction.Hash = VerifiableTransaction.HashTransaction(transaction.Signature, PublicKey, signBytes).ToHex();

                return transaction;
            }
            else throw new Exception("signer mismatch");  
        }

        public SimpleTransaction<AggregatePayload> CosignTransaction(SimpleTransaction<AggregatePayload> transaction, byte[] networkGenHash)
        {
            var signature = NaclFast.SignDetached(msg: transaction.Hash.FromHex(), SecretKey);

            Cosignature cosig = new Cosignature
            {
                Version = 0,
                SignerPublicKey = PublicKeyString,
                Signature = signature.ToHex(), 
            };

            transaction.TransactionExtension.Cosignatures.Add(cosig);
            transaction.Size += 8 + 32 + 64;

            return transaction;
        }
    }
}
