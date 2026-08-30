using Integration_Tests.HttpRequestTests;
using io.nem2.sdk.Model;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class KeyLinkTransactionTest
    {
        public async Task CreateEmbeddedVRFKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }

        public async Task CreateEmbeddedAccountKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }
        public async Task CreateEmbeddedNodeKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        }

        public async Task CreateEmbeddedVotingKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }
    }
}
