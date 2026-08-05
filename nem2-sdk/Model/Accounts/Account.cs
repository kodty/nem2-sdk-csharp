using Org.BouncyCastle.Crypto.Digests;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Infrastructure.Interfaces;
using Org.BouncyCastle.Security;

namespace io.nem2.sdk.Model.Accounts
{
    public class Account
    {
        public Address Address { get; }

        public SecretKeyPair KeyPair { get; }

        public PublicAccount PublicAccount { get; set; }

        public static Account CreateFromPrivateKey(string privateKey, NetworkType.Types networkType)
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(privateKey);
            var address = Address.CreateFromPublicKey(keyPair.PublicKeyString, networkType);
            
            return new Account(address, keyPair);
        }

        public Account(Address address, SecretKeyPair keyPair)
        {
            Address = address;
            KeyPair = keyPair;
            PublicAccount = new PublicAccount(keyPair.PublicKeyString, address.NetworkByte);
        }

        public Account(string privateKey, NetworkType.Types networkType)
        {
            KeyPair = SecretKeyPair.CreateFromPrivateKey(privateKey);
            Address = Address.CreateFromPublicKey(KeyPair.PublicKeyString, networkType);
            PublicAccount = new PublicAccount(KeyPair.PublicKeyString, networkType);
        }

        public CosignatureSignedTransaction SignCosignatureTransaction(CosignatureTransaction1 cosignatureTransaction)
        {
            return cosignatureTransaction.SignWith(KeyPair);
        }

        //public SignedTransaction SignTransactionWithCosignatories(AggregateTransaction transaction, List<Account> cosignatories, string networkGenHash)
        //{
        //    return transaction.SignWithAggregateCosigners(KeyPair, cosignatories, networkGenHash);
        //}

        public static Account GenerateNewAccount(NetworkType.Types networkType)
        {
            var keyPair = SecretKeyPair.GenerateNewKeyPair();

            return new Account(Address.CreateFromPublicKey(keyPair.PublicKeyString, networkType), keyPair);                 
        }
    }
}
