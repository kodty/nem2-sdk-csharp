using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class LockFundsTransaction
    {
        [Test, Timeout(20000)]
        public async Task CreateLockFundsEmbeddedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var lockFunds = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateLockFundsTransaction(
                    "72C0212E67A08BCE",
                    10000000,
                    1440,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    true
                );

            var result = lockFunds.SignEmbeddedTransaction(keys);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("680000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984841CE8BA0672E21C0728096980000000000A00500000000000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D"));
        }
    }
}
