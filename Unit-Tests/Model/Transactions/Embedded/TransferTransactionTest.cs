using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;


namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class TransferTransactionTest
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

            var supplyChangePayload = supplyChange.SignEmbeddedTransaction(keys);

            Assert.That(supplyChangePayload.Payload.ToHex(), Is.EqualTo("410000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984D428969746E9B1A70570A0000000000000001"));
        }

        [Test, Timeout(20000)]
        public async Task CreateEmbeddedTransferAndSupplyChangeTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var factory = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var transfer = factory.CreateTransferTransaction(
                    Address.CreateFromEncoded("TBA6LOHEA6A465G2X5MSQF66JBYR254GJDPK7MQ"),
                    EmptyMessage.Create(),
                    Mosaic.CreateFromHexIdentifier("000056CE00002B67", 101),
                    1000000,
                    true
                );

            Assert.That(transfer.SignEmbeddedTransaction(keys).Payload.ToHex(), Is.EqualTo("600000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019854419841E5B8E40781CF74DABF592817DE48711D778648DEAFB20000010000000000672B0000CE5600006500000000000000"));

            var supplyChange = factory.CreateMosaicSupplyChangeTransaction(
                    10,
                    DataConverter.ConvertFrom(6300565133566699913).ToHex(),
                    MosaicSupplyType.Type.INCREASE,
                    true
                );

            var aggTx = factory.CreateAggregateBonded(
                [
                    transfer.SignEmbeddedTransaction(keys), 
                    supplyChange.SignEmbeddedTransaction(keys)
                ],
                 Account.CreateFromPrivateKey(HttpSetUp.TestSK, NetworkType.Types.TEST_NET).KeyPair.PublicKey,
                new byte[] { });

            var aggPayload = aggTx.SignTransaction(keys, HttpSetUp.genHash);

            Assert.True(aggPayload.Payload.ToHex().Contains(transfer.SignEmbeddedTransaction(keys).Payload.Concat(supplyChange.SignEmbeddedTransaction(keys).Payload).ToArray().ToHex()));
        }
    }
}
