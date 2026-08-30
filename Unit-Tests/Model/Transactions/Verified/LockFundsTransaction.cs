
using Integration_Tests.HttpRequestTests;
using io.nem2.sdk.Model;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class VerifiedLockFundsTransaction
    {
        public async Task CreateLockFundsVerifiedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        }
    }
}
