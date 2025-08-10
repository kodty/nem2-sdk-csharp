using CopperCurve;
using Org.BouncyCastle.Cms;

namespace io.nem2.sdk.src.Model.Transactions.CrossChainTransactions
{
    public class SecretProofTransaction : Transaction
    {
        public SecretProofTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) {}

        public SecretProofTransaction(string recipient, string secret, HashType.Types hashAlgo, string proof, bool embedded) : base(embedded)
        {    
            Secret = secret.FromHex();
            HashAlgo = hashAlgo.GetHashTypeValue();
            Proof = proof.FromHex();
            ProofSize = (uint)Proof.Length;
            Recipient = recipient.IsBase32()
                      ? AddressEncoder.DecodeAddress(recipient)
                      : recipient.FromHex();
        }

        public byte[] Recipient { get; set; }

        public byte[] Secret { get; set; }

        public uint ProofSize { get; set; }

        public byte HashAlgo { get; set; }

        public byte[] Proof { get; set; }
    }
}
