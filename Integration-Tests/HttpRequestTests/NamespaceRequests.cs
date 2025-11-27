using io.nem2.sdk.Infrastructure.HttpRepositories;
using Coppery;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using io.nem2.sdk.src.Model;

namespace Integration_Tests.HttpRequests
{
    internal class NamespaceRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearcNameSpaceRegistrationTransaction()
        {
            string pubKey = "BE0B4CF546B7B4F4BBFCFF9F574FDA527C07A53D3FC76F8BB7DB746F8E8E0A9F";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.NAMESPACE_REGISTRATION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.ComposedResponse.Count, Is.GreaterThan(0));

            response.ComposedResponse.ForEach(i =>
            {

                var tx = (NamespaceRegistration)i.Transaction;

                if (tx.RegistrationType == 0)
                {
                    var t = (RootNamespaceRegistration)tx;
                    Assert.That(t.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(t.Name, Is.EqualTo("symbol"));
                    Assert.That(t.Duration, Is.EqualTo(0));
                    Assert.That(t.Id, Is.EqualTo("A95F1F8A96159516"));

                    Assert.That(tx.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
                }
                if (tx.RegistrationType == 1)
                {
                    var t = (ChildNamespaceRegistration)tx;
                    Assert.That(t.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(t.Name, Is.EqualTo("xym"));
                    Assert.That(t.Id, Is.EqualTo("E74B99BA41F4AFEE"));
                    Assert.That(t.ParentId, Is.EqualTo("A95F1F8A96159516"));
                    Assert.That(t.Network.GetNetworkValue(), Is.EqualTo(NetworkType.Types.MAIN_NET));
                }
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchNamespaces()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchNamespaces);

            var response = await client.SearchNamespaces(queryModel);

            Assert.That(response.ComposedResponse.Data.Count, Is.GreaterThan(0));

            foreach (var item in response.ComposedResponse.Data)
            {
                Assert.That(item.Id.Length, Is.EqualTo(24));
                Assert.That(item.Meta.Active, Is.EqualTo(true));
                Assert.That(item.Meta.Index, Is.EqualTo(0));

                Assert.That(item.Namespace.Version, Is.EqualTo(1));
                Assert.That(item.Namespace.StartHeight, Is.GreaterThan(0));
                Assert.That(item.Namespace.Level0.Length, Is.EqualTo(16));
                if (item.Namespace.Level1 != null) Assert.That(item.Namespace.Level1.Length, Is.EqualTo(16));
                if (item.Namespace.Level2 != null) Assert.That(item.Namespace.Level2.Length, Is.EqualTo(16));
                Assert.That(item.Namespace.RegistrationType, Is.AnyOf(0, 1));
                Assert.That(item.Namespace.Alias.Type, Is.AnyOf(0, 1, 2));

                if (item.Namespace.Alias.MosaicId != null) Assert.That(item.Namespace.Alias.MosaicId.Length, Is.EqualTo(16));
                if (item.Namespace.Alias.Address != null) Assert.That(item.Namespace.Alias.Address.Length, Is.EqualTo(48));
            }
        }

        [Test, Timeout(20000)]
        public async Task GetNamespace()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetNamespace("A95F1F8A96159516");

            Assert.That(response.ComposedResponse.Id.IsHex(16));
            Assert.That(response.ComposedResponse.Meta.Active, Is.EqualTo(true));
            Assert.That(response.ComposedResponse.Meta.Index, Is.EqualTo(0));
            Assert.That(response.ComposedResponse.Namespace.RegistrationType, Is.EqualTo(0));
            Assert.IsTrue(response.ComposedResponse.Namespace.Level0.IsHex(16));
            Assert.That(response.ComposedResponse.Namespace.Depth, Is.EqualTo(1));
            Assert.That(response.ComposedResponse.Namespace.Alias.Type, Is.EqualTo(0));
            if (response.ComposedResponse.Namespace.Alias.Address != null) Assert.That(response.ComposedResponse.Namespace.Alias.Address.Length, Is.EqualTo(48));
            if (response.ComposedResponse.Namespace.Alias.MosaicId != null) Assert.That(response.ComposedResponse.Namespace.Alias.MosaicId.Length, Is.EqualTo(16));
            Assert.That(response.ComposedResponse.Namespace.ParentId, Is.EqualTo("0000000000000000"));
            Assert.That(response.ComposedResponse.Namespace.OwnerAddress.IsHex(48));
            Assert.That(response.ComposedResponse.Namespace.EndHeight, Is.EqualTo(18446744073709551615));
        }

        [Test, Timeout(20000)]
        public async Task GetNamespaceMerkle()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetNamespaceMerkle("A95F1F8A96159516");

            Assert.That(response.ComposedResponse.Raw.Length, Is.EqualTo(1682));
            Assert.That(response.ComposedResponse.Tree[1].Type, Is.EqualTo(0));
            Assert.That(response.ComposedResponse.Tree[1].Links[0].Link.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetNamespaceNames()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetNamespacesNames(new List<string> { "A95F1F8A96159516" });

            Assert.That(response.ComposedResponse[0].Name, Is.EqualTo("symbol"));
            Assert.That(response.ComposedResponse[0].ParentId, Is.Null);
            Assert.That(response.ComposedResponse[0].Id.IsHex(16));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountNames()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetAccountNames(new List<string> { "NBCXLKLGGDWGYC47X42AQADSCMZBV7YHU6BX4UA" });

            Assert.IsTrue(response.ComposedResponse.AccountNames[0].Address.IsHex(48));
            Assert.That(response.ComposedResponse.AccountNames[0].Names, Is.Empty);

        }
       

        [Test, Timeout(20000)]
        public async Task GetMosaicNames()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMosaicNames(new List<string> { "6BED913FA20223F8" });

            Assert.That(response.ComposedResponse.MosaicNames[0].Names[0], Is.EqualTo( "symbol.xym"));
            Assert.That(response.ComposedResponse.MosaicNames[0].MosaicId.IsHex(16));
        }
    }
}
