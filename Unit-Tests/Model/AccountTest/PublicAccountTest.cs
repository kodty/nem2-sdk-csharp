using io.nem2.sdk.src.Model2;
using io.nem2.sdk.src.Model2.Accounts;

namespace test.Model.AccountTest
{

    public class PublicAccountTest
    {
        private readonly string publicKey = "4DDEC6FB920947C1765CF461525923B1A4FC94545BA360735EE7111ABBC98577";

        [Test]
        public void EqualityIsBasedOnPublicKeyAndNetwork()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.MIJIN_TEST);
            var publicAccount2 = new PublicAccount(publicKey, NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual(publicAccount.Address.Pretty, publicAccount2.Address.Pretty);
        }

        [Test]
        public void EqualityReturnsFalseIfNetworkIsDifferent()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.MIJIN_TEST);
            var publicAccount2 = new PublicAccount(publicKey, NetworkType.Types.MAIN_NET);
            Assert.AreNotEqual(publicAccount.Address.Plain, publicAccount2.Address.Plain);
            Assert.AreNotEqual(publicAccount.Address.Pretty, publicAccount2.Address.Pretty);
            Assert.AreNotEqual(publicAccount.Address.NetworkByte, publicAccount2.Address.NetworkByte);
        }
    }
}
