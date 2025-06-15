using Integration_Tests;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;
using System.Diagnostics;

namespace test.Model.AccountTest
{
    public class CreateAccount
    {
        [Test]
        public void CreateNewAccount()
        {
            var acc = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

            Assert.AreEqual(64, acc.PublicAccount.PublicKey.Length);
        }

        [Test] 
        public void CreateNewAccountFromKey()
        {
            var acc = Account.CreateFromPrivateKey(HttpSetUp.TestSK, NetworkType.Types.TEST_NET);
            var account = new Account(HttpSetUp.TestSK, NetworkType.Types.TEST_NET);

            Debug.WriteLine(account.PublicKey);
            Assert.AreEqual(64, account.PublicAccount.PublicKey.Length);
            Debug.WriteLine(AddressEncoder.DecodeAddress(account.Address.Plain).ToHexUpper());
            Assert.That(account.Address.Plain, Is.EqualTo("TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA"));
            Debug.WriteLine(account.Address.Plain);      
            Assert.IsTrue(account.Address.Plain == HttpSetUp.TestAddress);         
        }

        [Test]
        public void TestPublicAccount1()
        {
            var pubAccount = new PublicAccount("87C45C6A2C87589786549BAD91568E56822507CA1D85D5B0E86B6F555231A4F8", NetworkType.Types.TEST_NET);
            
            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TCSJY245ZPSF5SSC4OBBKGOLQ3VEPSRIBTVXTTQ"));
            Debug.WriteLine(AddressEncoder.DecodeAddress(pubAccount.Address.Plain).ToHexLower());
            
        }


        [Test]
        public void TestPublicAccount2()
        {
            var pubAccount = new PublicAccount("17D0F0B4DF56A44DA77C38D377D2AE4F4A0BEA320B78D033B377890D318D0DC0", NetworkType.Types.TEST_NET);

            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TAMYTGVH3UEVZRQSD64LGSMPKNTKMASOIDNYROI"));
            Debug.WriteLine(pubAccount.Address.Plain);
        }

        [Test]
        public void TestPublicAccount3()
        {
            var pubAccount = new PublicAccount("F2ABE3C9CBFCC7E1AE9A3142856EA69B6153D7F7540AFC389A99E920072B7C67", NetworkType.Types.TEST_NET);

            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TDSRY7NDJHLDNZVDB64I3VJ5GAJ5UOFNXMNQQZA"));
            Debug.WriteLine(pubAccount.Address.Plain);
        }
    }
}
