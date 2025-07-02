using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model2.Transactions;


namespace io.nem2.sdk.src.Model2.Transactions.CrossChainTransactions
{
    public class SecretProofTransaction1 : Transaction1
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
