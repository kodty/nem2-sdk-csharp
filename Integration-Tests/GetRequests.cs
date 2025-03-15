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
        public async Task SearchAccountWithVoting()
        {
            var accHttp = new AccountHttp("75.119.150.108", 3000);

            var response = await accHttp.GetAccount(new PublicAccount("FC8C66547D7C20CD6CBF7F31DC5657247351AF8C12188E56F885FF012431B8C1", NetworkType.Types.MAIN_NET));

            Assert.That(response.Account.Version, Is.EqualTo(1));
            Assert.That(response.Account.AddressHeight, Is.EqualTo(1));
            Assert.That(response.Account.PublicKey, Is.EqualTo("FC8C66547D7C20CD6CBF7F31DC5657247351AF8C12188E56F885FF012431B8C1"));
            Assert.That(response.Account.Address, Is.EqualTo("68AB07874BCFA82C28D84818217EBC49AE0DF421C738161F"));
            Assert.That(response.Account.SupplementalPublicKeys.Voting.PublicKeys[0].PublicKey, Is.EqualTo("E1A8274A61DC5D2A378F5719B1FADB64FBF82120B4B876AEA3774E387C2650FF"));
            Assert.That(response.Account.SupplementalPublicKeys.Voting.PublicKeys[0].StartEpoch, Is.EqualTo(181));
            Assert.That(response.Account.SupplementalPublicKeys.Voting.PublicKeys[0].EndEpoch, Is.EqualTo(357));

            Assert.That(response.Account.Importance, Is.EqualTo(0));
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
                Assert.That(i.Account.PublicKey.Length, Is.GreaterThan(0));
                Assert.That(i.Account.Importance, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Account.AccountType, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Account.AddressHeight, Is.GreaterThan(0));

                Assert.That(i.Account.PublicKey.Length, Is.EqualTo(64));

                if (i.Account.SupplementalPublicKeys != null)
                {
                    if (i.Account.SupplementalPublicKeys.Linked != null)
                    {
                        Assert.That(i.Account.SupplementalPublicKeys.Linked.PublicKey.Length, Is.EqualTo(64));
                    }
                    if (i.Account.SupplementalPublicKeys.Node != null)
                    {
                        Assert.That(i.Account.SupplementalPublicKeys.Node.PublicKey.Length, Is.EqualTo(64));
                    }
                    if (i.Account.SupplementalPublicKeys.Vrf != null)
                    {
                        Assert.That(i.Account.SupplementalPublicKeys.Vrf.PublicKey.Length, Is.EqualTo(64));
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
           
            var accountClient = new AccountHttp("75.119.150.108", 3000);

            var response = await accountClient.GetAccounts(new List<string> { pubKey, "D3D95BFD3E990F418B4CFAD6A67081ECD0AE229000CEC981E380EB0528FD7DE4" });

            response.ForEach(i => {  
                
                Assert.That(i.Account.PublicKey.Length, Is.GreaterThan(0));
                Assert.That(i.Account.Importance, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Account.AccountType, Is.GreaterThanOrEqualTo(0));
                Assert.That(i.Account.AddressHeight, Is.GreaterThan(0));
                            
                Assert.That(i.Account.PublicKey.Length, Is.EqualTo(64));
                
                if(i.Account.SupplementalPublicKeys != null)
                {
                    if(i.Account.SupplementalPublicKeys.Linked != null)
                    {
                        Assert.That(i.Account.SupplementalPublicKeys.Linked.PublicKey.Length, Is.EqualTo(64));
                    }
                    if (i.Account.SupplementalPublicKeys.Node != null)
                    {
                        Assert.That(i.Account.SupplementalPublicKeys.Node.PublicKey.Length, Is.EqualTo(64));
                    }
                    if (i.Account.SupplementalPublicKeys.Vrf != null)
                    {
                        Assert.That(i.Account.SupplementalPublicKeys.Vrf.PublicKey.Length, Is.EqualTo(64));
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

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var account = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);
           
            var response = await accountClient.GetAccount(account.Address);

            Assert.That(response.Id.Length, Is.EqualTo(24));
            Assert.That(response.Account.Version, Is.EqualTo(1));
            Assert.That(response.Account.PublicKeyHeight, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.Account.Address.Length, Is.GreaterThan(0));
            Assert.That(response.Account.Importance, Is.EqualTo(107949223969));
            Assert.That(response.Account.AccountType, Is.GreaterThanOrEqualTo(0));
            Assert.That(response.Account.AddressHeight, Is.GreaterThan(0));
            
            Assert.That(response.Account.PublicKey.Length, Is.GreaterThan(0));
            Assert.That(response.Account.SupplementalPublicKeys, !Is.Null);
            Assert.That(response.Account.SupplementalPublicKeys.Linked, !Is.Null);
            Assert.That(response.Account.SupplementalPublicKeys.Linked.PublicKey.Length, Is.EqualTo(64));
            Assert.That(response.Account.SupplementalPublicKeys.Node, !Is.Null);
            Assert.That(response.Account.SupplementalPublicKeys.Node.PublicKey.Length, Is.EqualTo(64));
            Assert.That(response.Account.SupplementalPublicKeys.Vrf, !Is.Null);
            Assert.That(response.Account.SupplementalPublicKeys.Vrf.PublicKey.Length, Is.EqualTo(64));
            Assert.That(response.Account.SupplementalPublicKeys.Linked.PublicKey.Length, Is.EqualTo(64));
            
            Assert.That(response.Account.ImportanceHeight, Is.GreaterThan(0));

            if (response.Account.Mosaics != null)
            {
                foreach(var item in response.Account.Mosaics)
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

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var account = Address.CreateFromHex("6874CFD2665CD89339B9B74175A7F5A01059A1FEBF0A58A7");
            var response = await accountClient.GetAccountMerkle(account);
            Debug.WriteLine(account.Plain);

            Assert.That(response.Raw.Length, Is.EqualTo(4520));
            Assert.That(response.Raw, Is.EqualTo("0000FFFFEAD0C50D2B48CCD225E785568B3A10D84A21BABD2DBCFF5B19D26D6CAA3367556FAC70057AAA75495378339400450DB0AFD4200EC2CDD52E1390170805B48CF41673184DC5BB06463F14200F3FC8B9B4AB7B97C3152DEDE06D880DEC1523453F48F8DA1635487005A67CC92DFBC2CF6BD1C1621F630289848AED01792905503668D4F35A7F343E13091BA4E9065E7E2EF9BC07893833D6ABA352B4CA4051FD1BDBFCF5B4A21C151B18AFA78BA845B7B6B3713EF71EEF8698D22227BC3FFCB27D1D7B08BDC3CA8DD981202AB6E4A869EBB21E2EF9D0774399B8AA58E657E2952D4EB35814FA53B2850012EA9C832D6346393E232C902CC3232AA3D0EB0DA560B2C71D0601BEF801651C35A1646397E6F6330E4CE0F40483360807881A311D16F3EF9722650645A766C1DB36E1F09A5C97159C97926887530C057B981583EF93C3178787C6B5668A11833236046C2E8D93D47CFA9412470CCB9F986EE8762630D614267ED83115457B8B2806233E4730EBF0E9054FF15C21DB6BB07D007F14669B84C11A2EB89D61E33DE62917E3F324DFCBE3007E6EDEED67D0E16D89A004E10A62945780FC919C29C35C017BA4B566B5E1CA650F66737AA85E1F210F065A165553C05ADAF53F50849D0D6F47A43D74B8216C7DC747B9FAF809278845963EE5431A81C8300CD544B0BFC9B37A406A074DB4F8E5920653EABC63244815A069FC690000FFFF37E4794E62E08B9D9CCBAD1630F19D3314C9ABFCD50E24C2D73646A01CFE9F29A130E07B622318DC0FB827C7877EBB096B9560BAA451942C4519B9603E34732FDE307AFDB292A2B93F215F08B7A8003EE3E3F73E45D02CBD72F4EC110B46F05B0FA1E3B84A8CCD7E7BE9F7A41977CCD30364F446A77F72465A8937ABB9D9BE272B21E63401901D8C93B946D7ECC8647F175C0A121CB5E77BE8927640887FEFA46D8581E42BE8BB89BD5AA00783951BAFACC989B7E16D3BE8392A90AE12D9A4C11B9714371538A80E3182E79698BAFC50121E175D43D1EDA1A829BD001A49F66104AC301574CD88171D16E88B84822FF39EEF7C442E4866E1B4D1905DE69046F9D3E622BC77A616B5ACEB8039DD8D31594949A5251C0C6C4372861E1DDAFD33EF69022DA9475F3F9F508320AD4BA43E832E87E08EB3F796633607A4E0A5AEC1CE95E8D8F24CE5916FE567DDF2FC6C792C53FD1AF55AA2CD632E0954E22D0FD474CFAF217296FED87EE12DC7AB698620EBE40D4292C902720F4FCEC8B9F0894C4104C6B215C8A9F9F46CDC4D3D1F1FD489266AFDC02985CEF4E1B789E63790B83895C405803303863A00B88BF8F1DAAA56FDBDE65476AA57372E46C5C5C72834688C784EBDA0268DC098B57F89FE80CC6A4155056C90A241BAA0EE9B5CE956ED0FADE1C38CC6B0915B56BB0F4FFF4986AD1BDFC87B91B4239451BE5D3F975514150000FFFF1361FBB7E0DA4AC54AAFF2C51B7B773D97933393BE10BBDA01A0D6AB6CF32DF2EE3C061F8F2EFF91E21F87A86E8E9B448A8C118EDF3AB4FF24884FF1C3BD5E91920D35DB1EEC2FEBD60523C5CCA3E9631F5E0EF51F3BFB5742A060E960E6AA289C92849FA9725B7289F12B700558575413D1A471B559B175577C952F97F4C6E5F5E1794D7CD9059B808271161668CE880E75E3DDD2358B43168A9CEE1641470A8F15FA4155888BED0ACE3E691ED506B836F617CC096E59557DE62707326DF4DCFE39380023A2B61D1ED504C757E04F1B801DE11633EF8EF4E8D49AAF0B9145C58F5101BCB7686B7CA6D88AA607912A3970DC3A1C554B3F4B52CB027C9847B73187C5FAD675201952F79274EB9B572670292E3D1F76016C7DD4E441191D3135C1E71DD16B13F6691B121DF2153B0D16401826086AF2D97BF8E9296DB7A600CAFB766F78D6BE25D5AFE9B487E4DF19B48D7913FD1CA171F6F9B7E93482C8A49A1D578BBC4CA0DDE19D8BCF253D3DB9499256FC38795B7950914BA1AE5274681769E98B1840AA5340D69EE786D430A39179B452AF8A3EC4F511FEFF0F34969BFEB46C72DB164C3696BB18B2D0B73E95AD4626FE1D4684F787C8067A0D5C2A8B4EC59FBA6F99C0A2F57BED88D6353980FBFDB4A11DE749F7D3012789DFBC13C32826FAAF1C8AE6311916D8DF23B32E9B387C8DF41F5F7C72F8E741A8F26DD0F0322D0000FFDF4839FFCDD30F613F523362E9A444EFC4347ADC3781323DA587A95CE0E372E889E03BC98F6636145647EC3F66D0E0104249D50465359A7118694CB3C538B79041F934D1DE289D84BDB39AB192E3BE95DC20264A7CCC39F3760009A42CC0D5BF4A345F953A3D28D083637E086FF38B51DE6EF3329711184B4351E0E72811BE0FA0154AA633EC0325AC8FBD84911E4D6149FC9E47A080BC5AA53B0D1E552222C2FC1ED20A6B404BC717C01E7DCC24EF528F42B32910C5D49392A524C369D00CDAA29E8B45D9F0DAE80D9C6C477B6134A6D798F79535CF020A91BC8242550AB5EDB60991BCF0C7DCFFD033FE4960BC53479341B34CA6A7887309E582F5C7C778EDF516134963582AD4648EE8412D76FDB8D5BAC3B07F7DCC48EE8C3FED0658021D5E7C844097B58A63D0CBBC7542FC8D0B194FEE8FBB016C0E5D8B4BB1F29018A64E26E1785366EFD6A96810D83778A13F008F7BE3277E80FEB28D34E668430F980C8DCD6C9E8828E1AF1E13B986A37EBE7E3532386FF7939A2D4B0F19C56A03B43276FD772DC2A4CEFB719AFD66B1AC88528C0FB83AEF44267A61F1EA65D4E84DBC48BB35948036CA89CAD49F72A679FD4A433CBA0C00EAB23BB1DF2C90F595176B4DF9897FC071C5748332F8656A9C41EEB19CB6A24BF4C124504D3B759B922A8A00005218017522145952461F2343947CB94143C14E20E32638E6F8F81F3F228CF0B37E8BD821A0D9028A7C7F18C563AA293F62C36A61D8248B3BDF4F810FEC12C388E4ED66C7D7C77450A453F0AE8768D922ED7376F037FEA7E0FC1F7AEEAD986B715BFC07BFA500F707985620D2989C92E995A92A116493B1037A5E331E283365A5717B68604E6AA5EC7F9B018F1DF3289956F7F5BC0F7F9840792929328D9DC7B0D87EFF3B7FF5A25EE2B9225A1C165B2158703347CF29F4D4B1B6DEA8CEB56BEB76A093C7B1D5A5B2C39A5B234DA141718130692485D338D75B9EB70003B4BF9DC538"));

            response.Tree.ForEach(t =>
            {
                if(t.Links.Count > 0) Assert.That(t.Links[0].Link.Length, Is.EqualTo(64));
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

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var address = Address.CreateFromHex("6808E6F24BFCE08CDBB62AC8AECAB34E573FB46D617D6C28");

            var queryModel = new QueryModel(QueryModel.DefineRequest.SearchAccountRestrictions);

            queryModel.SetParam(QueryModel.DefinedParams.address, address.Plain);
            var response = await accountClient.SearchAccountRestrictions(queryModel);

            Assert.That(response[0].AccountRestrictions.Version, Is.GreaterThan(0));
            Assert.That(response[0].AccountRestrictions.Address.Length, Is.GreaterThan(0));
            Assert.That(response[0].AccountRestrictions.Restrictions[0].RestrictionFlags, Is.GreaterThan(0));
            Assert.That(response[0].AccountRestrictions.Restrictions[0].Values[0], Is.EqualTo("6BED913FA20223F8"));
            Assert.That(Address.CreateFromHex(response[0].AccountRestrictions.Address).Plain, Is.EqualTo("NAEON4SL7TQIZW5WFLEK5SVTJZLT7NDNMF6WYKA"));
        }

        [Test, Timeout(20000)]
        public async Task GetAccountRestrictions()
        {
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var address = Address.CreateFromHex("6808E6F24BFCE08CDBB62AC8AECAB34E573FB46D617D6C28");

            var response = await accountClient.GetAccountRestriction(address.Plain);

            Assert.That(response.AccountRestrictions.Address.Length, Is.GreaterThan(0));
            Assert.That(response.AccountRestrictions.Restrictions[0].RestrictionFlags, Is.GreaterThan(0));
            Assert.That(response.AccountRestrictions.Restrictions[0].Values[0], Is.EqualTo("6BED913FA20223F8"));
            Assert.That(Address.CreateFromHex(response.AccountRestrictions.Address).Plain, Is.EqualTo("NAEON4SL7TQIZW5WFLEK5SVTJZLT7NDNMF6WYKA"));
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
