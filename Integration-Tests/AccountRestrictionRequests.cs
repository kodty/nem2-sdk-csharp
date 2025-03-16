using io.nem2.sdk.Infrastructure.HttpRepositories;
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
    public class AccountRestrictionRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountOpperationRestriction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((AccountOpperationRestriction)i.Transaction);

                Assert.That(tx.Type, Is.EqualTo(TransactionTypes.Types.ACCOUNT_OPERATION_RESTRICTION));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountMosaicRestriction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ACCOUNT_MOSAIC_RESTRICTION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((AccountMosaicRestriction)i.Transaction);

                Assert.That(tx.SignerPublicKey.Length, Is.GreaterThan(0));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountAddressRestriction()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var client = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.MOSAIC_ADDRESS_RESTRICTION.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {

                var tx = ((MosaicAddressRestriction)i.Transaction);

                Assert.That(tx.RestrictionKey.Length, Is.GreaterThan(0));
                Assert.That(tx.SignerPublicKey, Is.EqualTo("6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D"));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(tx.Version, Is.EqualTo(1));
            });
        }
    }
}
