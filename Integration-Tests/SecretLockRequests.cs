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

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i => {


                if (i.Transaction.Type.GetValue() == 16722)
                {
                    var tx = ((SecretLockT)i.Transaction);

                    Assert.That(tx.HashAlgorithm, Is.EqualTo(0));
                    Assert.That(tx.MosaicId.Length, Is.EqualTo(16));
                    Assert.That(tx.Duration, Is.GreaterThan(0));
                    Assert.That(tx.Amount, Is.LessThanOrEqualTo(10000000));
                    Assert.That(tx.RecipientAddress.Length, Is.EqualTo(48));
                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey1));
                    Assert.That(tx.Secret.Length, Is.GreaterThan(0));
                }
                if (i.Transaction.Type.GetValue() == 16712)
                {
                    var tx = ((HashLockT)i.Transaction);

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

            Assert.That(response[0].Lock.OwnerAddress, Is.EqualTo("687D93E7E07283BDD3D964F5C85A77A9F2E1C81213693BB9"));
            Assert.That(response[0].Lock.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response[0].Lock.Status, Is.EqualTo(0));
            Assert.That(response[0].Lock.Amount, Is.EqualTo(10000000));
            Assert.That(response[0].Lock.CompositeHash, Is.EqualTo("8A2DED8B62C83A5E67899789BE274F1152EAEE46164C9C5432A1DEF42BEDC30A"));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretLock()
        {
            var nodeClient = new SecretLockHttp(HttpSetUp.Node, HttpSetUp.Port);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.GetSecretLock("EEA42C40275F6705D75E22D73C2B1D19D780EDE2A8E94BF774E704804D32075A");

            Assert.That(response.Id.Length, Is.GreaterThan(0));
            Assert.That(response.Lock.OwnerAddress, Is.EqualTo("68219E09E01CAEA6136339345177DDED5A8027A2F54BBF40"));          
            Assert.That(response.Lock.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response.Lock.Status, Is.EqualTo(0));
            Assert.That(response.Lock.Amount, Is.EqualTo(10000000));
            Assert.That(response.Lock.CompositeHash, Is.EqualTo("EEA42C40275F6705D75E22D73C2B1D19D780EDE2A8E94BF774E704804D32075A"));
            Assert.That(response.Lock.EndHeight, Is.EqualTo(4260267));
            Assert.That(response.Lock.RecipientAddress, Is.EqualTo("68B55EB4DE36860E2953A6D06244A4B1FDAD9025303998B8"));
            Assert.That(response.Lock.Secret, Is.EqualTo("D2B075E66FDD0489B3B71E32B83E5C0A5DFFCA5D4405DD9CC662A27DD0074F0A"));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretLockMerkle()
        {
            var nodeClient = new SecretLockHttp(HttpSetUp.Node, HttpSetUp.Port);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.GetSecretLockMerkle("EEA42C40275F6705D75E22D73C2B1D19D780EDE2A8E94BF774E704804D32075A");

            Assert.That(response.Raw, Is.EqualTo("0000AF323152A69FDEF6E1ADCB108E6867446FCD04DDF3B4398B6F1AA4422F0E61CDDF01F1C0922CA42745F1188D6BBA154349E9F3B71D1BB3F3741CC071193592E78EF85CABF63918136CADA1EA009D849D1CCAB513CE02AE692D025E2B92B359FE5040B7EA4C8BBD8C12D5CBED47C60C34E93586F474F11ED4D0291EE3810285D4D15EA89AD31819A1DADE31DACD278E56B35972E5DCFFE6A4CC5D57C3908CEBB6CC3916436B8B2B4BA662BF11E490745B7F391FA550870E61C35DD60D2DB8EED89FEB1EEF7303B535ECA2E83F8C5D6B7079052B4242CA8891EC5491759B9B3792CAD72A3DDD5CEA5712145B2F8985BB38A852B3AD74890518D574BEF780206FE54D381D85944FD87449A9881FA4E7120FC3985518CD141D4DF6006452DD91F8A91EECFF3FAAB4BCA050BBD498A59E8D32E8A458443AFE32C3B32E14BB9098837F45DCAB5056149D47CD139B40D224CCA2AB5115548112E816D890EEA38D92C7EF4F7B4EF3"));
            Assert.That(response.Tree[0].BranchHash, Is.EqualTo("69A79B4396A841A10E21C3C8F830DC88C8034F0B46CCC3B350D1C3D27B6255B7"));
            Assert.That(response.Tree[0].Links[0].Link, Is.EqualTo("3152A69FDEF6E1ADCB108E6867446FCD04DDF3B4398B6F1AA4422F0E61CDDF01"));

            
            Assert.That(response.Tree[1].Type, Is.EqualTo(255));
            Assert.That(response.Tree[1].NibbleCount, Is.EqualTo(63));
            Assert.That(response.Tree[1].LeafHash, Is.EqualTo("5CABF63918136CADA1EA009D849D1CCAB513CE02AE692D025E2B92B359FE5040"));
            Assert.That(response.Tree[1].BranchHash, Is.Null);
            Assert.That(response.Tree[1].Value, Is.EqualTo("56149D47CD139B40D224CCA2AB5115548112E816D890EEA38D92C7EF4F7B4EF3"));


        }
    }
}
