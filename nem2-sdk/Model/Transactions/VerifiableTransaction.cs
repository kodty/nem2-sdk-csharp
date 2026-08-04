using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class VerifiableTransaction : Transaction
    {
        public static SimpleTransaction Create(TransactionExtension transaction, NetworkType.Types networkType, ulong fee, Deadline deadline)
        {
            return new SimpleTransaction(transaction, networkType, fee, deadline);
        }

        public static SimpleTransaction<T> Create<T>(T transaction, NetworkType.Types networkType, ulong fee, Deadline deadline) where T : TransactionExtension
        {
            return new SimpleTransaction<T>(transaction, networkType, fee, deadline);
        }

        public VerifiableTransaction()
        {
            Size += 80;

            Signature = new byte[64];
        }

        public byte[] Signature { get; set; }

        public byte[] Fee { get; set; }

        public byte[] Deadline { get; set; }

        public abstract bool IsAggregate();

        public SignedTransaction SignTransaction(SecretKeyPair keyPair, string networkGenHash) => SignTransaction(keyPair, networkGenHash.FromHex());

        protected SignedTransaction SignTransaction(SecretKeyPair signer, byte[] networkGenHash)
        {
            var tBytes = Prepare();

            byte[] signBytes = new byte[] { };

            if (IsAggregate())
            {
                signBytes = new byte[32 + 52];

                for (int i = 0; i < 32; i++)
                    signBytes[i] = networkGenHash[i];

                for (int i = 0; i < 52; i++)
                    signBytes[i + 32] = tBytes.VerifiablePayload[i];
            }
            else
            {
                signBytes = [.. networkGenHash, .. tBytes.VerifiablePayload];
            }

            this.Signature = NaclFast.SignDetached(msg: signBytes, signer.SecretKey.ToArray());

            for (int x = 0; x < 64; x++)
                tBytes.Payload[x + 8] = this.Signature[x];

            return new SignedTransaction()
            {
                Signature = this.Signature.ToHex(),
                VerifiablePayload = signBytes,
                Signer = signer.PublicKeyString,
                Payload = tBytes.Payload,
                Hash = HashTransaction(this.Signature, signer.PublicKey, signBytes).ToHex()
            };
        }

        internal override UnsignedTransaction Prepare()
        {
            byte[][] tBytes = new byte[2][];

            tBytes = this.Serialize(Size);

            return new UnsignedTransaction()
            {
                Payload = tBytes[0],
                VerifiablePayload = tBytes[1]
            };
        }

        public static byte[] HashTransaction(byte[] signature, byte[] signer, byte[] signBytes)
        {
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);
            sha3Hasher.BlockUpdate(signature, 0, signature.Length);
            sha3Hasher.BlockUpdate(signer, 0, signer.Length);
            sha3Hasher.BlockUpdate(signBytes, 0, signBytes.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }

        internal override byte[][] Serialize(uint size)
        {
            lock (this)
            {
                DataSerializer serializer = new DataSerializer(size, 108);

                serializer.SerializeProperty(Size);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(Signature);
                serializer.SerializeProperty(Signer);
                serializer.SerializeProperty(new byte[4]);
                serializer.SerializeProperty(Version);
                serializer.SerializeProperty(Network);
                serializer.SerializeProperty(Type);
                serializer.SerializeProperty(Fee);
                serializer.SerializeProperty(Deadline);

                Extend(serializer);

                return serializer.GetBytes();
            }
        }
    }
}
