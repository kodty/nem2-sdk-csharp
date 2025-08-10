using CopperCurve;
using Integration_Tests;
using io.nem2.sdk.src.Core.Utils;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Articles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class NamespaceTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateEmbeddedNamespaceRegistrationTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var factory = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var nsTx = factory.CreateNamespaceRegistrationTransaction(
                1440, 0, IdGenerator.GenerateId(0, "symbol"), NamespaceTypes.Types.RootNamespace, "symbol", true);

            var result = nsTx.Embed(keys.PublicKeyString);

            Debug.WriteLine(result.Payload.ToHex());

            Assert.That(result.Payload.ToHex(), Is.EqualTo("480000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984E41A005000000000000A95F1F8A96159516000673796D626F6C"));
        }
    }
}
