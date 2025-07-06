using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;

namespace io.nem2.sdk.Model.Transactions
{
    public class SecretProofTransaction : Transaction
    {
        internal byte[] Proof { get; }

        public string ProofString() => Proof.ToHex();

        internal byte[] Secret { get; }

        public string SecretString() => Secret.ToHex();

        public HashType.Types HashAlgo { get; }

        public SecretProofTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, HashType.Types hashAlgo, string secret, string proof)
        : this(networkType, 3,deadline, fee, hashAlgo, secret, proof, null, null) {}

        public SecretProofTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, HashType.Types hashAlgo, string secret, string proof, string signature, PublicAccount signer)
        {
            Deadline = deadline;
            Version = version;
            Fee = fee;
            NetworkType = networkType;
            HashAlgo = hashAlgo;
            Secret = secret.FromHex();
            Proof = proof.FromHex();
            TransactionType = TransactionTypes.Types.SECRET_PROOF;
            Signer = signer;
            Signature = signature;
            //TransactionInfo = transactionInfo;
        }

        public static SecretProofTransaction Create(NetworkType.Types netowrkType, Deadline deadline, ulong fee, HashType.Types hashAlgo, string secret, string proof)
        {
            return new SecretProofTransaction(netowrkType, 3, deadline, fee, hashAlgo, secret, proof);
        }

        internal override byte[] GenerateBytes()
        {
            ushort size = (ushort)(187 + Proof.Length);

            var serializer = new DataSerializer();

            serializer.WriteUlong(size);

            serializer.Reserve(64);
            serializer.WriteBytes(GetSigner());
            serializer.Reserve(4);
            serializer.WriteByte((byte)Version);
            serializer.WriteByte(NetworkType.GetNetworkByte());
            serializer.WriteUShort(TransactionType.GetValue());
            serializer.WriteUlong(Fee);
            serializer.WriteUlong(Deadline.Ticks);
            serializer.WriteByte(HashAlgo.GetHashTypeValue());
            serializer.WriteBytes(Secret);
            serializer.WriteUShort((ushort)Proof.Length);
            serializer.WriteBytes(Proof);

            return serializer.Bytes;
        }
    }
}
