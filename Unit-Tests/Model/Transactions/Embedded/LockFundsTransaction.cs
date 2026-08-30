
using Integration_Tests.HttpRequestTests;
using io.nem2.sdk.Model;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class LockFundsTransaction
    {
        public async Task CreateLockFundsEmbeddedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }
    }
}
