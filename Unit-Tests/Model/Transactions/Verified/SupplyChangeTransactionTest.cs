using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Articles;
using System.Diagnostics;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class SupplyChangeTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateSupplyChangeTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var factory = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var supplyChange = factory.CreateMosaicSupplyChangeTransaction(
                    10,
                    DataConverter.ConvertFrom(6300565133566699913).ToHex(),
                    MosaicSupplyType.Type.INCREASE,
                    false
                );

            var result = supplyChange.WrapVerified(keys);

            Debug.WriteLine(result.Payload.ToHex());

            Assert.That(result.Payload.ToHex(), Is.EqualTo("910000000000000032E0497B02E6D4D7B7F9ED10CA5E2F615E0B313D75465967FCCF5DE01C6A231ECBBCC45902E45D50295BCDC2A19D0341432FAAA37C5165AED105905B3D8C3A0B91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984D426A5D22B404000000B52E115A020000008969746E9B1A70570A0000000000000001"));
        }
    }
}
