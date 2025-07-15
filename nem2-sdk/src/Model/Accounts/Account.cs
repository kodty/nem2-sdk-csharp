using CopperCurve;
using System.Security.Cryptography;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using Org.BouncyCastle.Crypto.Digests;
using io.nem2.sdk.src.Model2;

namespace io.nem2.sdk.Model.Accounts
{
    /// <summary>
    /// Class Account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public Address Address { get; }

        /// <summary>
        /// Gets or sets the key pair.
        /// </summary>
        /// <value>The key pair.</value>
        public SecretKeyPair KeyPair { get; }

        /// <summary>
        /// Gets the private key.
        /// </summary>
        /// <value>The private key.</value>
        public string PrivateKey => KeyPair.PrivateKeyString;

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey => KeyPair.PublicKeyString;

        /// <summary>
        /// Gets or sets the public account.
        /// </summary>
        /// <value>The public account.</value>
        public PublicAccount PublicAccount { get; set; }

        /// <summary>
        /// Creates from private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="networkType">Type of the network.</param>
        /// <returns>Account.</returns>
        public static Account CreateFromPrivateKey(string privateKey, NetworkType.Types networkType)
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(privateKey);
            var address = Address.CreateFromPublicKey(keyPair.PublicKeyString, networkType);
            
            return new Account(address, keyPair);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account" /> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="keyPair">The key pair.</param>
        public Account(Address address, SecretKeyPair keyPair)
        {
            Address = address;
            KeyPair = keyPair;
            PublicAccount = new PublicAccount(keyPair.PublicKeyString, address.NetworkByte);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account" /> class.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="networkType">Type of the network.</param>
        public Account(string privateKey, NetworkType.Types networkType)
        {
            KeyPair = SecretKeyPair.CreateFromPrivateKey(privateKey);
            Address = Address.CreateFromPublicKey(KeyPair.PublicKeyString, networkType);
            PublicAccount = new PublicAccount(KeyPair.PublicKeyString, networkType);
        }

        /// <summary>
        /// Signs the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>SignedTransaction.</returns>
        public SignedTransaction Sign(Transaction transaction, string networkGenHash)
        {
            return transaction.SignWith(KeyPair, networkGenHash.FromHex());
        }

        /// <summary>
        /// Signs the cosignature transaction.
        /// </summary>
        /// <param name="cosignatureTransaction">The cosignature transaction.</param>
        /// <returns>CosignatureSignedTransaction.</returns>
        public CosignatureSignedTransaction SignCosignatureTransaction(CosignatureTransaction cosignatureTransaction)
        {
            return cosignatureTransaction.SignWith(KeyPair);
        }

        /// <summary>
        /// Signs the transaction with cosignatories.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="cosignatories">The cosignatories.</param>
        /// <returns>SignedTransaction.</returns>
        public SignedTransaction SignTransactionWithCosignatories(AggregateTransaction transaction, List<Account> cosignatories, string networkGenHash)
        {
            return transaction.SignWithAggregateCosigners(KeyPair, cosignatories, networkGenHash);
        }

        public static Account GenerateNewAccount(NetworkType.Types networkType)
        {
            using (var ng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[2048];
                ng.GetNonZeroBytes(bytes);

                var digestSha3 = new Sha3Digest(256);
                var stepOne = new byte[32];
                digestSha3.BlockUpdate(bytes, 0, 32);
                digestSha3.DoFinal(stepOne, 0);

                var keyPair = SecretKeyPair.CreateFromPrivateKey(stepOne.ToHex());

                return new Account(Address.CreateFromPublicKey(keyPair.PublicKeyString, networkType), keyPair);
            }         
        }
    }
}
