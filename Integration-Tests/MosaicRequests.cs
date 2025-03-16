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
            var client = new MosaicHttp("75.119.150.108", 3000);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchMosaics);
            queryModel.SetParam(QueryModel.DefinedParams.ownerAddress, Address.CreateFromHex("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8").Plain);

            var response = await client.SearchMosaics(queryModel);

            Assert.That(response[0].Id, Is.EqualTo("6644D77F079630C9338F7048"));
            Assert.That(response[0].Mosaic.Id, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response[0].Mosaic.Version, Is.EqualTo(1));
            Assert.That(response[0].Mosaic.Supply, Is.EqualTo(8391424283926782));
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
            var client = new MosaicHttp("75.119.150.108", 3000);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchMosaics);
            queryModel.SetParam(QueryModel.DefinedParams.ownerAddress, Address.CreateFromHex("68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8").Plain);

            var response = await client.GetMosaic("6BED913FA20223F8");

            Assert.That(response.Id, Is.EqualTo("6644D77F079630C9338F7048"));
            Assert.That(response.Mosaic.Id, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response.Mosaic.Version, Is.EqualTo(1));
            Assert.That(response.Mosaic.Supply, Is.EqualTo(8391424283926782));
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
            var client = new MosaicHttp("75.119.150.108", 3000);

            var response = await client.GetMosaics(new List<string> { "63078E73FBCC2CAC", "6BED913FA20223F8" });

            Assert.That(response[0].Id, Is.EqualTo("6644D78C079630C9338F734F"));
            Assert.That(response[0].Mosaic.Id, Is.EqualTo("63078E73FBCC2CAC"));
            Assert.That(response[0].Mosaic.Version, Is.EqualTo(1));
            Assert.That(response[0].Mosaic.Supply, Is.EqualTo(3800000));
            Assert.That(response[0].Mosaic.StartHeight, Is.EqualTo(117));
            Assert.That(response[0].Mosaic.OwnerAddress, Is.EqualTo("6854F763D03307D0281EFC4BB3B4926316F9AEE74EB63EE8"));
            Assert.That(response[0].Mosaic.Revision, Is.EqualTo(1));
            Assert.That(response[0].Mosaic.Flags, Is.EqualTo(3));
            Assert.That(response[0].Mosaic.Divisibility, Is.EqualTo(0));
            Assert.That(response[0].Mosaic.Duration, Is.EqualTo(0));
            Assert.That(response[1].Id, Is.EqualTo("6644D77F079630C9338F7048"));
            Assert.That(response[1].Mosaic.Id, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response[1].Mosaic.Version, Is.EqualTo(1));
            Assert.That(response[1].Mosaic.Supply, Is.EqualTo(8391424283926782));
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
            var client = new MosaicHttp("75.119.150.108", 3000);

            var response = await client.GetMosaicMerkle( "6BED913FA20223F8" );

            Assert.That(response.Raw, Is.EqualTo("0000FFFF8E9AF9A7835AE6880272F08F01B986AC981FCC52B908F77BDF9F419C837356F4E43E4DB6DED68F1A5CCC1E947422CC0ACB132AE59D066E49598E177012C95B86812874CA35E21481B0116F875282867F46C485B26167AC87A2A016379008399210A6F3C0E35F233B29F8C84A80C8E25795F48512A7FAE404DE3158F970722202B789172ECB842DB48DB88BAEC595AD04F0007835A8094DEE756BA2EB6EB1838B79F17B92C87C0A359A5AD6EE8E19FB32774BBF94D1124417F486CDEDC92CEC3CEF1D6FE382485DCA503BC1965EFC96CBBDD35E05F100AF6A03EFCE3AA5E72E96E13D9E26855A24CA32E0D37FFEB10E90CB51BF68F7F6BD50DEAA88BCCF28C58F58EDFF8260C02EBEF033D6FDFE6363F764E02A0CFFA2851E7D53D9D623C0B64C32BD5A93C985D2F5197F82EAC763F135312CBE75BFEDB860127C67E37CD17066C15C4CE5001707DE11CA3BF1DC502E90208E02E0670706083D814CEFBA1B9FCB8D92B10BC568CA5C1ED1F468DEB3732680E602ECC03AA3620BE63BA2013863E78DD58697430CB8865D5539DA79F0290119C2CF8314227186516331016BF5334038E258A28749CFA8333E5872FD735080FA4C81984AF2BB5B3DFFC9DA49B1FCCF8F1B24DD9F8DEFDAF16D895474F0A43D2D6884A86321099F8664FDFDC893CD399F810C10A54EEA87886196C70DAF0E40DA0A713E93469109D355AEB77818304D0000FFFFCBCA0919D64AFD389256047B86EB2FCA8ECCB3CD8A7E6D559F547F1A6CFF2E0C32029B1710056021DA56A28BC7C76E8A776112E733FD84473535D38A243C3B5AEA515F111E2A83BAAB266B363A29CC8717AFE7C830F0DB474FA57A0E702DFC2477B98008969936F8F917B38522D6984D6956E1D1353DC0F4F57F6C6A5748DEFD6A424D23F9125ABE4EF1242F58E2DB1BD769DB9D68598DCAFE963DC1B355E24F99B95366EE5275718781CDE420C479F583AD31084640A74CBD55B824B178FA7DD7F81328B24381E9E97ADEBDCF9FFBC1A3A6C72559529727B79017F7AE10F0E9FEA82CF6BE0906F5F3EFD37480FD6F509EABB471B4D1C6246450DFDDC07D5D47BD27B57E84A619A629432E40C64DD97720C7F2CCB8F6D07FDC6CDA615837A50FA6E8C5BDD336FAA3354F8A9825BEC7FCB818BF40E6D224DBD3010E3AD24AC20FB6A5741679007383E00F5525F1CF957071AEEA8926F240208D6A7F1262F8D8E3F5F9CD6507151DDC765CD3FDA7291972E16E4BC92F9B1EF9067EE98F9063D44529EC490DB3EE70FDCFB450C59627BFCABDCEA1E3C020C79ACEEE070F73A30746C0411A3092D57023D9FD1730B2EA9F980C7836411FAD06C1982E061AD71AC7670433631782B077129C6E268CC8718E42722404ED77BDCD923BC3D0EF2BD7303C5BC95C7774E4DFB75992E9361C1102EE45F9CB2508901E60F6A36828E7CCF1FF0000FFF7703F2D4BADC44AA5CF827187FF084B517C51993AD9B56A57816E66F8ECA4DE0737BF36D439495FA0A033A291472AB5ED89EFEA04BA02B85574AC7AFF048953F592FDE5C7D51DCA70B410F80DFD5B5A2C0A72D1CF5A3EE7ED802F16B82F8A4DA9CF5A9DDECF7C1F1BBEE683611F8A8F6610F0C4795956CB284D04DEF6F4C2A64C3014F1A183141907AD4DE8ED1AE5E5105F8A0F64D404ABB0469D85A1189C42AD8D16A451F4CC1221C5E0A3F41C8B3EDE4ACCD627D38267096C8BD936251BB3AB691329CECE63C2C17613DA52FE73A5C793262193419D609C99DF02E1319B9ABFA7064783A57B120146FDADAFB97FBE428A8B9E9D1D0F291C36993F86DC3710B726BD616BAD0E09873D024D71917E4E61835D928A89A8351CCA7D45F5EDA542424FA1A969EE89BD64CE84CA7951E56CD9FE1E431AA812BF3D6497D1D5F90FE28F653A52A44B1054F0DC1FF7C8CAFCE34B49CA1328F6419248A4D990D6E6DAC83C108B8DC509BA16F11128C11529D7605E97711FECFE08BBBF106AAD7D1F85B9C420FB60FE759A3338DA57454BAC21A9DB66026099FD4E3CE789F168C19BFC9DAF854EA0DCC9F6077A30BBD95270149C1B4921DA5F17A6E0A3F8356ED3867FE9C075795040EFA74A06DC54E4E4E57B6BA29C2CDCB6E1633311F665B8D1C170D92DFF3D950620B3D2E66FBBDC3B99EBB6093C57481A259E4FC673D110CC0404E738F03A511F23153538180CE67271991643810F21D3D4E281E7313CEF9CA90DBC99B6"));
            Assert.That(response.Tree[1].Type, Is.EqualTo(0));
            Assert.That(response.Tree[1].NibbleCount, Is.EqualTo(0));
            Assert.That(response.Tree[1].BranchHash, Is.EqualTo("E43E4DB6DED68F1A5CCC1E947422CC0ACB132AE59D066E49598E177012C95B86"));
            Assert.That(response.Tree[1].Value, Is.Null);
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicDefinitionTransaction()
        {
            var client = new TransactionHttp("75.119.150.108", 3000);

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
            var client = new TransactionHttp("75.119.150.108", 3000);

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

            var client = new TransactionHttp("75.119.150.108", 3000);

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

            var client = new TransactionHttp("75.119.150.108", 3000);

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
