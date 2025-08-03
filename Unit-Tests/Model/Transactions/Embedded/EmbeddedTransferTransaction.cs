using CopperCurve;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Articles;
using System.Diagnostics;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class EmbeddedTransferTransaction
    {

        [Test, Timeout(20000)]
        public async Task CreateSupplyChangeEmbeddedTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var factory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var supplyChange = factory.CreateMosaicSupplyChangeTransaction(
                    10,
                    DataConverter.ConvertFromUInt64(6300565133566699913).ToHex(),
                    MosaicSupplyType.Type.INCREASE,
                    true
                );

            var supplyChangePayload = supplyChange.Embed(publicAcc);

            Assert.That(supplyChangePayload.Payload.ToHex(), Is.EqualTo("410000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984D428969746E9B1A70570A0000000000000001"));
        }

        [Test, Timeout(20000)]
        public async Task CreateEmbeddedTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var factory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var transfer = factory.CreateTransferTransaction(
                    "TBA6LOHEA6A465G2X5MSQF66JBYR254GJDPK7MQ",
                    "",
                    new Tuple<string, ulong>("672B0000CE560000", 101),
                    true
                );

            var supplyChange = factory.CreateMosaicSupplyChangeTransaction(
                    10,
                    DataConverter.ConvertFromUInt64(6300565133566699913).ToHex(),
                    MosaicSupplyType.Type.INCREASE,
                    true
                );

            var aggTx = factory.CreateAggregateBonded(
                "61E0F8B9AB2FE3E008DCE1380FECDAF5BCFB1851247BF990771154177A0B7E78", 
                [
                    transfer.Embed(publicAcc), 
                    supplyChange.Embed(publicAcc)
                ], 
                new byte[] { }, 
                false);

            var aggPayload = aggTx.WrapVerified(keys);

            Assert.True(aggPayload.Payload.ToHex().Contains(transfer.Embed(publicAcc).Payload.Concat(supplyChange.Embed(publicAcc).Payload).ToArray().ToHex()));
           //Assert.That(aggPayload.Payload.ToHex(), Is.EqualTo(""));
        }
    }
}
