using io.nem2.sdk.Infrastructure.HttpRepositories;
using Coppery;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Diagnostics;
using System.Reactive.Linq;
using io.nem2.sdk.src.Model;

namespace Integration_Tests.HttpRequests
{
    internal class SecretLockRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchSecretLockTransaction()
        {

            string pubKey1 = "38F6ED41900877DF6AA8E70A352EDC9B24504ED88EE5CF9BE5A034D05483DCC3";


            var hashClient = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey1);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.SECRET_LOCK.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.HASH_LOCK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            Assert.That(response.ComposedResponse.Count, Is.GreaterThan(0));

            response.ComposedResponse.ForEach(i =>
            {


                if (i.Transaction.Type == 16722)
                {
                    var tx = (SecretLockT)i.Transaction;

                    Assert.That(tx.HashAlgorithm, Is.EqualTo(0));
                    Assert.That(tx.MosaicId.Length, Is.EqualTo(16));
                    Assert.That(tx.Duration, Is.GreaterThan(0));
                    Assert.That(tx.Amount, Is.LessThanOrEqualTo(10000000));
                    Assert.That(tx.RecipientAddress.Length, Is.EqualTo(48));
                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey1));
                    Assert.That(tx.Secret.Length, Is.GreaterThan(0));
                }
                if (i.Transaction.Type == 16712)
                {
                    var tx = (HashLockT)i.Transaction;

                    Assert.That(tx.MosaicId.Length, Is.EqualTo(16));
                    Assert.That(tx.Duration, Is.LessThan(10000000));
                    Assert.That(tx.Amount, Is.GreaterThan(0));
                    Assert.That(tx.SignerPublicKey.Length, Is.EqualTo(64));
                    Assert.That(tx.Hash.Length, Is.GreaterThan(0));
                }

            });
        }

        [Test, Timeout(20000)]
        public async Task SearchSecreltLocks()
        {
            var nodeClient = new SecretLockHttp(HttpSetUp.Node, HttpSetUp.Port);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.SearchSecretLocks(queryModel);

            Assert.IsTrue(response.ComposedResponse[0].Lock.OwnerAddress.IsHex(48));
            Assert.IsTrue(response.ComposedResponse[0].Lock.MosaicId.IsHex(16));
            Assert.That(response.ComposedResponse[0].Lock.Status, Is.EqualTo(0));
            Assert.That(response.ComposedResponse[0].Lock.Amount, Is.EqualTo(100000000));
            Assert.IsTrue(response.ComposedResponse[0].Lock.CompositeHash.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretLock()
        {
            var nodeClient = new SecretLockHttp(HttpSetUp.Node, HttpSetUp.Port);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.GetSecretLock("8381CE9DCDDB13FB8095C8E0A29DE893D337443822A1AA9DD515644092BD52DA");

            Assert.That(response.ComposedResponse.Id.Length, Is.GreaterThan(0));
            Assert.That(response.ComposedResponse.Lock.OwnerAddress.IsHex(48));
            Assert.IsTrue(response.ComposedResponse.Lock.MosaicId.IsHex(16));
            Assert.That(response.ComposedResponse.Lock.Status, Is.EqualTo(0));
            Assert.That(response.ComposedResponse.Lock.Amount, Is.EqualTo(100000000));
            Assert.IsTrue(response.ComposedResponse.Lock.CompositeHash.IsHex(64));
            Assert.That(response.ComposedResponse.Lock.EndHeight, Is.EqualTo(4611561));
            Assert.IsTrue(response.ComposedResponse.Lock.RecipientAddress.IsHex(48));
            Assert.IsTrue(response.ComposedResponse.Lock.Secret.IsHex(64));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretLockMerkle()
        {
            var nodeClient = new SecretLockHttp(HttpSetUp.Node, HttpSetUp.Port);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.GetSecretLockMerkle("8381CE9DCDDB13FB8095C8E0A29DE893D337443822A1AA9DD515644092BD52DA");

            Assert.That(response.ComposedResponse.Raw.IsHex(396));
            Assert.That(response.ComposedResponse.Tree[0].BranchHash.IsHex(64));
            Assert.That(response.ComposedResponse.Tree[0].Links[0].Link.IsHex(64));
            Assert.That(response.ComposedResponse.Tree[1].Type, Is.EqualTo(255));
            Assert.That(response.ComposedResponse.Tree[1].NibbleCount, Is.EqualTo(63));
            Assert.That(response.ComposedResponse.Tree[1].LeafHash.IsHex(64));
            Assert.That(response.ComposedResponse.Tree[1].BranchHash, Is.Null);
            Assert.That(response.ComposedResponse.Tree[1].Value.IsHex(64));
        }
    }
}
