using io.nem2.sdk.Model.Transactions.Messages;
using Assert = NUnit.Framework.Assert;

namespace Unit_Tests.Model
{
    public class Message
    {
        [Test]
        public void CanCreateSecureMessage()
        {
            var secureMessage = SecureMessage.Create("Hello", "5949fc564c90ac186cd4f9d2b8298b677bca300b9d8f926ca04e1739e4ed0cba", "2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0");

            var decoded = secureMessage.GetDecodedPayload(
                "5949fc564c90ac186cd4f9d2b8298b677bca300b9d8f926ca04e1739e4ed0cba",
                "2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0");

            Assert.AreEqual("Hello", decoded);
        }
    }
}
