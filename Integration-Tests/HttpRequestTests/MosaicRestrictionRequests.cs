using io.nem2.sdk.Infrastructure.HttpRepositories;
using Coppery;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Diagnostics;
using System.Reactive.Linq;
using io.nem2.sdk.src.Model;

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

            Assert.That(response.ComposedResponse.Data[3].MosaicRestrictionEntry.MosaicId, Is.EqualTo("613E6D0FC11F4530"));
            Assert.That(response.ComposedResponse.Data[3].MosaicRestrictionEntry.Version, Is.EqualTo(1));
            Assert.That(response.ComposedResponse.Data[3].MosaicRestrictionEntry.TargetAddress, Is.EqualTo("68EE1B99E5D8BE46B1921A49CBD85B704C2DABA2A09235F9"));
            Assert.That(response.ComposedResponse.Data[3].MosaicRestrictionEntry.CompositeHash, Is.EqualTo("FDA655832A99844A650868FDED88B2A9723A70B32CAAF5BD8B32A5CDD1318972"));
            Assert.That(response.ComposedResponse.Data[3].MosaicRestrictionEntry.EntryType, Is.EqualTo(0));
            Assert.IsTrue(response.ComposedResponse.Data[3].MosaicRestrictionEntry.Restrictions[0].Key.IsHex(16));
        }

        [Test, Timeout(20000)]
        public async Task GetMosaicRestriction()
        {
            var client = new MosaicHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetMosaicRestriction("048113BBAE7C5739F71C474FBD92EB911D4048170FC05EDEF28C4EDF8C665F52");

            Assert.IsTrue(response.ComposedResponse.Id.IsHex(24));
            Assert.IsTrue(response.ComposedResponse.MosaicRestrictionEntry.MosaicId.IsHex(16));
            Assert.That(response.ComposedResponse.MosaicRestrictionEntry.Version, Is.EqualTo(1));
            Assert.IsTrue(response.ComposedResponse.MosaicRestrictionEntry.TargetAddress.IsHex(48));
            Assert.IsTrue(response.ComposedResponse.MosaicRestrictionEntry.CompositeHash.IsHex(64));
            Assert.That(response.ComposedResponse.MosaicRestrictionEntry.EntryType, Is.EqualTo(0));
            Assert.IsTrue(response.ComposedResponse.MosaicRestrictionEntry.Restrictions[0].Key.IsHex(16));
            Assert.That(response.ComposedResponse.MosaicRestrictionEntry.Restrictions[0].Value, Is.EqualTo("1"));
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

            Assert.That(response.ComposedResponse.Count, Is.GreaterThan(0));

            response.ComposedResponse.ForEach(i =>
            {

                var tx = (MosaicAddressRestriction)i.Transaction;

                Assert.IsTrue(tx.RestrictionKey.IsHex(16));
                Assert.IsTrue(tx.SignerPublicKey.IsHex(64));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(tx.MosaicId.Length, Is.EqualTo(16));
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

            Assert.That(response.ComposedResponse.Tree[0].Links[0].Link.IsHex(64));
            Assert.IsTrue(response.ComposedResponse.Raw.IsHex(1490));
            Assert.That(response.ComposedResponse.Tree[0].Type, Is.EqualTo(0));
            Assert.That(response.ComposedResponse.Tree[0].NibbleCount, Is.EqualTo(0));
            Assert.That(response.ComposedResponse.Tree[0].Value, Is.Null);
            Assert.IsTrue(response.ComposedResponse.Tree[0].BranchHash.IsHex(64));
        }
    }
}
