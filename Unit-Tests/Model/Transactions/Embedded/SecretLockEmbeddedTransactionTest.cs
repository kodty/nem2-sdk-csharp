using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class SecretLockEmbeddedTransactionTest
    {
        public async Task CreateSecretLockEmbeddedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }
    }
}
