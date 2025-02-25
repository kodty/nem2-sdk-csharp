using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Model.Network;

namespace test.Model.AccountTest
{
    public class CreateAccount
    {
        [Test]
        public void CreateNewAccount()
        {
            var acc = Account.GenerateNewAccount(NetworkType.Types.MIJIN_TEST);

            Assert.AreEqual(64, acc.PublicAccount.PublicKey.Length);

        }

        [Test]
        public void CreateNewAccountFromKey()
        {
            var acc = Account.CreateFromPrivateKey("52b62ec8fafe1d5b7dc2896749f979d5c9ec852a4d37cff9f10656629f4efbf7", NetworkType.Types.MIJIN_TEST);

            //sk        52B62EC8FAFE1D5B7DC2896749F979D5C9EC852A4D37CFF9F10656629F4EFBF7
            //pk        4DDEC6FB920947C1765CF461525923B1A4FC94545BA360735EE7111ABBC98577
            //address   SDAPAHH2YJT5TGY6UVXOA5YS2ZIWLT5NDNQKEVQY
          
            Assert.AreEqual(64, acc.PublicAccount.PublicKey.Length);
            Assert.IsTrue(acc.Address.Plain.Length == 40);
            
        }
    }
}
