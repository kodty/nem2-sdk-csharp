using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
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

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i => {

                var tx = ((NamespaceRegistration)i.Transaction);

                if (tx.RegistrationType == 0)
                {
                    var t = (RootNamespaceRegistration)tx;
                    Assert.That(t.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(t.Name, Is.EqualTo("symbol"));
                    Assert.That(t.Duration, Is.EqualTo(0));
                    Assert.That(t.Id, Is.EqualTo("A95F1F8A96159516"));

                    Assert.That(tx.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                }
                if (tx.RegistrationType == 1)
                {
                    var t = (ChildNamespaceRegistration)tx;
                    Assert.That(t.SignerPublicKey, Is.EqualTo(pubKey));                
                    Assert.That(t.Name, Is.EqualTo("xym"));
                    Assert.That(t.Id, Is.EqualTo("E74B99BA41F4AFEE"));
                    Assert.That(t.ParentId, Is.EqualTo("A95F1F8A96159516"));
                    Assert.That(t.Network, Is.EqualTo(NetworkType.Types.MAIN_NET));
                }
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchNamespaces()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchNamespaces);

            var response = await client.SearchNamespaces(queryModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            foreach (var item in response)
            {
                Assert.That(item.Id.Length, Is.EqualTo(24));
                Assert.That(item.Meta.Active, Is.EqualTo(true));
                Assert.That(item.Meta.Index, Is.EqualTo(0));

                Assert.That(item.Namespace.Version, Is.EqualTo(1));
                Assert.That(item.Namespace.StartHeight, Is.GreaterThan(0));
                Assert.That(item.Namespace.Level0.Length, Is.EqualTo(16));
                if(item.Namespace.Level1 != null) Assert.That(item.Namespace.Level1.Length, Is.EqualTo(16));
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

            Assert.That(response.Id, Is.EqualTo("64503822B0FC9F6D340B0B89"));
            Assert.That(response.Meta.Active, Is.EqualTo(true));
            Assert.That(response.Meta.Index, Is.EqualTo(0));           
            Assert.That(response.Namespace.RegistrationType, Is.EqualTo(0));
            Assert.That(response.Namespace.Level0, Is.EqualTo("A95F1F8A96159516"));
            Assert.That(response.Namespace.Depth, Is.EqualTo(1));
            Assert.That(response.Namespace.Alias.Type, Is.EqualTo(0));
            if(response.Namespace.Alias.Address != null) Assert.That(response.Namespace.Alias.Address.Length, Is.EqualTo(48));
            if (response.Namespace.Alias.MosaicId != null) Assert.That(response.Namespace.Alias.MosaicId.Length, Is.EqualTo(16));
            Assert.That(response.Namespace.ParentId, Is.EqualTo("0000000000000000"));
            Assert.That(response.Namespace.OwnerAddress, Is.EqualTo("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8"));
            Assert.That(response.Namespace.EndHeight, Is.EqualTo(18446744073709551615));
        }

        [Test, Timeout(20000)]
        public async Task GetNamespaceMerkle()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetNamespaceMerkle("A95F1F8A96159516");

            Assert.That(response.Raw, Is.EqualTo("0000FFFFCBC69DD39AC09FDDC903E290E9804D536C0B44DE8A555F14682D6F85829FCABFB3A3F01087DCFBED18668F08C5150A14206A1E43E8A824435572ACC7CE05F0B664F68FAC35600DD35E8F7A6FADAF9423B891004A5B4B5DBD97BEC8D2E6BBA6C3C5CAA102E352EDBC570A6529E063C3C3CE7D3A587A874445B5071821C2C3553D2DB19E8EA12C0DF41F33DB38EE6CB82EB1673C4AD91E62B060C598AAA1B526749571B54FEF5F2B238A3860D4C8E1770528A1EB37963EADA19CEA6F02A4858E9945D7B6AE0121CE4CA2E22C0B49F263E4364848DDCC4D4C85BC23F18D75E5C0A8354AAEC0FEF4EB3F112D028D591E7B023141FB43E58B9C29F43A2242C3E9DF01D254FDEDD8BFC3BAF2BADC6CE404F562E58F61ABF74BE349A46F498E0F98B74018A643D1E442534BE8DF4F3FD6050F23DD033079CA138D30273431524ABFF4DE8C790322F77D025584D6D066B1CA4E3AA01AC92C70BD205246052FCDA7AC01831DE3ED6E01AF0F32CE9590F7154290141E761F16B7B9CB1962EA0EF2B12A612E9A14921C71619D2624980A7CF01BCFE237C4E4EC3A9A73375321885730D2BF4C4616AB96B8476E72612F219E251052BE2F967B373236F7C85C440DE5D3EB616FAA202FB73843B6F7F5C5494EF691708631099211EAFA93EC2F8E1A94FB24BF59DD02F764B3DB6BE9D594CC9C8E77359CEDA476659628D9C33560ADCBF2B743570000958D6A2762E3CAF3F3910E30051F1BD584730EAB629BF48508D4C7983494172F2E43E67816E2EFF469C31E7D1C01889454D02087DF9E6602D89EC2D9512FFAB6057769DD0501D7368DE7D6BF5C44C55E22146CEFF01EA6E88E70660BD40B8B41C77579B51FB595A90FEEE720C1194E024D31B07C9AFCB46A6F00EFEF6739B7AEA4408FBB62A7F618CDC99C454266990FA19BD8B20B7620DA363F91442194C5090E1485AA375FCF9F46F82E71126195A6DA0C0DD2F00378C16C7B986C5DDDA15192C62131BB884AEA6D68A2C04DB4195CF7EE3319B95F39EB8FC86F62C2E1B113E02FF616A227B6869EB0D7855F79039A34A896925E399DA8E43184DD7DBCC0202285FF3E793E37E8616665E4E5DA0CAF388172AB754CEE17FE702019228A996084F6E7F48F12376B7C72F97E1533DE6DDB6F957DAB4F9031F959261AA2C5B655C864AA"));
            Assert.That(response.Tree[1].Type, Is.EqualTo(0));
            Assert.That(response.Tree[1].Links[0].Link, Is.EqualTo("6A2762E3CAF3F3910E30051F1BD584730EAB629BF48508D4C7983494172F2E43"));
        }

        [Test, Timeout(20000)]
        public async Task GetNamespaceNames()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetNamespacesNames(new List<string> { "A95F1F8A96159516" });

            Assert.That(response[0].Name, Is.EqualTo("symbol"));
            Assert.That(response[0].ParentId, Is.Null);
            Assert.That(response[0].Id, Is.EqualTo("A95F1F8A96159516"));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountNames()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetAccountNames(new List<string> { "NBCXLKLGGDWGYC47X42AQADSCMZBV7YHU6BX4UA" });

            Assert.That(response[0].Address, Is.EqualTo("684575A96630EC6C0B9FBF3408007213321AFF07A7837E50"));
            Assert.That(response[0].Names, Is.Empty);
           
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicNames()
        {
            var client = new NamespaceHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMosaicNames(new List<string> { "A95F1F8A96159516" });

            Assert.That(response[0].names, Is.Empty);
            Assert.That(response[0].mosaicId, Is.EqualTo("A95F1F8A96159516"));
        }
    }
}
