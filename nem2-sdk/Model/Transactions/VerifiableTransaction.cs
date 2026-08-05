using Coppery;
using Org.BouncyCastle.Crypto.Digests;

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

        public ulong Fee { get; set; }

        public ulong Deadline { get; set; }

        public abstract bool IsAggregate();

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

        protected override byte[][] Serialize(uint size)
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
