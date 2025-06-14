using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    internal class MetadataRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchMetadata()
        {
            var client = new MetadataHttp(HttpSetUp.Node, HttpSetUp.Port);

            var queryModel = new QueryModel();
            queryModel.SetParam(QueryModel.DefinedParams.pageNumber, 2);

            var response = await client.SearchMetadataEntries(queryModel);

            Assert.That(response[0].MetadataEntry.Version, Is.EqualTo(1));
            Assert.That(response[0].MetadataEntry.ValueSize, Is.EqualTo(6));
            Assert.That(response[0].MetadataEntry.CompositeHash, Is.EqualTo("878ABDD6BA8A311255A6D9967C93BA4957414709CF763CA0BB5A78A47108052F"));
            Assert.That(response[0].MetadataEntry.SourceAddress, Is.EqualTo("682F70FA28E167702898EFDFCCA9B808304B86A22117A86F"));
            Assert.That(response[0].MetadataEntry.TargetAddress, Is.EqualTo("68FAF5ACB04257242865BBC5D1C712E7538ECC92930660B3"));
            Assert.That(response[0].MetadataEntry.ScopedMetadataKey, Is.EqualTo("BCCC2EE01677A923"));
            Assert.That(response[0].MetadataEntry.TargetId, Is.EqualTo("0000000000000000"));
            Assert.That(response[0].MetadataEntry.Value, Is.EqualTo("323632313738"));
        }

        [Test, Timeout(20000)]
        public async Task GetMetadata()
        {
            var client = new MetadataHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMetadata("878ABDD6BA8A311255A6D9967C93BA4957414709CF763CA0BB5A78A47108052F");


            Assert.That(response.MetadataEntry.Version, Is.EqualTo(1));
            Assert.That(response.MetadataEntry.ValueSize, Is.EqualTo(6));
            Assert.That(response.MetadataEntry.CompositeHash, Is.EqualTo("878ABDD6BA8A311255A6D9967C93BA4957414709CF763CA0BB5A78A47108052F"));
            Assert.That(response.MetadataEntry.SourceAddress, Is.EqualTo("682F70FA28E167702898EFDFCCA9B808304B86A22117A86F"));
            Assert.That(response.MetadataEntry.TargetAddress, Is.EqualTo("68FAF5ACB04257242865BBC5D1C712E7538ECC92930660B3"));
            Assert.That(response.MetadataEntry.ScopedMetadataKey, Is.EqualTo("BCCC2EE01677A923"));
            Assert.That(response.MetadataEntry.TargetId, Is.EqualTo("0000000000000000"));
            Assert.That(response.MetadataEntry.MetadataType, Is.EqualTo(0));
            Assert.That(response.MetadataEntry.Value, Is.EqualTo("323632313738"));
        }

        [Test, Timeout(20000)]
        public async Task GetMetadataMerkle()
        {
            var client = new MetadataHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMetadataMerkle("878ABDD6BA8A311255A6D9967C93BA4957414709CF763CA0BB5A78A47108052F");

            Assert.That(response.Tree[0].Links[0].Link, Is.EqualTo("77CEA282B87990335028CF914A96BE78D7D842E7681EF7116CDC0A3E5CC6024B"));
            //Assert.That(response.Raw, Is.EqualTo("0000FFFF77CEA282B87990335028CF914A96BE78D7D842E7681EF7116CDC0"));
            Assert.That(response.Tree[0].Type, Is.EqualTo(0));
            Assert.That(response.Tree[0].NibbleCount, Is.EqualTo(0));
            Assert.That(response.Tree[0].Value, Is.Null);
            Assert.That(response.Tree[0].BranchHash, Is.EqualTo("65C492990917A6B96E5619565C6E937281C521925A232FEA23B5A6DD4703F3E6"));
        }
    }
}
