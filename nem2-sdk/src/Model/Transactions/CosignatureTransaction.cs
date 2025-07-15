using System.Text.RegularExpressions;
using CopperCurve;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model2;
using io.nem2.sdk.src.Model2.Transactions;


namespace io.nem2.sdk.Model.Transactions
{
    public class CosignatureTransaction
    {
        public string Hash { get; }

        public CosignatureTransaction(string hash)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if (!Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("invalid hash length");
            if (hash.Length != 64) throw new ArgumentException("invalid hash, the given String is not in hex format");

            Hash = hash;
        }

        public static CosignatureTransaction Create(string hash)
        {
            return new CosignatureTransaction(hash);
        }
        public CosignatureSignedTransaction SignWith(SecretKeyPair account)
        {
            var signatureBytes = account.Sign(Hash.FromHex());

            return new CosignatureSignedTransaction{ ParentHash = Hash, Signature = signatureBytes.ToHex(), Signer = account.PublicKeyString};
        }      
    }
}
