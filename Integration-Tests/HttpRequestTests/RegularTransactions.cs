using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model.Network;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    internal class RegularTransactions
    {

        [SetUp]
        public void Setup()
        {
        }


        [Test, Timeout(20000)]
        public async Task SearchTransactions()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";
            PublicAccount acc = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.TRANSFER.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.HASH_LOCK.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {
                ((SimpleTransfer)i.Transaction).Mosaics
                    .ForEach(m =>
                    {
                        Assert.That(m.Id, Is.EqualTo("E74B99BA41F4AFEE"));
                        Assert.That(m.Amount, Is.GreaterThan(0));

                    });

                Assert.That(i.Meta.Height, Is.GreaterThan(0));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
            });

            Assert.That(acc.Address.Plain, Is.EqualTo("NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA"));
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicSupplyChangeTransaction()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_SUPPLY_CHANGE.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                var tx = (MosaicSupplyChange)i.Transaction;

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Action, Is.EqualTo(1));
                Assert.That(tx.MosaicId.Length, Is.EqualTo(16));
                Assert.That(tx.Delta, Is.EqualTo(7842928625000000));
                Assert.That(tx.Signature, Is.EqualTo("6FC30E98378ADBA9F79D5CEF2ECBCB6D3AD6010FC265708E62419862534D51E3F56B688B55B01AE631281CC589FB1FEFF43D88141B13AD5C9C63A5E15D0E320A"));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(i.Meta.Index, Is.EqualTo(4));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicSupplyRevocationTransaction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_SUPPLY_REVOCATION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            response.ForEach(i =>
            {

                var tx = (MosaicSupplyRevocation)i.Transaction;

                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(tx.SourceAddress.Length, Is.EqualTo(48));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicDefinitionTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("B48A3BCE25D31A458303489D8EC02006CB74B72F05E046E5D7428C654CDC0625");

            var tx = (MosaicDefinition)response.Transaction;

            Assert.That(tx.Nonce, Is.EqualTo(0));
            Assert.That(tx.Duration, Is.EqualTo(0));
            Assert.That(tx.Divisibility, Is.EqualTo(6));
            Assert.That(tx.Flags, Is.EqualTo(2));
            Assert.That(tx.Id, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(tx.MaxFee, Is.EqualTo(0));


        }

        [Test, Timeout(20000)]
        public async Task SearchTransferTransaction()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.TRANSFER.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                var tx = (SimpleTransfer)i.Transaction;

                Assert.That(tx.RecipientAddress.Length, Is.EqualTo(48));
                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Mosaics[0].Amount, Is.GreaterThan(0));
                Assert.That(tx.Mosaics[0].Id.Length, Is.EqualTo(16));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.TRANSFER));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicSupplyRevocationTransaction()
        {

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("9B4D7D69E671E60D7862D7AFC183896A1758FD144C0C1A93BA1BA93191F1CDFE");

            var tx = (MosaicSupplyRevocation)response.Transaction;

            Assert.That(tx.Amount, Is.EqualTo(9));
            Assert.That(tx.SourceAddress, Is.EqualTo("68EC4E549EB78CF7623F1CF4CDE5FE5BBADA55C5D504DAF9"));
            Assert.That(tx.Size, Is.EqualTo(168));
            Assert.That(tx.MosaicId, Is.EqualTo("0CEDE2DEDDB4832F"));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("A2B0D50C7DB2724FEF6037821C86E62CC6C31F57AC166A36033267DA47424304"));
        }

        [Test, Timeout(20000)]
        public async Task SearchTransferTransactionWithMessege()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("11B55558B111E21CABAE7278DE2D3CF393A2384F65AF2C62B88872312FFD0101");


            var tx = (SimpleTransfer)response.Transaction;

            Assert.That(tx.Message, Is.EqualTo("FE2A8061577301E2402E3F75637E6EFD62DBA4580EE027304459C8C6C50C0E305766F88AE75F6734F6FA6C36A1E6F5093CBB53FC3F8F4BD34B5709DC46A3DB5104685E233024B972E5543FEC16B4458F712FD0AAA00E61CE3B716811DA4E3BB3F1F6851BCD0D58D892BF213BA3F3CE72918F70AA2F78B333654AB2AF8E09F8318C2A63F5"));
            Assert.That(tx.RecipientAddress, Is.EqualTo("68BA45B6240991DA609C702A2DC3ECC1BED47FA589ED331B"));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("D32168A40E4A2DB9F1FB0D60554BFCE3142835CFFFF6D2BB104AE97F8B4829B4"));
            Assert.That(tx.Mosaics, Is.Empty);
            Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.TRANSFER));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Id.Length, Is.EqualTo(24));
        }
    }
}
