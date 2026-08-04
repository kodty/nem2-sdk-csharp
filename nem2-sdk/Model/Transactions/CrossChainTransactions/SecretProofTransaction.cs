using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.CrossChainTransactions
{
    public class SecretProofTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(Recipient);
            serializer.SerializeProperty(Secret);
            serializer.SerializeProperty(ProofSize);
            serializer.SerializeProperty(HashAlgo);
            serializer.SerializeProperty(Proof);
        }

        public SecretProofTransaction(string recipient, string secret, HashType.Types hashAlgo, string proof)
        {
            Secret = secret.FromHex();
            HashAlgo = hashAlgo.GetHashTypeValue();
            Proof = proof.FromHex();
            ProofSize = (ushort)Proof.Length;
            Recipient = recipient.IsBase32()
                      ? AddressEncoder.DecodeAddress(recipient)
                      : recipient.FromHex();
        }

        public byte[] Recipient { get; set; }

        public byte[] Secret { get; set; }

        public ushort ProofSize { get; set; }

        public byte HashAlgo { get; set; }

        public byte[] Proof { get; set; }

        internal override int AddSize() 
        {
            return Proof.Length + 59;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.SECRET_PROOF;
        }
    }
}
