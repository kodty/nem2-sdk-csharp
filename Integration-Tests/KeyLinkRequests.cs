using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
{
    public class KeyLinkRequests
    {
        [SetUp]
        public async Task SetUp()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.ACCOUNT_KEY_LINK.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i => {

                var tx = ((KeyLink)i.Transaction);

                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
                Assert.That(tx.SignerPublicKey.Length, Is.EqualTo(64));
                Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchNodeKeyLinkTransaction()
        {
            string pubKey = "0B349D6FB4E93FAB29065D51B7A5375FFAF3856BA7F64DDE66B86579816D6E77";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.NODE_KEY_LINK.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i => {

                var tx = ((KeyLink)i.Transaction);

                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchVotingKeyLinkTransaction()
        {
            string pubKey = "AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.VOTING_KEY_LINK.GetValue());

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i => {

                var tx = ((VotingKeyLink)i.Transaction);

                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
                Assert.That(tx.StartEpoch, Is.EqualTo(1));
                Assert.That(tx.EndEpoch, Is.EqualTo(180));
            });
        }

        [Test, Timeout(20000)]
        public async Task SearchVotingKeyLinkTransaction2()
        {
            string pubKey = "B26D01FC006EAC09B740A3C8F12C1055AE24AFD3268F0364C92D51800FC07361";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("55EB9659C81600F1760C4C0A4F8A7A5C90A39FCEE36E3165143B8E72BBC709E8");

            var agg = (Aggregate)response.Transaction;

            var EmbeddedKeyLink = (EmbeddedVotingKeyLink)agg.Transactions[4].Transaction;

            Assert.That(EmbeddedKeyLink.SignerPublicKey, Is.EqualTo(pubKey));
            Assert.That(EmbeddedKeyLink.LinkedPublicKey.Length, Is.EqualTo(64));
            Assert.That(EmbeddedKeyLink.StartEpoch, Is.EqualTo(2));
            Assert.That(EmbeddedKeyLink.EndEpoch, Is.EqualTo(361));
            
        }

        [Test, Timeout(20000)]
        public async Task SearchVRFKeyLinkTransaction()
        {
            string pubKey = "AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF";

            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);
            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.VRF_KEY_LINK.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.height, 1);

            var response = await client.SearchConfirmedTransactions(qModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i => {

                var tx = ((KeyLink)i.Transaction);

                Assert.That(i.Id.Length, Is.EqualTo(24));
                Assert.That(i.Meta.Hash.Length, Is.EqualTo(64));
                Assert.That(i.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
                Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetVotingKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("901807C96B582AACC140BE64CE3C18AF754E3DFBD2269AC573A5121097005DF8");

            var tx = (VotingKeyLink)response.Transaction;

            Assert.That(response.Id.Length, Is.EqualTo(24));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF"));
            Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
            Assert.That(tx.StartEpoch, Is.EqualTo(1));
            Assert.That(tx.EndEpoch, Is.EqualTo(180));
        }

        [Test, Timeout(20000)]
        public async Task GetNodeKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("F6A12DDA59412CF3A74D558E631FF6C6F5E2B43620CDC950698BBD17FF8F0B57");

            var tx = ((KeyLink)response.Transaction);

            Assert.That(response.Id.Length, Is.EqualTo(24));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("0B349D6FB4E93FAB29065D51B7A5375FFAF3856BA7F64DDE66B86579816D6E77"));
            Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
        }

        [Test, Timeout(20000)]
        public async Task GetVRFKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("901807C96B582AACC140BE64CE3C18AF754E3DFBD2269AC573A5121097005DF8");

            var tx = ((KeyLink)response.Transaction);

            Assert.That(response.Id.Length, Is.EqualTo(24));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("AFF16052217A847A6A71B326FEA9073CFF70D07FC5BA9026B3E05FB453C950DF"));
            Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountKeyLinkTransaction()
        {
            var client = new TransactionHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await client.GetConfirmedTransaction("5C27AD1B777E811946DDB9AB5ABCC464C7B80CCE77CDA3870EB19F1DD1AF22BA");

            var tx = ((KeyLink)response.Transaction);

            Assert.That(response.Id.Length, Is.EqualTo(24));
            Assert.That(response.Meta.Hash.Length, Is.EqualTo(64));
            Assert.That(response.Meta.MerkleComponentHash.Length, Is.EqualTo(64));
            Assert.That(tx.SignerPublicKey, Is.EqualTo("9261DB223A28A3DB05315235DF2186260951B66515B17A6B890BBCE3EE9E3FE7"));
            Assert.That(tx.LinkedPublicKey.Length, Is.EqualTo(64));
        }
    }
}
