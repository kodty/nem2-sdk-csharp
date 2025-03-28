using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    public class MosaicRestrictionRequests
    {
        [SetUp]
        public async Task SetUp()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicRestriction()
        {
            var client = new MosaicHttp(HttpSetUp.Node, HttpSetUp.Port);

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchMosaicRestrictions);
            queryModel.SetParam(QueryModel.DefinedParams.pageNumber, 2);


            var response = await client.SearchMosaicRestrictions(queryModel);

            Assert.That(response[3].MosaicRestrictionEntry.MosaicId, Is.EqualTo("613E6D0FC11F4530"));
            Assert.That(response[3].MosaicRestrictionEntry.Version, Is.EqualTo(1));
            Assert.That(response[3].MosaicRestrictionEntry.TargetAddress, Is.EqualTo("68D0C092D4C97729FD5C7E625E675FE50131350C540D381D"));
            Assert.That(response[3].MosaicRestrictionEntry.CompositeHash, Is.EqualTo("5B64D25F3D07DF33CD580B78B638B3B85553EF5F02890C1B8CE70286771497B0"));
            Assert.That(response[3].MosaicRestrictionEntry.EntryType, Is.EqualTo(0));
            Assert.That(response[3].MosaicRestrictionEntry.Restrictions[0].Key, Is.EqualTo("14694524492525660186"));
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicRestriction()
        {
            var client = new MosaicHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMosaicRestriction("048113BBAE7C5739F71C474FBD92EB911D4048170FC05EDEF28C4EDF8C665F52");

            Assert.That(response.Id, Is.EqualTo("6665D8B1D7CFAAB6174EF835"));
            Assert.That(response.MosaicRestrictionEntry.MosaicId, Is.EqualTo("613E6D0FC11F4530"));
            Assert.That(response.MosaicRestrictionEntry.Version, Is.EqualTo(1));
            Assert.That(response.MosaicRestrictionEntry.TargetAddress, Is.EqualTo("6875A613C7F4D9A220DB3E141830ECC7132458D01A45787E"));
            Assert.That(response.MosaicRestrictionEntry.CompositeHash, Is.EqualTo("048113BBAE7C5739F71C474FBD92EB911D4048170FC05EDEF28C4EDF8C665F52"));
            Assert.That(response.MosaicRestrictionEntry.EntryType, Is.EqualTo(0));
            Assert.That(response.MosaicRestrictionEntry.Restrictions[0].Key, Is.EqualTo("14694524492525660186"));
            Assert.That(response.MosaicRestrictionEntry.Restrictions[0].Value, Is.EqualTo("1"));
        }

        [Test, Timeout(20000)]
        public async Task SearchMosaicAddressRestriction()
        {
            string pubKey = "A39EA1EEA2BF80902ED5B573FC9DEE1EDF53FB6E05099669743DFA3E8233400E";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                var tx = (MosaicAddressRestriction)i.Transaction;

                Assert.That(tx.RestrictionKey.Length, Is.GreaterThan(0));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("A39EA1EEA2BF80902ED5B573FC9DEE1EDF53FB6E05099669743DFA3E8233400E"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(tx.MosaicId.Length, Is.EqualTo(16));
                Assert.That(tx.RestrictionKey.Length, Is.EqualTo(16));
                Assert.That(tx.TargetAddress.Length, Is.EqualTo(48));
                Assert.That(tx.NewRestrictionValue, Is.GreaterThan(0));
                Assert.That(tx.PreviousRestrictionValue, Is.LessThanOrEqualTo(18446744073709551615));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicRestrictionMerkle()
        {
            var client = new MosaicHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMosaicRestrictionMerkle("048113BBAE7C5739F71C474FBD92EB911D4048170FC05EDEF28C4EDF8C665F52");

            Assert.That(response.Tree[0].Links[0].Link, Is.EqualTo("AE52FA5CCFAFDB5BD0F91E0CE8B4DAA342734389C2A5D53D184A5D5C2BEC5AC4"));
            Assert.That(response.Raw, Is.EqualTo("0000FFF7AE52FA5CCFAFDB5BD0F91E0CE8B4DAA342734389C2A5D53D184A5D5C2BEC5AC4F2159CAF6A45951EDD5A7E233FB1C37BADE91912A3FBA80E8CA760CD1290748F9356A913B155B63B63A22F9D3994569DF7DA787086DB3F734C94861724D449B2ED692013CF9C97A04067AA9A6BA4C341F785848F809DB4C6EF4057BDE848A8EB1D262602897A0D502E12C855AE31E2185396FBD59E83DB464E09A0926CE75C1F971142DBB2848AADF630CD4993CFEE8EC4EF41C15B7ED25F3A396730B64BAEB39C3E0BBA94268BCFE29C41AD8DF86F58CDB378B9551D492C119909A7965796993DF07CD13509EA9BD3202E3571D735480A6E9A1B09CE37CC0DF302EB7E21F42105D994CFA6210BC56A8C9611BC828F9F02DAAC6DDD5394A400A8FB545812B4E84AE8750416953AEC5C89A607FF4FF0AE5DFFEFB7811DB800390485D75E65ECF3A81A388F72E9812B728085BFE75AF3C07C4026922B6CAB35B4AC1C37FA11C428EA80DF779DBE607A3C367C22B31080811AF29C7C0EF728364B7A2F3CC22586601A7A5031B4A8B0711FD3740BD7BBF6610BCB227A7E81E7CE4AC8F4BCD6A14311C171CB29B47C5B2800EF7AFFA45B14874D88C7A96295ABF8B47BFF33B057D9361CFA30BA02ABB0A9DBC7D4B99E0C6FFC68894EA769253BBBE496D0C0BD38CBB400005164FB39709AB1C7AD06884BF0D1CDAD7D9EF42B7955AACF70104FDDE84446EC584570A600140928A2F9B07F29EE9BC49314F0E08F8FB0E20F470696C581AF3286ECC0838AA96D068E2A721D7335A7446525AD47D1CF26465A9D6879EF2B28126F94D5A993A1FB4E2A3B7B7FC2D1CD41B3EA37F98419D7D34D4EB3AC0B4C5BE382B2A4CEB6F7FCBA26B3DA5922AF2E6536277500DEFFB793B3FA1F62224A4789A07D38FA04FF4A257305C5AF91CE7A4F3E10CDD83A76145FF5E5F092E3EB5691CBE1FF3E2F104C1E6036A6B587BEDB09F4A9F2E53D7E8BDC49D4BB605A6B4B32E04EBF575CE5ACD0A82E3083333DBBA21B5F156AF41AC8EF8CF11F767ED2CCAC64AEF9"));
            Assert.That(response.Tree[0].Type, Is.EqualTo(0));
            Assert.That(response.Tree[0].NibbleCount, Is.EqualTo(0));
            Assert.That(response.Tree[0].Value, Is.Null);
            Assert.That(response.Tree[0].BranchHash, Is.EqualTo("71F6FA9C3B6F19BB35210D8B6F3736254A699B5F0BD8759169CFC3D3D1C65007"));

        }
    }
}
