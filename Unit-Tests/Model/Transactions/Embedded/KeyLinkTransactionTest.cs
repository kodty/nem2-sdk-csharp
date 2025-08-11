using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;


namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class KeyLinkTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateEmbeddedKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.VRF_KEY_LINK,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    1,
                    true
                );

            var result = accountRestriction.Embed(keys.PublicKeyString);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("510000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B452000000000198434298AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D01"));
        }
    }
}
