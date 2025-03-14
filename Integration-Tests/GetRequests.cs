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
            string pubKey = "6BBE9AF9CCD65F5E438175A8BF0D9AA7C26244679AB99CB1ED83F902662EEC7D";

            var accountClient = new AccountHttp("75.119.150.108", 3000);
            var account = new PublicAccount(pubKey, NetworkType.Types.MAIN_NET);
            var response = await accountClient.GetAccountMerkle(account.Address);

            Assert.That(response.Raw.Length, Is.GreaterThan(0));
            Assert.That(response.Tree[0].Links[0].Link, Is.EqualTo("EAD0C50D2B48CCD225E785568B3A10D84A21BABD2DBCFF5B19D26D6CAA336755"));
            Assert.That(response.Raw, Is.EqualTo("0000FFFFEAD0C50D2B48CCD225E785568B3A10D84A21BABD2DBCFF5B19D26D6CAA3367556FAC70057AAA75495378339400450DB0AFD4200EC2CDD52E1390170805B48CF41673184DC5BB06463F14200F3FC8B9B4AB7B97C3152DEDE06D880DEC1523453F48F8DA1635487005A67CC92DFBC2CF6BD1C1621F630289848AED01792905503668D4F35A7F343E13091BA4E9065E7E2EF9BC07893833D6ABA352B4CA4051FD1BDBFCF5B4A21C151B18AFA78BA845B7B6B3713EF71EEF8698D22227BC3FFCB27D1D7B08BDC3CA8DD981202AB6E4A869EBB21E2EF9D0774399B8AA58E657E2952D4EB35814FA53B2850012EA9C832D6346393E232C902CC3232AA3D0EB0DA560B2C71D0601BEF801651C35A1646397E6F6330E4CE0F40483360807881A311D16F3EF9722650645A766C1DB36E1F09A5C97159C97926887530C057B981583EF93C3178787C6B5668A11833236046C2E8D93D47CFA9412470CCB9F986EE8762630D614267ED83115457B8B2806233E4730EBF0E9054FF15C21DB6BB07D007F14669B84C11A2EB89D61E33DE62917E3F324DFCBE3007E6EDEED67D0E16D89A004E10A62945780FC919C29C35C017BA4B566B5E1CA650F66737AA85E1F210F065A165553C05ADAF53F50849D0D6F47A43D74B8216C7DC747B9FAF809278845963EE5431A81C8300CD544B0BFC9B37A406A074DB4F8E5920653EABC63244815A069FC690000FFFF93725B2495B05066870B3AD486E4AED40426F72A9D4A05511161A0D9E168655C775DC7B741592B7FD59ED2CB94FEF0A3298C628D7C1F482333748552341DC5034EF7993F0C04E532A3EBCF785484935A1DBDE39673A0C3764969F48436C29CC3C7767D143AD4F47B2110408050F5D7799D5D9AC10CE6A8057ED985C9CE1C5B82DDE3C9422E38E4913C824C52AC425547CC3E51058CEF70C7B66A9C85F8B93631428BEBC764CEED2708BAFFA181B80EE4FBCA0CB1162EDFDCCFB1923639A4BA18DB0FB6E71DFB4256231A75C41D2C091836A189D4253154537ABCF3B7C352AE0B3D36C36DC516C78885BD51683857F5B75D2E3DD7F11C718F0F8C889E88BE75CFB5E1D95AB4FB5279CE7F9E2930BA8A0F7025AE1993D1A21B4B3D6016260032F5A63D1E6CAD1B43A93B50B26456562B34BAB2E63C673520FF69D907C97DD9D4D5F458636998EB8A9DE4598A599A0913116DB9E88190E01F62E9EDF839DA26656B172C709F58C27C78E75AFBF9A4ED1E0AAB1FA4B137636F008D4CC7E104A95AC712BCAEB163230DC93916A2061B0E7BB0DF37CBB861E83590510C28E867944CE3E81C8BC4BFCF39778940544B0887AA8B7EB73B42924DE745AEA3416C4FA02859B1D21A0060CF1F45CE866D7714769DCE7E6E63D3182B281999FE26BD8318E0BA17B4244C048CBE99BC44C9A75D342F2DCEBD2C073CF81F80404F5C8DA0BC386F0000FFFFAF5592F15F854D4FA49B9ADA10047F8B12F7018530992B9887C444030BA75DBEECF636D1A729B70FF71F492315F32F5236D7C9F23963EFA4A02C7B743CD1C75530A94F44D31771428C08B444F4BD72FCFC1F00B9CE508C3BF63EC8675E21A9C76AE7BB08D44F90E17631F0EA510C231DD3E3A05741F21604818FCA5289994E01D246E4E33D64969D855B16DF061EC026D91D3C2D6D34DBAD0A16D0C714103EA7629E5DC5D913492FB2FBCE099351332CFEDDE3583512883288ADAC6D01FE699775684230C056F2744C7429151D8883BF68F134C3D81C58E4DEA721EF45BCD665730987F85D2AA7E35558BDAEE909FEEDB8D4C435704D8CF7974E8711F3CDDA986D132B4235AEBF8AEC156B362F761195B841F86B8BEA3B6255376954E3FA11F67EF20FD879396CDEAE382A18DADBA7E402DD985864F68C59AB1E1CE526B3E9B86B0A391912FE88FAE87F25DB74B0459D255D92B9C929DD6BB491CCD81256BC59BE051274DA60B3E51478B0234C5C28E32CC99947804E9C70DDD75F914ED6E27FA0FA0BC6A71804C89B7E89EEB89A5789209F97969F77D1E3D724C47B6EC5C160A08FB251866E2A5FF361BF3BAD08DD56E3CC00A648FF6B67155ECC12980F8ED5DB8B2D797DA3BC0E620EB219A9C9486ADCC1AEC334A40449AE224CFE4C545400CCF9AB816FD782E1481B8DD8F2947FB82CB0F8BF318DE396F1C05A4A73079FA10000FFFF022B7E95B2903BC158C79B81339D92DAC3D739CF1E0DF14A80C343F67CA414F8FC56C36BF8C229378D81745CD554FE471DD3B3CDE169EF532386489F7C0401AE92928C22739A32D1BEA088B20502E32E7FF2160611CB57577423767E7114C3457ADCABAFFC1018C86FE80ED230566A6C913030A66EB4DCA0FCFE79D385D2E44E4C2F6349C8CE9E0C5161F78E9963E8504B1ABB4E80C1ED3142D67BB654945EED6BEED80562625CE5E9B19343CB36575E3178F524ED5ED2F91904DC0D1383DFB8314F333B7A1FDFD930BC2E3C97FD21D4A4A6B8BC8CA21368CE9029FC2F65E4A1C8B6D4D30024F88A191227E3A39A74F18B27A497DE625D328C7891E0E27CE88E89C860125A9FE417F02783934E6BAD385DE2E8408F7FC09A39F668AEE3AEB0C74BB037E4311DD895EEE46F4BBBB72683B12A8B65249B320A1FEFA7F820BD49C6F5CD86323B4B653CC0E1E0B407A66C6EF2ED4C432F3B9BC8DE2A8B1E22D80BCA1A4CEBF0454B5F7F0DD075078FBAEB1503CE284EB01EF391AAE4EE91357A2B7400CA177FC21C62C3D1A462A57CCE60CA90E8D3FDABC697143423F4F09D54E42EE1AA80020B479E5175FE8746A15E39679B81FE92F5C94018248D70F96C555A8E8DEAFD7E92A47D9C9C1FC98688A45CCC6AB08D368A14B0DB5FF3A50B26E21A8317AA2A644A705BF4B727BA908111A58FA26F0E9FBA1A9F4245FAA8027097BC50000020027F8720D86161A24F65939EB9F8B13B39A28FBB400DD6E34CAF06570063ADB0C64260ED6162F14380059EEBD6A3BD93EC6C41ABE9E28FD032C7E520761C12317900000030A15F6E3CE6F24ED4C16173F2982E1C4340D5FAD6188C1D2815044517776AFEBDC0163A0C5F3361A6737F0FF01CB37C94DB81D78BAEFD47F56A001E8B6FB91F68FF3A01D4BC916561C1229AFD3ED1C3DE1C54C1C5CE5F1B5CE8D0E29CDAE315953CC92E2587A39834BF98BF7236D74026F276528722576B8657C41091087B89"));
            Assert.That(response.Tree[0].Type, Is.EqualTo(0));
            Assert.That(response.Tree[0].NibbleCount, Is.EqualTo(0));
            Assert.That(response.Tree[0].Value, Is.Null);
            Assert.That(response.Tree[0].BranchHash, Is.EqualTo("4D3FE6D0F7A7AB3BAFE22C0B273B4E1AB043F6BE8DEF74232C9310F6EC479C67"));
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
