using io.nem2.sdk.src.Infrastructure.HttpRepositories;
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
            Assert.That(response[0].Statement.Unresolved, Is.EqualTo("6985738C26EB1534A4000000000000000000000000000000"));
            Assert.That(response[0].Statement.ResolutionEntries[0].Source.PrimaryId, Is.EqualTo(6));
            Assert.That(response[0].Statement.ResolutionEntries[0].Source.SecondaryId, Is.EqualTo(0));
            Assert.That(response[0].Statement.ResolutionEntries[0].Resolved, Is.EqualTo("688928C64395E16FAE78B30F970AE0249AD0B5B5B9DE1F5B"));
        }

        [Test]
        public async Task MosaicStatements()
        {
            var client = new BlockReceiptsHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.GetRecieptsAddressResolutionStatements);

            var response = await client.GetMosaicStatements(qModel);

            Assert.That(response[2].Meta.Timestamp, Is.EqualTo(118726452));
            Assert.That(response[2].Statement.Height, Is.EqualTo(1142));
            Assert.That(response[2].Statement.Unresolved, Is.EqualTo("E74B99BA41F4AFEE"));
            Assert.That(response[2].Statement.ResolutionEntries[0].Resolved, Is.EqualTo("6BED913FA20223F8"));
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
            Assert.That(response[2].Statement.Receipts[0].MosaicId, Is.EqualTo("6BED913FA20223F8"));

            Assert.That(response[2].Statement.Receipts[0].Type, Is.EqualTo(4942)); // flag
            // https://docs.symbol.dev/concepts/receipt.html#recorded-receipts

            Assert.That(response[2].Statement.Receipts[0].Version, Is.EqualTo(1));
            Assert.That(response[2].Statement.Receipts[0].Amount, Is.EqualTo(0));
            Assert.That(response[2].Statement.Receipts[0].SenderAddress, Is.EqualTo("688E875B26B1ABB98CB47EF0508F133B103BCBE13C8237AE"));
            Assert.That(response[2].Statement.Receipts[0].RecipientAddress, Is.EqualTo("684730D07E8EF59C26C3259696730C75F6E7216730E8C9C8"));
            Assert.That(response[2].Statement.Source.PrimaryId, Is.EqualTo(25657));
            Assert.That(response[2].Statement.Source.SecondaryId, Is.EqualTo(0));
        }
    }
}
