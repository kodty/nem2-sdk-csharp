using Coppery;
using System.Diagnostics;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;

namespace test.Model.AccountTest
{
    public class CreateAccount
    {
        [Test] 
        public void CreateNewAccountFromKey()
        {
            var acc = Account.CreateFromPrivateKey("575DBB3062267EFF57C970A336EBBC8FBCFE12C5BD3ED7BC11EB0481D7704CED", NetworkType.Types.TEST_NET);

            Assert.AreEqual("2E834140FD66CF87B254A693A2C7862C819217B676D3943267156625E816EC6F", acc.PublicAccount.PublicKeyString);             
        }

        [Test]
        public void TestPublicAccount1()
        {
            var pubAccount = new PublicAccount("87C45C6A2C87589786549BAD91568E56822507CA1D85D5B0E86B6F555231A4F8", NetworkType.Types.TEST_NET);
            
            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TCSJY245ZPSF5SSC4OBBKGOLQ3VEPSRIBTVXTTQ"));            
        }


        [Test]
        public void TestPublicAccount2()
        {
            var pubAccount = new PublicAccount("17D0F0B4DF56A44DA77C38D377D2AE4F4A0BEA320B78D033B377890D318D0DC0", NetworkType.Types.TEST_NET);

            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TAMYTGVH3UEVZRQSD64LGSMPKNTKMASOIDNYROI"));
        }

        [Test]
        public void TestPublicAccount3()
        {
            var pubAccount = new PublicAccount("F2ABE3C9CBFCC7E1AE9A3142856EA69B6153D7F7540AFC389A99E920072B7C67", NetworkType.Types.TEST_NET);

            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TDSRY7NDJHLDNZVDB64I3VJ5GAJ5UOFNXMNQQZA"));
        }
    }
}
