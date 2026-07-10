using Coppery;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;

namespace test.Model.AccountTest
{

    public class AccountTest
    {
        [Test]
        public void GenerateAccounts()
        {
            for(int x = 0; x < 1000; x++)
            {
                var newAcc = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);


                Assert.That(newAcc.PublicAccount.PublicKey.ToHex().Length, Is.EqualTo(64));
                Assert.That(newAcc.KeyPair.PrivateKeyString.IsHex());
                Assert.That(newAcc.PublicAccount.PublicKey.ToHex().IsHex());
            }
        }

        [Test]
        public void EncodeAddress()
        {
            var address = Address.CreateFromPublicKey("9F780097FB6A1F287ED2736A597B8EA7F08D20F1ECDB9935DE6694ECF1C58900", NetworkType.Types.MAIN_NET);

            Assert.That(address.Plain, Is.EqualTo("NCOXVZMAZJTT4I3F7EAZYGNGR77D6WPTRH6SYIQ"));
        }

        [Test]
        public void CreateNewTestNetAccount()
        {
            var newAcc = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

            Assert.That(newAcc.PublicAccount.PublicKey.ToHex().Length, Is.EqualTo(64));
            Assert.That(newAcc.Address.Plain.IsBase32(), Is.True);
        }
    }
}
