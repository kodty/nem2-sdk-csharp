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
    internal class HashlockRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchHashLockTransaction()
        {
            string pubKey = "1799A50301C17D0BA45D2599193B49C4A5377640B3D6695B84F6320466958B5C";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.HASH_LOCK.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i => {

                var tx = ((HashLockT)i.Transaction);

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Amount, Is.GreaterThan(0));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));

            });
        }       
    }
}
