using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
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
        public async Task SearchNamespaces()
        {
            var metadataHttp = new NamespaceHttp("75.119.150.108", 3000);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchNamespaces);
            queryModel.SetParam(QueryModel.DefinedParams.ownerAddress, Address.CreateFromHex("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8").Plain);

            var response = await metadataHttp.SearchNamespaces(queryModel);

            Assert.That(response[0].Id, Is.EqualTo("6644D77FED4FBE21460A2240"));
            Assert.That(response[0].Namespace.RegistrationType, Is.EqualTo(0));
            Assert.That(response[0].Namespace.Alias.Type, Is.EqualTo(0));
        }

        [Test, Timeout(20000)]
        public async Task GetNamespace()
        {
            var metadataHttp = new NamespaceHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetNamespace("A95F1F8A96159516");

            Assert.That(response.Id, Is.EqualTo("6644D77FED4FBE21460A2240"));
            Assert.That(response.Namespace.RegistrationType, Is.EqualTo(0));
            Assert.That(response.Namespace.Level0, Is.EqualTo("A95F1F8A96159516"));
            Assert.That(response.Namespace.Depth, Is.EqualTo(1));
            Assert.That(response.Namespace.Alias.Type, Is.EqualTo(0));
            Assert.That(response.Namespace.ParentId, Is.EqualTo("0000000000000000"));
            Assert.That(response.Namespace.OwnerAddress, Is.EqualTo("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8"));
            Assert.That(response.Namespace.EndHeight, Is.EqualTo(18446744073709551615));
        }

        [Test, Timeout(20000)]
        public async Task GetNamespaceMerkle()
        {
            var metadataHttp = new NamespaceHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetNamespaceMerkle("A95F1F8A96159516");

            Assert.That(response.Raw, Is.EqualTo("0000FFFF5D4A0DE9ECA0B19A4B906A04ACB0C9B576C1F2A26D7DE13A7E85F3782C009630C1DC764FAE2C8C8BE46D34F596E0B052B502872235B0851894046C799BEBF5850E06C80D3E5BE5B9B5F3551727B2461324DF7E68409F9067CF48EEEA2B2201E9EEBDE9EA860338ED40C7A2171A8F74BE2AE0140F88BF2F793D0259F46C0A4BF955DC0BA8A6D57F7C7FC9BDB6CE55EB3B1964C2FBC174640A5AA4FBB2A7FEBAC4A43261678C2D7CE0863F4A1FE20FF83D3185DDDEE078D4AF1699AC792B222D7EE67A5B52F8F9A2C191CBB470ABA4A9964CF478D9234C15890BF4F5D01D1023E15612E6E9B045B41B7FA5D0250EF64766D12D93A39C3F5E422078CC051176770BCA99A41ABEBEE29B3310BD86CAFC65695777E0012E757EA094531DC215B9FB7518A643D1E442534BE8DF4F3FD6050F23DD033079CA138D30273431524ABFF4DE8B7A0608A4F677BDD2C4F8E44B1D7CFABE161622F23C7C2E199B02DB5D68074C1DE3ED6E01AF0F32CE9590F7154290141E761F16B7B9CB1962EA0EF2B12A612E1AB3B4ABF5589BB4568DDA44DC7C419EB9C594E28FB4F67E523C1FCA5D046A3A6890369864D5BEF5BDC1ADD2C345DD7ACC3494AD725014C4C3656E12ACCB2A7705DBA6FD6AC4E6FDCF4040743AF74C189F4424A804CBB6871B091AEE63B41476C95302AC85DB9A81DA91AD0106F2CF9D5817266638D2AD78D67227D10C8AF361000095DD6A2762E3CAF3F3910E30051F1BD584730EAB629BF48508D4C7983494172F2E4315C0A8D983DC091ECCB28E4EADF3F3FD65C2A0FD1C6FBD9BC1F5183AD656814669DD0501D7368DE7D6BF5C44C55E22146CEFF01EA6E88E70660BD40B8B41C77532B4A8E4DC75860B0ECBA0308950456CF26CEB7472399051F648F91EEDB1E4927474FB08E5BB7D342C66C9F153AECDC9C8060CE1CC0B8624FA64C3D2F25558E285AA375FCF9F46F82E71126195A6DA0C0DD2F00378C16C7B986C5DDDA15192C62131BB884AEA6D68A2C04DB4195CF7EE3319B95F39EB8FC86F62C2E1B113E02FCDA71B001C137010642590BA32A9BCC51C3897A726DE217F819D2D8B34F7387EFF958F682E2F5AE215F4FC8DBD31D28B6EE3BEB5033F1D4D3D3FE350E4D0FFA4F616A227B6869EB0D7855F79039A34A896925E399DA8E43184DD7DBCC0202285FF3E793E37E8616665E4E5DA0CAF388172AB754CEE17FE702019228A996084F6E7F48F12376B7C72F97E1533DE6DDB6F957DAB4F9031F959261AA2C5B655C864AA"));
            Assert.That(response.Tree[1].Type, Is.EqualTo(0));
            Assert.That(response.Tree[1].Links[0].Link, Is.EqualTo("6A2762E3CAF3F3910E30051F1BD584730EAB629BF48508D4C7983494172F2E43"));
        }

        [Test, Timeout(20000)]
        public async Task GetNamespaceNames()
        {
            var metadataHttp = new NamespaceHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetNamespacesNames(new List<string> { "A95F1F8A96159516" });

            Assert.That(response[0].Name, Is.EqualTo("symbol"));
            Assert.That(response[0].ParentId, Is.Null);
            Assert.That(response[0].Id, Is.EqualTo("A95F1F8A96159516"));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountNames()
        {
            var metadataHttp = new NamespaceHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetAccountNames(new List<string> { "NBCXLKLGGDWGYC47X42AQADSCMZBV7YHU6BX4UA" });

            Assert.That(response[0].Address, Is.EqualTo("684575A96630EC6C0B9FBF3408007213321AFF07A7837E50"));
            Assert.That(response[0].Names, Is.Empty);
           
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicNames()
        {
            var metadataHttp = new NamespaceHttp("75.119.150.108", 3000);

            var response = await metadataHttp.GetMosaicNames(new List<string> { "A95F1F8A96159516" });

            Assert.That(response[0].names, Is.Empty);
            Assert.That(response[0].mosaicId, Is.EqualTo("A95F1F8A96159516"));
        }

    }
}
