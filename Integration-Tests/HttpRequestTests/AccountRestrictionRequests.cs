using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    public class AccountRestrictionRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetAccountAddressRestriction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var restriction = await client.GetConfirmedTransaction("2A5280F16603DCF1544619D87BB0BC367F29C32D3D52C5B659744B7CEE6301A6");
            var tx = (AccountRestriction)restriction.Transaction;

            Assert.That(tx.RestrictionAdditions[0], Is.EqualTo("68E1B300EDBBCE31ED8F922BFDEE477D47061E44CB46CC64"));
            Assert.That(tx.RestrictionDeletions.Count, Is.EqualTo(0));
            Assert.That(tx.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.ADDRESS));
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountOpperationRestriction()
        {
            string pubKey = "9B8534E757F7AD292430FC5EF6ED970D92BA1B93EBF5BB2265864594CCD75E60";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                var tx = (AccountOperationRestriction)i.Transaction;

                Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("9B8534E757F7AD292430FC5EF6ED970D92BA1B93EBF5BB2265864594CCD75E60"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Version, Is.EqualTo(1));
                Assert.That(tx.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.BLOCK));
                Assert.That(tx.RestrictionFlags[1], Is.EqualTo(RestrictionTypes.Types.OUTGOING));
                Assert.That(tx.RestrictionFlags[2], Is.EqualTo(RestrictionTypes.Types.TRANSACTION_TYPE));
                Assert.That(tx.RestrictionAdditions[0], Is.EqualTo(TransactionTypes.Types.AGGREGATE_BONDED));
                Assert.That(tx.RestrictionDeletions.Count, Is.EqualTo(0));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountMosaicRestriction()
        {
            string pubKey = "BD15F73AEC98D613DC7290095B96328A76A0D41324E21F717E67FA2C73074D8D";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response[0].Meta, !Is.EqualTo(null));
            Assert.That(response[0].Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response[0].Id.Length, Is.EqualTo(24));
            Assert.That(response.Count, Is.EqualTo(2));

            var tx1 = (AccountRestriction)response[0].Transaction;

            Assert.That(tx1.SignerPublicKey.Length, Is.GreaterThan(0));
            Assert.That(tx1.SignerPublicKey, Is.EqualTo("BD15F73AEC98D613DC7290095B96328A76A0D41324E21F717E67FA2C73074D8D"));
            Assert.That(tx1.Version, Is.EqualTo(1));
            Assert.That(tx1.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.BLOCK));
            Assert.That(tx1.RestrictionAdditions[0], Is.EqualTo("051FAEC15105C808"));
            Assert.That(tx1.RestrictionDeletions.Count, Is.EqualTo(0));

            var tx2 = (AccountRestriction)response[1].Transaction;

            Assert.That(tx2.SignerPublicKey.Length, Is.GreaterThan(0));
            Assert.That(tx2.SignerPublicKey, Is.EqualTo("BD15F73AEC98D613DC7290095B96328A76A0D41324E21F717E67FA2C73074D8D"));
            Assert.That(tx2.Version, Is.EqualTo(1));
            Assert.That(tx2.RestrictionFlags[0], Is.EqualTo(RestrictionTypes.Types.BLOCK));
            Assert.That(tx2.RestrictionFlags[1], Is.EqualTo(RestrictionTypes.Types.MOSAIC_ID));
            Assert.That(tx2.RestrictionDeletions[0], Is.EqualTo("051FAEC15105C808"));
            Assert.That(tx2.RestrictionAdditions.Count, Is.EqualTo(0));
        }
    }
}
