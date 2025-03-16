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
            string pubKey1 = "1799A50301C17D0BA45D2599193B49C4A5377640B3D6695B84F6320466958B5C";
            string pubKey = "D4A1468E54DD31B850CF9ABFFD32EFB98547091301668E777A43D3D88BEB76D8";
            var hashClient = new TransactionHttp("75.119.150.108", 3000);

            var qModel = new QueryModel(QueryModel.DefineRequest.SearchConfirmedTransactions);

            qModel.SetParam(QueryModel.DefinedParams.signerPublicKey, pubKey);

            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.SECRET_LOCK.GetValue());
            qModel.SetParam(QueryModel.DefinedParams.type, TransactionTypes.Types.HASH_LOCK.GetValue());

            var response = await hashClient.SearchConfirmedTransactions(qModel);

            response.ForEach(i => {


                if (i.Transaction.Type.GetValue() == 16722)
                {
                    var tx = ((SecretLockT)i.Transaction);

                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(tx.Secret.Length, Is.GreaterThan(0));
                }
                if (i.Transaction.Type.GetValue() == 16712)
                {
                    var tx = ((HashLockT)i.Transaction);

                    Assert.That(tx.SignerPublicKey, Is.EqualTo(pubKey));
                    Assert.That(tx.Hash.Length, Is.GreaterThan(0));
                }

            });
        }

        [Test, Timeout(20000)]
        public async Task SearchSecreltLocks()
        {
            var nodeClient = new SecretLockHttp("75.119.150.108", 3000);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.SearchSecretLocks(queryModel);

            Assert.That(response[0].Lock.OwnerAddress, Is.EqualTo("6891EA51DEB3ACD82189DB472332231FE67F5A14C1FCF1B6"));
            Assert.That(response[0].Lock.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response[0].Lock.Status, Is.EqualTo(0));
            Assert.That(response[0].Lock.Amount, Is.EqualTo(500000000));
            Assert.That(response[0].Lock.CompositeHash, Is.EqualTo("F38A9D64B472C41FA377F1B1ADD0CA6DF4AC0C5186FBA53D9E95DA72136D4C23"));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretLock()
        {
            var nodeClient = new SecretLockHttp("75.119.150.108", 3000);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.GetSecretLock("F38A9D64B472C41FA377F1B1ADD0CA6DF4AC0C5186FBA53D9E95DA72136D4C23");

            Assert.That(response.Id.Length, Is.GreaterThan(0));
            Assert.That(response.Lock.OwnerAddress, Is.EqualTo("6891EA51DEB3ACD82189DB472332231FE67F5A14C1FCF1B6"));          
            Assert.That(response.Lock.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response.Lock.Status, Is.EqualTo(0));
            Assert.That(response.Lock.Amount, Is.EqualTo(500000000));
            Assert.That(response.Lock.CompositeHash, Is.EqualTo("F38A9D64B472C41FA377F1B1ADD0CA6DF4AC0C5186FBA53D9E95DA72136D4C23"));
            Assert.That(response.Lock.EndHeight, Is.EqualTo(3906515));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretLockMerkle()
        {
            var nodeClient = new SecretLockHttp("75.119.150.108", 3000);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.GetSecretLockMerkle("F38A9D64B472C41FA377F1B1ADD0CA6DF4AC0C5186FBA53D9E95DA72136D4C23");

            Assert.That(response.Raw, Is.EqualTo("0000C662031DC376639F132191FCBF087A505FF4397B5FEBEF68CCC71BA0FD96B656B9D5BB02111A8C5B41BDA054A245E82231D859DBB2C4A73E4321241031AC93376A3C028CFD4711C1EF98A6D4B3ADE46652AB085A0C13BB15575E5760310CFC93927116436B8B2B4BA662BF11E490745B7F391FA550870E61C35DD60D2DB8EED89FEB2902BAA1E433622D16D3298D04C2000D82FFA341426917791AF50E099A47BEAB1D85944FD87449A9881FA4E7120FC3985518CD141D4DF6006452DD91F8A91EEC2C303E96A7C062F0CDB382987F0D5A3037DDB46D25ADB9D8DBEC540EEC5896AAFF3F839F5BA5F704734779F9444A2B5163A215BB73F01C9347BAD59D575F3CBC04406659AEBC7A3B35B3580AC1D1D08582673F44E9787BA6449996B4D72D61033531"));
            Assert.That(response.Tree[1].Type, Is.EqualTo(255));
            Assert.That(response.Tree[1].NibbleCount, Is.EqualTo(63));
            Assert.That(response.Tree[1].LeafHash, Is.EqualTo("028CFD4711C1EF98A6D4B3ADE46652AB085A0C13BB15575E5760310CFC939271"));
            Assert.That(response.Tree[1].BranchHash, Is.Null);
            Assert.That(response.Tree[1].Value, Is.EqualTo("6659AEBC7A3B35B3580AC1D1D08582673F44E9787BA6449996B4D72D61033531"));


        }
    }
}
