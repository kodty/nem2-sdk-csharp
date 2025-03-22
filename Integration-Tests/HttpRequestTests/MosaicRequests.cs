using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
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

            var response = await client.GetMosaicMerkle("6BED913FA20223F8");

            Assert.That(response.Tree[1].Type, Is.EqualTo(0));
            Assert.That(response.Tree[1].NibbleCount, Is.EqualTo(0));
            Assert.That(response.Tree[1].Links[0].Link, Is.EqualTo("C3ED57BC62D90EDEB02F390061ED622B43F3ACB2A60C193B91466A6A002F8675"));
            Assert.That(response.Tree[0].BranchHash.Length, Is.EqualTo(64));
            Assert.That(response.Tree[1].Value, Is.Null);
        }


    }
}
