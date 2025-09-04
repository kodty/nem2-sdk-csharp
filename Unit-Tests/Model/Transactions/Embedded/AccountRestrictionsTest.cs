using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class AccountRestrictionsTest
    {
        [Test, Timeout(20000)]
        public void CreateEmbeddedAccountRestriction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAccount = PublicAccount.CreateFromPublicKey(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateAccountRestrictionTransaction(
                    TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION,
                    0x1,
                    ["NALS4SSCJU4VNFNJFKT5VL6AWGEHQRSERK6VPLA", "SALS4SSCJU4VNFNJFKT5VL6AWGEHQRSERK6VPLA"],
                    ["TALS4SSCJU4VNFNJFKT5VL6AWGEHQRSERK6VPLA", "VALS4SSCJU4VNFNJFKT5VL6AWGEHQRSERK6VPLA"],
                    true
                );

            var st = accountRestriction.Embed(keys.PublicKeyString);

            Assert.That(st.Payload.ToHex(), Is.EqualTo("980000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001985041010002020000000068172E4A424D395695A92AA7DAAFC0B1887846448ABD57AC90172E4A424D395695A92AA7DAAFC0B1887846448ABD57AC98172E4A424D395695A92AA7DAAFC0B1887846448ABD57ACA8172E4A424D395695A92AA7DAAFC0B1887846448ABD57AC"));
        }
    }
}
