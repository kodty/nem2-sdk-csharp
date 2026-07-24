using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class KeyLinkTransactionTest
    {
        public async Task CreateVRFKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }

        public async Task CreateNodeKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }

        public async Task CreateAccountKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }

        public async Task CreateVotingKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

        }
    }
}
