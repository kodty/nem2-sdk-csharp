using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class KeyLinkTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateVRFKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.VRF_KEY_LINK,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    1,
                    false
                );

            var result = accountRestriction.WrapVerified(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("A1000000000000008185574EFAD24FF13680E7C64791BF32A12F7225E3A26402F692A78C07FD3BAB2DB22992DB11EB908F255844DB139292E96CDE36FE4C571ED4DDAD123B8EC70D91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019843426A5D22B404000000B52E115A0200000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D01"));
        }

        [Test, Timeout(20000)]
        public async Task CreateNodeKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.NODE_KEY_LINK,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    1,
                    false
                );

            var result = accountRestriction.WrapVerified(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("A10000000000000047CBEE4E658606F7E8840AAAC37EC02E6673C1D0660BFB2AE949EABED8C42866C702F24108A0D4C5E450190417CB390E78FBDF1E78BEAC8837136FD3A8DD420191D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984C426A5D22B404000000B52E115A0200000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D01"));
        }

        [Test, Timeout(20000)]
        public async Task CreateAccountKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateKeyLinkTransaction(
                    TransactionTypes.Types.ACCOUNT_KEY_LINK,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    1,
                    false
                );

            var result = accountRestriction.WrapVerified(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("A100000000000000166E8B7A46274EE9F4C65C6C3375E9A2783EFB3AD17140ADE8A2B10FFF7D6B2B07092C8628940C72C72B5E799D6E915E46FD96326545B1550D4CF234A62A0C0191D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984C416A5D22B404000000B52E115A0200000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D01"));
        }

        [Test, Timeout(20000)]
        public async Task CreateVotingKeyLinkTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateVotingKeyLinkTransaction(
                    TransactionTypes.Types.VOTING_KEY_LINK,
                    01,
                    10,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    1,
                    false
                );

            var result = accountRestriction.WrapVerified(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("B100000000000000A756A3475FA1BC48822171973A14FFCF8CB6382F31DCC761ACCA07EF6CA614E294A6BBE28341908B575A71FA0456ACBE212DE7C3641366E0E24454C198B0320691D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019843416A5D22B404000000B52E115A0200000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D01000000000000000A0000000000000001"));
        }
    }
}
