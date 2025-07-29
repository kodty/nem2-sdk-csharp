using io.nem2.sdk.Infrastructure.HttpRepositories;
using CopperCurve;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;
using io.nem2.sdk.src.Model2;
using io.nem2.sdk.src.Model2.Accounts;

namespace Integration_Tests.HttpRequests.AccountHttpTests
{
    public class AccountRequests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Timeout(20000)]
        public async Task SearchAccountWithVoting()
        {
            var accHttp = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await accHttp.GetAccount("FC8C66547D7C20CD6CBF7F31DC5657247351AF8C12188E56F885FF012431B8C1");

            Assert.That(response.Account.Version, Is.EqualTo(1));
            Assert.That(response.Account.AddressHeight, Is.EqualTo(1));
            Assert.IsTrue(response.Account.PublicKey.IsHex(64));
            Assert.IsTrue(response.Account.Address.IsHex(48));
            Assert.IsTrue(response.Account.SupplementalPublicKeys.Voting.PublicKeys[0].PublicKey.IsHex(64));
            Assert.That(response.Account.SupplementalPublicKeys.Voting.PublicKeys[0].StartEpoch, Is.EqualTo(181));
            Assert.That(response.Account.SupplementalPublicKeys.Voting.PublicKeys[0].EndEpoch, Is.EqualTo(357));
            Assert.That(response.Account.Importance, Is.EqualTo(0));
        }

        [Test, Timeout(20000)]
        public async Task SearchAccounts()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);

            QueryModel queryModel = new QueryModel(QueryModel.DefineRequest.SearchAccounts);
            queryModel.SetParam(QueryModel.DefinedParams.mosaicId, "63078E73FBCC2CAC");
            var response = await accountClient.SearchAccounts(queryModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {
                Assert.IsTrue(i.Account.PublicKey.IsHex(64));
                Assert.That(i.Account.Importance, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Account.AccountType, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Account.AddressHeight, Is.GreaterThan(0));

                Assert.IsTrue(i.Account.PublicKey.IsHex(64));

                if (i.Account.SupplementalPublicKeys != null)
                {
                    if (i.Account.SupplementalPublicKeys.Linked != null)
                    {
                        Assert.IsTrue(i.Account.SupplementalPublicKeys.Linked.PublicKey.IsHex(64));
                    }
                    if (i.Account.SupplementalPublicKeys.Node != null)
                    {
                        Assert.IsTrue(i.Account.SupplementalPublicKeys.Node.PublicKey.IsHex(64));
                    }
                    if (i.Account.SupplementalPublicKeys.Vrf != null)
                    {
                        Assert.IsTrue(i.Account.SupplementalPublicKeys.Vrf.PublicKey.IsHex(64));
                    }
                }
                if (i.Account.Mosaics != null)
                {
                    i.Account.Mosaics.ForEach(m => Assert.That(m.Amount, Is.GreaterThan(0)));

                }
            });
        }

        [Test, Timeout(20000)]
        public async Task GetAccounts()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);

            var response = await accountClient.GetAccounts(new List<string> { pubKey, "D3D95BFD3E990F418B4CFAD6A67081ECD0AE229000CEC981E380EB0528FD7DE4" });

            Assert.That(response.Count, Is.GreaterThan(0));

            response.ForEach(i =>
            {

                Assert.IsTrue(i.Account.PublicKey.IsHex(64));
                Assert.That(i.Account.Importance, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Account.AccountType, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Account.AddressHeight, Is.GreaterThan(0));

                Assert.IsTrue(i.Account.PublicKey.IsHex(64));

                if (i.Account.SupplementalPublicKeys != null)
                {
                    if (i.Account.SupplementalPublicKeys.Linked != null)
                    {
                        Assert.IsTrue(i.Account.SupplementalPublicKeys.Linked.PublicKey.IsHex(64));
                    }
                    if (i.Account.SupplementalPublicKeys.Node != null)
                    {
                        Assert.IsTrue(i.Account.SupplementalPublicKeys.Node.PublicKey.IsHex(64));
                    }
                    if (i.Account.SupplementalPublicKeys.Vrf != null)
                    {
                        Assert.IsTrue(i.Account.SupplementalPublicKeys.Vrf.PublicKey.IsHex(64));
                    }
                }
                if (i.Account.Mosaics != null)
                {
                    i.Account.Mosaics.ForEach(m => Assert.That(m.Amount, Is.GreaterThan(0)));

                }
            });
        }

        [Test, Timeout(20000)]
        public async Task GetAccount()
        {
            string pubKey = "592CE2CCDBFA0A97CDF3657983397BB915224273FA6038617BA848D77ED68FA7";

            var accountClient = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);
            var account = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);

            var response = await accountClient.GetAccount(account.Address.Plain);

            Assert.IsTrue(response.Id.IsHex(24));
            Assert.That(response.Account.Version, Is.EqualTo(1));
            Assert.That(response.Account.PublicKeyHeight, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.Account.Address.IsHex(48));
            Assert.That(response.Account.Importance, Is.GreaterThan(1));
            Assert.That(response.Account.AccountType, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.Account.AddressHeight, Is.GreaterThan(0));

            Assert.IsTrue(response.Account.PublicKey.IsHex(64));
            Assert.That(response.Account.SupplementalPublicKeys, !Is.Null);
            Assert.IsTrue(response.Account.SupplementalPublicKeys.Linked.PublicKey.IsHex(64));
            Assert.IsTrue(response.Account.SupplementalPublicKeys.Node.PublicKey.IsHex(64));
            Assert.IsTrue(response.Account.SupplementalPublicKeys.Vrf.PublicKey.IsHex(64));


            Assert.That(response.Account.ImportanceHeight, Is.GreaterThan(0));

            if (response.Account.Mosaics != null)
            {
                foreach (var item in response.Account.Mosaics)
                {
                    Assert.That(item.Amount, Is.GreaterThan(0));
                    Assert.That(item.Id.Length, Is.EqualTo(16));
                }
            }
            if (response.Account.ActivityBuckets != null)
            {
                foreach (var item in response.Account.ActivityBuckets)
                {
                    Assert.That(item.TotalFeesPaid, Is.GreaterThanOrEqualTo(0));
                    Assert.That(item.RawScore, Is.GreaterThanOrEqualTo(0));
                    Assert.That(item.BeneficiaryCount, Is.GreaterThanOrEqualTo(0));
                    Assert.That(item.StartHeight, Is.GreaterThanOrEqualTo(0));
                }

            }
        }

        [Test, Timeout(20000)]
        public async Task GetAccountMerkle()
        {
            string pubKey = "6874CFD2665CD89339B9B74175A7F5A01059A1FEBF0A58A7";

            var accountClient = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);
            var account = Address.CreateFromHex("6874CFD2665CD89339B9B74175A7F5A01059A1FEBF0A58A7");
            var response = await accountClient.GetAccountMerkle(account.Plain);

            Assert.That(response.Raw.Length, Is.EqualTo(4584));

            response.Tree.ForEach(t =>
            {
                if (t.Links.Count > 0) Assert.That(t.Links[0].Link.Length, Is.EqualTo(64));
                if (t.Links.Count > 0) Assert.That(t.Links[0].Bit.Length, Is.EqualTo(1));
                Assert.That(t.Type, Is.GreaterThanOrEqualTo(0));
                if (t.Path != null) Assert.That(t.Path.Length, Is.AnyOf(60, 0));
                Assert.That(t.EncodedPath.Length, Is.GreaterThan(0));
                if (t.LinkMask != null) Assert.That(t.LinkMask.Length, Is.EqualTo(4));
                Assert.That(t.NibbleCount, Is.GreaterThanOrEqualTo(0));
                if (t.Value != null) Assert.That(t.Value.Length, Is.EqualTo(64));
                if (t.BranchHash != null) Assert.That(t.BranchHash.Length, Is.EqualTo(64));
            });

        }

        [Test, Timeout(20000)]
        public async Task GetAccountsRestrictions()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);
            var address = Address.CreateFromHex("6808E6F24BFCE08CDBB62AC8AECAB34E573FB46D617D6C28");

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchAccountRestrictions);

            queryModel.SetParam(QueryModel.DefinedParams.address, address.Plain);
            var response = await accountClient.SearchAccountRestrictions(queryModel);

            Assert.That(response.Count, Is.GreaterThan(0));

            foreach (var item in response)
            {
                Assert.That(item.AccountRestrictions.Version, Is.GreaterThan(0));
                Assert.That(item.AccountRestrictions.Address.Length, Is.GreaterThan(0));
                Assert.That(item.AccountRestrictions.Restrictions[0].RestrictionFlags.ExtractRestrictionFlags()[0], Is.GreaterThan(RestrictionTypes.Types.ADDRESS));
                Assert.That(item.AccountRestrictions.Restrictions[0].Values[0].Length, Is.EqualTo(16));
                Assert.IsTrue(Address.CreateFromHex(item.AccountRestrictions.Address).Plain.IsBase32(39));
            }
        }

        [Test, Timeout(20000)]
        public async Task GetAccountMosaicRestriction()
        {
            var client = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);

            var acc = new PublicAccount("C807BE28855D0C87A8A2C032E51790CCB9158C15CBACB8B222E27DFFFEB3697D", NetworkType.Types.MAIN_NET);

            var restriction = await client.GetAccountRestriction(acc.Address.Plain);

            Assert.IsTrue(Address.CreateFromHex(restriction.AccountRestrictions.Address).Plain.IsBase32(39));
            Assert.That(restriction.AccountRestrictions.Restrictions[0].RestrictionFlags.ExtractRestrictionFlags()[0], Is.EqualTo(RestrictionTypes.Types.MOSAIC_ID));
            Assert.IsTrue(restriction.AccountRestrictions.Restrictions[0].Values[0].IsHex(16));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountRestrictions()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);
            var address = Address.CreateFromHex("6808E6F24BFCE08CDBB62AC8AECAB34E573FB46D617D6C28");

            var response = await accountClient.GetAccountRestriction(address.Plain);

            Assert.That(response.AccountRestrictions.Address.Length, Is.GreaterThan(0));
            Assert.That(response.AccountRestrictions.Restrictions[0].RestrictionFlags.ExtractRestrictionFlags()[0], Is.GreaterThan(RestrictionTypes.Types.ADDRESS));
            Assert.That(response.AccountRestrictions.Restrictions[0].Values[0], Is.EqualTo("6BED913FA20223F8"));
            Assert.That(Address.CreateFromHex(response.AccountRestrictions.Address).Plain, Is.EqualTo("NAEON4SL7TQIZW5WFLEK5SVTJZLT7NDNMF6WYKA"));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountRestrictionMerkle()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp(HttpSetUp.Node, HttpSetUp.Port);
            var account = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);

            var response = await accountClient.GetAccountRestrictionsMerkle(account.Address.Plain);

            Assert.That(response.Raw.Length, Is.GreaterThan(0));
            Assert.IsTrue(response.Tree[0].Links[0].Link.IsHex(64));
            Assert.That(response.Tree[0].Type, Is.EqualTo(0));
            Assert.That(response.Tree[0].NibbleCount, Is.EqualTo(0));
            Assert.That(response.Tree[0].Value, Is.Null);
            Assert.IsTrue(response.Tree[0].BranchHash.IsHex(64));
        }
    }
}
