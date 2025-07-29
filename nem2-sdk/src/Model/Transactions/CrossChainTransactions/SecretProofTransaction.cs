using CopperCurve;

namespace io.nem2.sdk.src.Model.Transactions.CrossChainTransactions
{
    public class SecretProofTransaction1 : Transaction
    {
        public SecretProofTransaction1(string recipientAddress, string secret, HashType.Types hashAlgo, string proof)
        {
            RecipientAddress = AddressEncoder.DecodeAddress(recipientAddress);
            Secret = secret.FromHex();
            HashAlgo = hashAlgo;
            Proof = proof.FromHex();
            ProofSize = (uint)Proof.Length;
        }

        public byte[] RecipientAddress { get; internal set; }

        public byte[] Secret { get; internal set; }

        public uint ProofSize { get; internal set; }

        public HashType.Types HashAlgo { get; internal set; }

        public byte[] Proof { get; internal set; }
    }
}
