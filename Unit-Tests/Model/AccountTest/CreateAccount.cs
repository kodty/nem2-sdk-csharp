using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Model.Network;

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
            var acc = Account.CreateFromPrivateKey("", NetworkType.Types.TEST_NET);

            Assert.AreEqual(64, acc.PublicAccount.PublicKey.Length);
            Assert.IsTrue(acc.Address.Plain.Length == 39);         
        }
    }
}
