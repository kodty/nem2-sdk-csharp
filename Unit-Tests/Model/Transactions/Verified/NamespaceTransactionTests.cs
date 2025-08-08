using CopperCurve;
using Integration_Tests;
using io.nem2.sdk.src.Core.Utils;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Articles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class NamespaceRegistrationTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateRootNamespace()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    1440,
                    0,
                    IdGenerator.GenerateId(0, "symbol"),
                    NamespaceTypes.Types.RootNamespace,
                    "symbol",
                    false
                );

            var st = transfer.WrapVerified(keys);

            Debug.WriteLine(st.Payload.ToHex());

            Assert.That(st.Payload.ToHex(), Is.EqualTo("A0000000000000003F9A69048F8335331AD4278E32DEC4DC4FE0997A74CC32C48109508B96AAE4848F44E179197795DFAFAC9DE1309E93C0FED476E2CE8FCB78546485051EE1F00E91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984E4164000000000000003B73DB5E14000000A005000000000000A95F1F8A96159516000673796D626F6C"));

        }

        [Test, Timeout(20000)]
        public async Task CreateChildNamespace()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    1440,
                    IdGenerator.GenerateId(0, "symbol"),
                    IdGenerator.GenerateId(IdGenerator.GenerateId(0, "symbol"), "xym"),
                    NamespaceTypes.Types.SubNamespace,
                    "symbol",
                    false
                );

            var st = transfer.WrapVerified(keys);

            Debug.WriteLine(st.Payload.ToHex());

            Assert.That(st.Payload.ToHex(), Is.EqualTo(""));

        }
    }
}
