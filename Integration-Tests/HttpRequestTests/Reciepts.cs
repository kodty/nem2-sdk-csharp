using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    internal class Reciepts
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task AddressStatements()
        {
            var client = new BlockReceiptsHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.GetRecieptsAddressResolutionStatements);

            var response = await client.GetAddressStatements(qModel);

            Assert.That(response[0].Meta.Timestamp, Is.EqualTo(112302496));
            Assert.That(response[0].Statement.Height, Is.EqualTo(923));
            Assert.IsTrue(response[0].Statement.Unresolved.IsHex(48));
            Assert.That(response[0].Statement.ResolutionEntries[0].Source.PrimaryId, Is.EqualTo(6));
            Assert.That(response[0].Statement.ResolutionEntries[0].Source.SecondaryId, Is.EqualTo(0));
            Assert.That(response[0].Statement.ResolutionEntries[0].Resolved.IsHex(48));
        }

        [Test]
        public async Task MosaicStatements()
        {
            var client = new BlockReceiptsHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.GetRecieptsAddressResolutionStatements);

            var response = await client.GetMosaicStatements(qModel);

            Assert.That(response[2].Meta.Timestamp, Is.EqualTo(118726452));
            Assert.That(response[2].Statement.Height, Is.EqualTo(1142));
            Assert.That(response[2].Statement.Unresolved.IsHex(16));
            Assert.That(response[2].Statement.ResolutionEntries[0].Resolved.IsHex(16));
            Assert.That(response[2].Statement.ResolutionEntries[0].Source.PrimaryId, Is.EqualTo(6));
            Assert.That(response[2].Statement.ResolutionEntries[0].Source.SecondaryId, Is.EqualTo(0));
        }

        [Test]
        public async Task TransactionStatements()
        {
            var client = new BlockReceiptsHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchTransactionStatements);

            var response = await client.SearchTransactionStatements(qModel);

            Assert.That(response[2].Meta.Timestamp, Is.EqualTo(0));
            Assert.That(response[2].Statement.Height, Is.EqualTo(1));
            Assert.IsTrue(response[2].Statement.Receipts[0].MosaicId.IsHex(16));
            Assert.That(response[2].Statement.Receipts[0].Type, Is.EqualTo(4942)); // flag
            // https://docs.symbol.dev/concepts/receipt.html#recorded-receipts

            Assert.That(response[2].Statement.Receipts[0].Version, Is.EqualTo(1));
            Assert.That(response[2].Statement.Receipts[0].Amount, Is.EqualTo(0));
            Assert.That(response[2].Statement.Receipts[0].SenderAddress.Length, Is.EqualTo(48));
            Assert.IsTrue(response[2].Statement.Receipts[0].RecipientAddress.IsHex(48));
            Assert.That(response[2].Statement.Source.PrimaryId, Is.EqualTo(25592));
            Assert.That(response[2].Statement.Source.SecondaryId, Is.EqualTo(0));
        }
    }
}
