using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using System.Diagnostics;


namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class KeyLinkTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateEmbeddedVRFKeyLinkTransactionTest()
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

        [Test, Timeout(20000)]
        public async Task CreateEmbeddedAccountKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.ACCOUNT_KEY_LINK,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    1,
                    true
                );

            var result = accountRestriction.Embed(keys.PublicKeyString);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("510000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984C4198AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D01"));
        }

        [Test, Timeout(20000)]
        public async Task CreateEmbeddedNodeKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.NODE_KEY_LINK,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    1,
                    true
                );

            var result = accountRestriction.Embed(keys.PublicKeyString);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("510000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984C4298AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D01"));
        }

        [Test, Timeout(20000)]
        public async Task CreateEmbeddedVotingKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateVotingKeyLinkTransaction(
                    TransactionTypes.Types.VOTING_KEY_LINK,
                    01,
                    10,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    1,
                    true
                );

            var result = accountRestriction.Embed(keys.PublicKeyString);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("610000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B452000000000198434198AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D01000000000000000A0000000000000001"));
        }
    }
}
