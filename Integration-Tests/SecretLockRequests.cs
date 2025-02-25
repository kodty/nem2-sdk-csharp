using io.nem2.sdk.src.Infrastructure.HttpRepositories;
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
        public async Task SearchSecreltLocks()
        {
            var nodeClient = new SecretLockHttp("75.119.150.108", 3000);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.SearchSecretLocks(queryModel);

            Assert.That(response[0].OwnerAddress, Is.EqualTo("6891EA51DEB3ACD82189DB472332231FE67F5A14C1FCF1B6"));
        }

        [Test, Timeout(20000)]
        public async Task GetSecretLock()
        {
            var nodeClient = new SecretLockHttp("75.119.150.108", 3000);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.GetSecretLock("F38A9D64B472C41FA377F1B1ADD0CA6DF4AC0C5186FBA53D9E95DA72136D4C23");

            Assert.That(response.OwnerAddress, Is.EqualTo("6891EA51DEB3ACD82189DB472332231FE67F5A14C1FCF1B6"));
           
            Assert.That(response.MosaicId, Is.EqualTo("6BED913FA20223F8"));
            Assert.That(response.Status, Is.EqualTo(0));
            Assert.That(response.Amount, Is.EqualTo(500000000));
            Assert.That(response.CompositeHash, Is.EqualTo("F38A9D64B472C41FA377F1B1ADD0CA6DF4AC0C5186FBA53D9E95DA72136D4C23"));
            Assert.That(response.EndHeight, Is.EqualTo(3906515));
           // Assert.That(response.Id, Is.EqualTo("67125FF7FF40591991E48EF6"));

        }

        [Test, Timeout(20000)]
        public async Task GetSecretLockMerkle()
        {
            var nodeClient = new SecretLockHttp("75.119.150.108", 3000);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchSecretLockTransactions);

            var response = await nodeClient.GetSecretLockMerkle("F38A9D64B472C41FA377F1B1ADD0CA6DF4AC0C5186FBA53D9E95DA72136D4C23");

            Assert.That(response.Raw, Is.EqualTo("0000C662031DC376639F132191FCBF087A505FF4397B5FEBEF68CCC71BA0FD96B656B9D5BB02111A8C5B41BDA054A245E82231D859DBB2C4A73E4321241031AC93376A3C028CFD4711C1EF98A6D4B3ADE46652AB085A0C13BB15575E5760310CFC93927116436B8B2B4BA662BF11E490745B7F391FA550870E61C35DD60D2DB8EED89FEB2902BAA1E433622D16D3298D04C2000D82FFA341426917791AF50E099A47BEAB1D85944FD87449A9881FA4E7120FC3985518CD141D4DF6006452DD91F8A91EEC2C303E96A7C062F0CDB382987F0D5A3037DDB46D25ADB9D8DBEC540EEC5896AAFF3F839F5BA5F704734779F9444A2B5163A215BB73F01C9347BAD59D575F3CBC04406659AEBC7A3B35B3580AC1D1D08582673F44E9787BA6449996B4D72D61033531"));
        }
    }
}
