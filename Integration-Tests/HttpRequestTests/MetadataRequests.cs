using Coppery;
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
            Assert.IsTrue(response[0].MetadataEntry.CompositeHash.IsHex(64));
            Assert.IsTrue(response[0].MetadataEntry.SourceAddress.IsHex(48));
            Assert.IsTrue(response[0].MetadataEntry.TargetAddress.IsHex(48));
            Assert.IsTrue(response[0].MetadataEntry.ScopedMetadataKey.IsHex(16));
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
            Assert.IsTrue(response.MetadataEntry.CompositeHash.IsHex(64)); 
            Assert.IsTrue(response.MetadataEntry.SourceAddress.IsHex(48)); 
            Assert.IsTrue(response.MetadataEntry.TargetAddress.IsHex(48));
            Assert.IsTrue(response.MetadataEntry.ScopedMetadataKey.IsHex(16));
            Assert.That(response.MetadataEntry.TargetId, Is.EqualTo("0000000000000000"));
            Assert.That(response.MetadataEntry.MetadataType, Is.EqualTo(0));
            Assert.That(response.MetadataEntry.Value, Is.EqualTo("323632313738"));
        }

        [Test, Timeout(20000)]
        public async Task GetMetadataMerkle()
        {
            var client = new MetadataHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMetadataMerkle("878ABDD6BA8A311255A6D9967C93BA4957414709CF763CA0BB5A78A47108052F");

            Assert.That(response.Tree[0].Links[0].Link.IsHex(64));
            Assert.That(response.Raw.IsHex(2056));
            Assert.That(response.Tree[0].Type, Is.EqualTo(0));
            Assert.That(response.Tree[0].NibbleCount, Is.EqualTo(0));
            Assert.That(response.Tree[0].Value, Is.Null);
            Assert.That(response.Tree[0].BranchHash.IsHex(64));
        }
    }
}
