using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Core.Utils;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Articles;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class NamespaceRegistrationTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateRootNamespace()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var transfer = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    1440,
                    0,
                    IdGenerator.GenerateId(0, "symbol"),
                    NamespaceTypes.Types.RootNamespace,
                    "symbol",
                    false
                );

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            Assert.That(st.Payload.ToHex(), Is.EqualTo("98000000000000009FA1CAE006AE6B24FCA6EDDB131EBD045EE4EFEF6AE7C8ACCF4693BB13704EE65FC320C2F0DBFC8647F4A9BCAA997A65DE58007FC2F5C60C8A44E4E847AE7C0E91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984E416A5D22B404000000B52E115A02000000A005000000000000A95F1F8A96159516000673796D626F6C"));

        }

        [Test, Timeout(20000)]
        public async Task CreateChildNamespace()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var transfer = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    1440,
                    IdGenerator.GenerateId(0, "symbol"),
                    IdGenerator.GenerateId(IdGenerator.GenerateId(0, "symbol"), "xym"),
                    NamespaceTypes.Types.SubNamespace,
                    "symbol",
                    false
                );

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            Assert.That(st.Payload.ToHex(), Is.EqualTo("9800000000000000CBA5C11C617F04BFD5F5290A3667FF0EAFF3576945BD2309FBFE5AFFC3281CCFFD832FFF792CC1D4DF266F22C8DCA8DA3E7231D341C962A2C69DE1FA917CC50B91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984E416A5D22B404000000B52E115A02000000A95F1F8A96159516E74B99BA41F4AFEE010673796D626F6C"));

        }
    }
}
