using io.nem2.sdk.Infrastructure;
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

        public VerifiableTransaction(byte version, byte network, ushort type, ulong fee, Deadline deadline) : base(version, network, type)
        {
            Size += 80;
            Fee = fee;
            Deadline = deadline.Ticks;
            Signature = new byte[64];
        }

        public byte[] Signature { get; set; }

        public ulong Fee { get; set; }

        public ulong Deadline { get; set; }

        public byte[] VerifiedPayload { get; set; }

        public abstract bool IsAggregate();

        public override SignedTransaction Prepare()
        {
            Serialize();

            return new SignedTransaction()
            {
                Payload = Payload,
                PayloadSigned = VerifiablePayload,
                Signature = Signature.ToHex(),
                Signer = Signer.ToHex(),
                Hash = Hash,
                IsAggregate = IsAggregate()
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

        protected override void Serialize()
        {
            Payload = new byte[Size];

            lock (this)
            {
                DataSerializer serializer = new DataSerializer(Payload);

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

                VerifiablePayload = Payload.Take(new Range(108, Payload.Length)).ToArray();
            }
        }
    }
}
