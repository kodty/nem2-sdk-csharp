using Coppery;
using Integration_Tests;
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
