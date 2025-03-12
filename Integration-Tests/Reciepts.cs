using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
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
            var recieptsHttp = new BlockReceiptsHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.GetRecieptsAddressResolutionStatements);

            var response = await recieptsHttp.GetAddressStatements(qModel);

            Assert.That(response[0].Id, Is.EqualTo("6644D79BED4FBE21460A4898"));
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
            var recieptsHttp = new BlockReceiptsHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.GetRecieptsAddressResolutionStatements);

            var response = await recieptsHttp.GetMosaicStatements(qModel);

            Assert.That(response[2].Id, Is.EqualTo("6644D79FED4FBE21460A544C"));
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
            var recieptsHttp = new BlockReceiptsHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchTransactionStatements);

            var response = await recieptsHttp.SearchTransactionStatements(qModel);

            Assert.That(response[2].Id, Is.EqualTo("6644D77CED4FBE21460A2122"));
            Assert.That(response[2].Meta.Timestamp, Is.EqualTo(0));
            Assert.That(response[2].Statement.Height, Is.EqualTo(1));
            Assert.That(response[2].Statement.Receipts[0].MosaicId, Is.EqualTo("6BED913FA20223F8"));

            Assert.That(response[2].Statement.Receipts[0].Type, Is.EqualTo(4942)); // flag
            // https://docs.symbol.dev/concepts/receipt.html#recorded-receipts

            Assert.That(response[2].Statement.Receipts[0].Version, Is.EqualTo(1));
            Assert.That(response[2].Statement.Receipts[0].Amount, Is.EqualTo(0));
            Assert.That(response[2].Statement.Receipts[0].SenderAddress, Is.EqualTo("6800AFEF07B9AADD3461BA1158ADDE1CB71CC1BF652EFDE3"));
            Assert.That(response[2].Statement.Receipts[0].RecipientAddress, Is.EqualTo("684730D07E8EF59C26C3259696730C75F6E7216730E8C9C8"));
            Assert.That(response[2].Statement.Source.PrimaryId, Is.EqualTo(25592));
            Assert.That(response[2].Statement.Source.SecondaryId, Is.EqualTo(0));
        }
    }
}
