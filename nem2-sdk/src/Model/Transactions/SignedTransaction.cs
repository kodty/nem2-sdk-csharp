using System.Text.RegularExpressions;

using io.nem2.sdk.src.Export;
using TweetNaclSharp;

namespace io.nem2.sdk.Model.Transactions
{
    public class SignedTransaction
    {
        public string Payload { get; set; }

        public string Hash { get; set; }

        public string Signer { get; set; }

        public string Signature { get; set; }

        private byte[] SignedBytes { get; set; }

        public TransactionTypes.Types TransactionType { get; }

        public bool VerifySignature()
        {
            return NaclFast.SignDetachedVerify(SignedBytes, Signature.FromHex(), Signer.FromHex());
        }

        internal SignedTransaction(string payload, byte[] signedBytes, string hash, string signer, string signature, TransactionTypes.Types transactionType)
        {          
            if (hash.Length != 64 || !Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("Invalid hash.");
            TransactionType = transactionType;
            Payload = payload;
            Hash = hash;
            Signer = signer;
            Signature = signature;  
            SignedBytes = signedBytes;  
        }

        public static SignedTransaction Create(byte[] payload, byte[] signedBytes, byte[] hash, byte[] signer, byte[] signature, TransactionTypes.Types transactionType)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if(hash.Length != 32) throw new ArgumentException("invalid hash length");
            if (signer == null) throw new ArgumentNullException(nameof(signer));
            if (signer.Length != 32) throw new ArgumentException("invalid signer length");

            return new SignedTransaction(payload.ToHex(), signedBytes, hash.ToHex(), signer.ToHex(), signature.ToHex(), transactionType);
        }
    }
}
