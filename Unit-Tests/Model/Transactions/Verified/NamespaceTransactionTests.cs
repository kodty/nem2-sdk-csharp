using Integration_Tests.HttpRequestTests;
using io.nem2.sdk.Model;

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
