using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Utils;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class NamespaceRegistrationTransactionTest
    {
        public async Task CreateRootNamespace()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }

        public async Task CreateChildNamespace()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }
    }
}
