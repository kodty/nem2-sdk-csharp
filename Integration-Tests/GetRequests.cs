using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
{
    public class AccountRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchAccounts()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp("75.119.150.108", 3000);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchAccounts);
            queryModel.SetParam(QueryModel.DefinedParams.mosaicId, "63078E73FBCC2CAC");  
            var response = await accountClient.SearchAccounts(queryModel);

            response.ForEach(i => {
                Assert.That(i.PublicKey.Length, Is.GreaterThan(0));
               // Assert.That(i.Id.Length, Is.EqualTo(24));
            });
        }

        [Test, Timeout(20000)]
        public async Task GetAccounts()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";
           
            var accountClient = new AccountHttp("75.119.150.108", 3000);

            var response = await accountClient.GetAccounts(new List<string> { pubKey, "D3D95BFD3E990F418B4CFAD6A67081ECD0AE229000CEC981E380EB0528FD7DE4" });

            response.ForEach(i => {              
                Assert.That(i.PublicKey.Length, Is.GreaterThan(0));
               // Assert.That(i.Id.Length, Is.EqualTo(24));  
            });
        }

        [Test, Timeout(20000)]
        public async Task GetAccount()
        {
            string pubKey = "98ECC6FFF03B4AB7C687A27C0DB120E3B0ECDFEAFECC54E10D8AA59409890170";

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var account = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);
            var response = await accountClient.GetAccount(account.Address);
            
            Assert.That(response.Address.Length, Is.GreaterThan(0));
            Assert.That(response.PublicKey.Length, Is.GreaterThan(0));
            Assert.That(response.SupplementalPublicKeys, !Is.Null);
            Assert.That(response.SupplementalPublicKeys.Linked.Length, Is.EqualTo(64));
            Assert.That(response.Mosaics[0].Amount, Is.GreaterThan(0));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountMerkle()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var account = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);
            var response = await accountClient.GetAccountMerkle(account.Address);

            Assert.That(response.Raw.Length, Is.GreaterThan(0));
            Assert.That(response.Tree[0].EncodedPath.Length, Is.EqualTo(2));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountsRestrictions()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var address = Address.CreateFromHex("6808E6F24BFCE08CDBB62AC8AECAB34E573FB46D617D6C28");

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchAccountRestrictions);

            queryModel.SetParam(QueryModel.DefinedParams.address, address.Plain);
            var response = await accountClient.SearchAccountRestrictions(queryModel);
            
            Assert.That(response.Data[0].AccountRestrictions.Address.Length, Is.GreaterThan(0));
            Assert.That(Address.CreateFromHex(response.Data[0].AccountRestrictions.Address).Plain, Is.EqualTo("NAEON4SL7TQIZW5WFLEK5SVTJZLT7NDNMF6WYKA"));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountRestrictions()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var address = Address.CreateFromHex("6808E6F24BFCE08CDBB62AC8AECAB34E573FB46D617D6C28");

            var response = await accountClient.GetAccountRestriction(address.Plain);

            Assert.That(response.AccountRestrictions.Address.Length, Is.GreaterThan(0));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountMerkleInfo()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var account = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);

            var response = await accountClient.GetAccountMerkle(account);

            Assert.That(response.Raw.Length, Is.GreaterThan(0));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountMerkleInfoAddress()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var account = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);

            var response = await accountClient.GetAccountMerkle(account.Address);

            Assert.That(response.Raw.Length, Is.GreaterThan(0));
        }
    }
}
