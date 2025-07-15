using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model2;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    internal class HashlockRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task GetHashLock()
        {
            string hash = "526262DEBE21A5A37CBFAF39907AB1C30D34BDD959148181B7EFCA8E67D4CBBF";

            var client = new HashLockHttp(HttpSetUp.Node, HttpSetUp.Port);

            var result = await client.GetHashLockInfo(hash);

            Assert.That(result.Lock.MosaicId, Is.EqualTo("6BED913FA20223F8"));
        }

        [Test, Timeout(20000)]
        public async Task GetHashLockMerkle()
        {
            string hash = "526262DEBE21A5A37CBFAF39907AB1C30D34BDD959148181B7EFCA8E67D4CBBF";

            var client = new HashLockHttp(HttpSetUp.Node, HttpSetUp.Port);

            var result = await client.GetHashLockMerkleInfo(hash);

            Assert.That(result.Tree[0].LeafHash, Is.EqualTo("C1B3FB7531222664A33F5199D2E0328F22F6C04C1717AF47C633E835E9EC90F4"));
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

            response.ForEach(i =>
            {

                var tx = (HashLockT)i.Transaction;

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.Amount, Is.GreaterThan(0));
                Assert.That(i.Meta, !Is.EqualTo(null));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));

            });
        }
    }
}
