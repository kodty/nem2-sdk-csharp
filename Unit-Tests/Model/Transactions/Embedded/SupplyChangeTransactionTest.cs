using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Articles;


namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class SupplyChangeTransactionTest
    {
        [Test, Timeout(20000)]
        public async Task CreateSupplyChangeEmbeddedTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var factory = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var supplyChange = factory.CreateMosaicSupplyChangeTransaction(
                    10,
                    DataConverter.ConvertFrom(6300565133566699913).ToHex(),
                    MosaicSupplyType.Type.INCREASE,
                    true
                );

            var supplyChangePayload = supplyChange.Embed(keys.PublicKeyString);

            Assert.That(supplyChangePayload.Payload.ToHex(), Is.EqualTo("410000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984D428969746E9B1A70570A0000000000000001"));
        }
    }
}
