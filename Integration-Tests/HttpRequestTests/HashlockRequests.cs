using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using io.nem2.sdk.Infrastructure.HttpClients;
using System.Reactive.Linq;
using Coppery;

namespace Integration_Tests.HttpRequestTests
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
            string hash = "FD492A6AD4BA0A2CD73277C4390BFCA885C17693DD6463F4418D0A6553A586D3";

            var client = new LockHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var result = await client.GetHashLockInfo(hash);

            Assert.That(result.ComposedResponse.Lock.MosaicId, Is.EqualTo("72C0212E67A08BCE"));
        }

        [Test, Timeout(20000)]
        public async Task GetHashLockMerkle()
        {
            string hash = "FD492A6AD4BA0A2CD73277C4390BFCA885C17693DD6463F4418D0A6553A586D3";

            var client = new LockHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var result = await client.GetHashLockMerkleInfo(hash);

            Assert.That(result.ComposedResponse.Tree[0].LeafHash, Is.EqualTo("80812C6A76AB011E217DF970C19356CFF53A6C4CF1D90767D5070C54E062B7AB"));
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

            Assert.That(response.ComposedResponse.Data.Count, Is.GreaterThan(0));

            response.ComposedResponse.Data.ForEach(i =>
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
