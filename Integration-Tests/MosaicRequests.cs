using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
{
    internal class MosaicRequests
    {

        [SetUp]
        public void Setup()
        {
        }


        [Test, Timeout(20000)]
        public async Task SearchMosaics()
        {
            var client = new MosaicHttp(HttpSetUp.Node, HttpSetUp.Port);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchMosaics);
            queryModel.SetParam(QueryModel.DefinedParams.ownerAddress, Address.CreateFromHex("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8").Plain);

            var response = await client.SearchMosaics(queryModel);

            Assert.That(response[0].Mosaic.Id, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response[0].Mosaic.Version, Is.EqualTo(1));
            Assert.That(response[0].Mosaic.Supply, Is.GreaterThan(8427457774427808));
            Assert.That(response[0].Mosaic.StartHeight, Is.EqualTo(1));
            Assert.That(response[0].Mosaic.OwnerAddress, Is.EqualTo("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8"));
            Assert.That(response[0].Mosaic.Revision, Is.EqualTo(1));
            Assert.That(response[0].Mosaic.Flags, Is.EqualTo(2));
            Assert.That(response[0].Mosaic.Divisibility, Is.EqualTo(6));
            Assert.That(response[0].Mosaic.Duration, Is.EqualTo(0));
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaic()
        {
            var client = new MosaicHttp(HttpSetUp.Node, HttpSetUp.Port);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchMosaics);
            queryModel.SetParam(QueryModel.DefinedParams.ownerAddress, Address.CreateFromHex("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8").Plain);

            var response = await client.GetMosaic("6BED913FA20223F8");

            Assert.That(response.Mosaic.Id, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response.Mosaic.Version, Is.EqualTo(1));
            Assert.That(response.Mosaic.Supply, Is.GreaterThan(0));
            Assert.That(response.Mosaic.StartHeight, Is.EqualTo(1));
            Assert.That(response.Mosaic.OwnerAddress, Is.EqualTo("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8"));
            Assert.That(response.Mosaic.Revision, Is.EqualTo(1));
            Assert.That(response.Mosaic.Flags, Is.EqualTo(2));
            Assert.That(response.Mosaic.Divisibility, Is.EqualTo(6));
            Assert.That(response.Mosaic.Duration, Is.EqualTo(0));
        }

        [Test, Timeout(20000)]
        public async Task GetMosaics()
        {
            var client = new MosaicHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMosaics(new List<string> { "63078E73FBCC2CAC", "6BED913FA20223F8" });

            Assert.That(response[0].Mosaic.Id, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(response[0].Mosaic.Version, Is.EqualTo(1));
            Assert.That(response[0].Mosaic.Supply, Is.EqualTo(3800000));
            Assert.That(response[0].Mosaic.StartHeight, Is.EqualTo(117));
            Assert.That(response[0].Mosaic.OwnerAddress, Is.EqualTo("6854F763D03307D0281EFC4BB3B4926316F9AEE74EB63EE8"));
            Assert.That(response[0].Mosaic.Revision, Is.EqualTo(1));
            Assert.That(response[0].Mosaic.Flags, Is.EqualTo(3));
            Assert.That(response[0].Mosaic.Divisibility, Is.EqualTo(0));
            Assert.That(response[0].Mosaic.Duration, Is.EqualTo(0));
            Assert.That(response[1].Mosaic.Id, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response[1].Mosaic.Version, Is.EqualTo(1));
            Assert.That(response[1].Mosaic.Supply, Is.GreaterThan(0));
            Assert.That(response[1].Mosaic.StartHeight, Is.EqualTo(1));
            Assert.That(response[1].Mosaic.OwnerAddress, Is.EqualTo("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8"));
            Assert.That(response[1].Mosaic.Revision, Is.EqualTo(1));
            Assert.That(response[1].Mosaic.Flags, Is.EqualTo(2));
            Assert.That(response[1].Mosaic.Divisibility, Is.EqualTo(6));
            Assert.That(response[1].Mosaic.Duration, Is.EqualTo(0));
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicMerkle()
        {
            var client = new MosaicHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMosaicMerkle( "6BED913FA20223F8" );

           Assert.That(response.Tree[1].Type, Is.EqualTo(0));
            Assert.That(response.Tree[1].NibbleCount, Is.EqualTo(0));
            Assert.That(response.Tree[1].Links[0].Link, Is.EqualTo("C3ED57BC62D90EDEB02F390061ED622B43F3ACB2A60C193B91466A6A002F8675"));
            Assert.That(response.Tree[0].BranchHash.Length, Is.EqualTo(64));
            Assert.That(response.Tree[1].Value, Is.Null);
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicDefinitionTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059");

            var tx = ((Aggregate)response.Transaction);
            var embedded = tx.Transactions;
            var definition = (EmbeddedMosaicDefinition)embedded[0].Transaction;

            Assert.That(tx.TransactionsHash, Is.EqualTo("DF5C0B4D7CC979FA385D4785FCA2A5D9A8F172C0BAB90883BF167DFE9C78A13B"));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));
            Assert.That(embedded.Count, Is.EqualTo(2));
            Assert.That(definition.Id, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(definition.Flags, Is.EqualTo(3));
            Assert.That(definition.Duration, Is.EqualTo(0));
            Assert.That(definition.Divisibility, Is.EqualTo(0));
            Assert.That(definition.Nonce, Is.EqualTo(3525458556));
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicSupplyChangeTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("7E3049EBF37DD84C2C52C96A4234281326F3FA434DCFBDA71CF68A194ACB5059");

            var tx = ((Aggregate)response.Transaction);
            var embedded = tx.Transactions;
            var definition = (EmbeddedMosaicSupplyChange)embedded[1].Transaction;

            Assert.That(tx.TransactionsHash, Is.EqualTo("DF5C0B4D7CC979FA385D4785FCA2A5D9A8F172C0BAB90883BF167DFE9C78A13B"));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("7E43EC810A64FCCA5F9FBF6FC3E51AA89A0507762DC7E6B8047DCACBE97A8D4B"));
            Assert.That(embedded.Count, Is.EqualTo(2));
            Assert.That(definition.MosaicId, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(definition.Delta, Is.EqualTo(3800000));
            Assert.That(definition.Action, Is.EqualTo(1));
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

            response.ForEach(i => {

                var tx = ((MosaicSupplyChange)i.Transaction);

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Action, Is.EqualTo(1));
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

            response.ForEach(i => {

                var tx = ((MosaicSupplyRevocation)i.Transaction);

                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(tx.SourceAddress.Length, Is.EqualTo(48));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
            });
        }
    }
}
