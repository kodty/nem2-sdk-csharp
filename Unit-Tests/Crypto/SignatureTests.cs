using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Model.Accounts;

namespace Unit_Tests.Crypto
{
    internal class SignatureTests
    {
        [Test]
        public void TestSignature()
        {
            string privKey = "C83CE30FCB5B81A51BA58FF827CCBC0142D61C13E2ED39E78E876605DA16D8D7";
            string pubKey = "4CD65AE31B90557EA0F80BCA0748AE1C91C9A1FB53666E8DCCC176774B94E52A";
            int len = 49;
            byte[] data = "A2704638434E9F7340F22D08019C4C8E3DBEE0DF8DD4454A1D70844DE11694F4C8CA67FDCB08FED0CEC9ABB2112B5E5F89".FromHex();
            string signature = "9B4AFBB7B96CAD7726389C2A4F31115940E6EEE3EA29B3293C82EC8C03B9555C183ED1C55CA89A58C17729EFBA76A505C79AA40EC618D83124BC1134B887D305";

            KeyPair keyPair = KeyPair.CreateFromPrivateKey(privKey);


            Assert.That(keyPair.PublicKey.ToHexUpper(), Is.EqualTo(pubKey));
            Assert.That(signature, Is.EqualTo(keyPair.Sign(data).Take(64).ToArray().ToHexUpper()));
        }
    }
}
