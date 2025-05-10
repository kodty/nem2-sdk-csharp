using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model.Transactions
{
    public class SecretLockTransaction : Transaction
    {
        public Mosaic1 Mosaic { get; }

        public ulong Duration { get; }

        internal byte[] Secret { get; }

        public string SecretString() => Secret.ToHexUpper();

        public HashType.Types HashAlgo { get; }

        public Address Recipient { get; }

        public static SecretLockTransaction Create(NetworkType.Types netowrkType, int version, Deadline deadline, ulong fee, Mosaic1 mosaic, ulong duration, HashType.Types hashAlgo, string secret, Address recipient)
        {
            return new SecretLockTransaction(netowrkType, version, deadline,fee, mosaic, duration, hashAlgo, secret, recipient, null, null, null);
        }

        public SecretLockTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Mosaic1 mosaic, ulong duration, HashType.Types hashAlgo, string secret, Address recipient)
            : this(networkType, version, deadline, fee, mosaic, duration, hashAlgo, secret, recipient, null, null, null) {}

        public SecretLockTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Mosaic1 mosaic, ulong duration, HashType.Types hashAlgo, string secret, Address recipient, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            if (hashAlgo.GetHashTypeValue() == 0 && secret.Length != 128) throw new ArgumentException("invalid secret");

            Deadline = deadline;
            Version = version;
            Fee = fee;
            Duration = duration;
            Mosaic = mosaic;
            NetworkType = networkType;
            HashAlgo = hashAlgo;
            Secret = secret.FromHex();
            Recipient = recipient;
            TransactionType = TransactionTypes.Types.SECRET_LOCK;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
        }

        internal override byte[] GenerateBytes()
        {
            ushort size = 234;

            var serializer = new DataSerializer(size);

            serializer.WriteUlong(size);

            serializer.Reserve(64);
            serializer.WriteBytes(GetSigner());
            serializer.Reserve(4);
            serializer.WriteByte((byte)Version);
            serializer.WriteByte(NetworkType.GetNetworkByte());
            serializer.WriteUShort(TransactionType.GetValue());
            serializer.WriteUlong(Fee);
            serializer.WriteUlong(Deadline.Ticks);
            serializer.WriteUlong(Mosaic.MosaicId.Id);
            serializer.WriteUlong(Mosaic.Amount);
            serializer.WriteUlong(Duration);
            serializer.WriteByte(HashAlgo.GetHashTypeValue());
            serializer.WriteBytes(Secret);
           
            serializer.WriteBytes(AddressEncoder.DecodeAddress(Recipient.Plain));

            return serializer.Bytes;
        }
    }
}
