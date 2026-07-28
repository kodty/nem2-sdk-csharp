using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class SecretProofTransaction : VerifiableTransaction
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Recipient, 10);
            serializer.SerializeProperty(Secret, 11);
            serializer.SerializeProperty(ProofSize, 12);
            serializer.SerializeProperty(HashAlgo, 13);
            serializer.SerializeProperty(Proof, 14);
        }

        public SecretProofTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) {}

        public SecretProofTransaction(string recipient, string secret, HashType.Types hashAlgo, string proof, bool embedded) : base(TransactionTypes.Types.SECRET_PROOF, embedded)
        {
            Version = 0x01;

            Size += 59;

            Secret = secret.FromHex();
            HashAlgo = hashAlgo.GetHashTypeValue();
            Proof = proof.FromHex();
            ProofSize = (ushort)Proof.Length;
            Recipient = recipient.IsBase32()
                      ? AddressEncoder.DecodeAddress(recipient)
                      : recipient.FromHex();

            Size += (uint)Proof.Length;
        }

        public byte[] Recipient { get; set; }

        public byte[] Secret { get; set; }

        public ushort ProofSize { get; set; }

        public byte HashAlgo { get; set; }

        public byte[] Proof { get; set; }

        public override SecretProofTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}
