using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class SecretLockEmbeddedTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateSecretLockEmbeddedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var lockFunds = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretLockTransaction(
                    "72C0212E67A08BCE",
                    1440,
                    "67A08BCE",
                    HashType.Types.SHA3_512,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    true
                );

            var result = lockFunds.Embed(keys.PublicKeyString);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("660000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B452000000000198524172C0212E67A08BCEA00500000000000067A08BCE0098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D00"));
        }
    }
}
