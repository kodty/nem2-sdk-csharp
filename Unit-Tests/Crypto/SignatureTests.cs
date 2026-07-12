using Integration_Tests;
using Coppery;
using System.Reactive.Linq;
using Unit_Tests.Model.Transactions;
using TweetNaclSharp;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;

namespace Unit_Tests.Crypto
{
    internal class SignatureTests
    {

        [Test, Timeout(20000)]
        public void TestSignVerify()
        {
            var privateKey = "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D";
            var data = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4019854416E1D1AEC42000000B52E115A0200000098E21944E27CE919474CE22D4145725E322766E1A278E414050001000000000072C0212E67A08BCEE80300000000000068656C6C6F";
            var sigVectorData = "23A6CF4C33E70B2B2108D9BB1F6534DCB929002FF3D7ADE5274B45E58760D82AFF79B5F85B027515CAD1AF85CE1F6547C13DAFDD85C285814966DBB950FD710B";

            var keyPair = SecretKeyPair.CreateFromPrivateKey(privateKey);

            var result = keyPair.Sign(data.FromHex());

            Assert.AreEqual(sigVectorData, result.ToHex());

            Assert.IsTrue(data == keyPair.SignOpen(sigVectorData.FromHex().Concat(data.FromHex()).ToArray()).ToHex());
            Assert.IsTrue(keyPair.SignDetachedVerify(data.FromHex(), sigVectorData.FromHex()));
        }

        [Test, Timeout(20000)]
        public async Task TestSignature()
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var account = new Account(HttpSetUp.TestSK, NetworkType.Types.TEST_NET);
            var address = Address.CreateFromEncoded("TDRBSRHCPTURSR2M4IWUCRLSLYZCOZXBUJ4OIFA");

            var factory = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var tx = factory.CreateTransferTransaction(address, PlainMessage.Create("hello"), Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000), false);
            
            var st = tx.WrapVerified(keyPair, HttpSetUp.genHash);

            Assert.True(st.VerifySignature());
        }

        [Test, Timeout(20000)]
        public async Task CosignatureSignatureTest()
        {
            string privKey = "6AA6DAD25D3ACB3385D5643293133936CDDDD7F7E11818771DB1FF2F9D3F9215";
            string signature = "F21E4BE0A914C0C023F724E1EAB9071A3743887BB8824CB170404475873A827B301464261E93700725E8D4427A3E39D365AFB2C9191F75D33C6BE55896E0CC00";
            string data = "E4A92208A6FC52282B620699191EE6FB9CF04DAF48B48FD542C5E43DAA9897763A199AAA4B6F10546109F47AC3564FADE0";
            
            var pair = SecretKeyPair.CreateFromPrivateKey(privKey);
            
            var sig = pair.Sign(data.FromHex());
           
            Assert.AreEqual(sig.ToHex(), signature);
            Assert.True(NaclFast.SignDetachedVerify(data.FromHex(), signature.FromHex(), pair.PublicKey));
        }


        [Test, Timeout(20000)]
        public async Task VectorSignTest()
        {
            string privKey = "ABF4CF55A2B3F742D7543D9CC17F50447B969E6E06F5EA9195D428AB12B7318D";
            string publicKey = "4DB881D07086498C3626F1F84EF89D7E08E5D8293298400F27CA98C92AB2D271";
            string signature = "31D272F0662915CAC43AB7D721CAF65D8601F52B2E793EA1533E7BC20E04EA97B74859D9209A7B18DFECFD2C4A42D6957628F5357E3FB8B87CF6A888BAB4280E";
            string data = "8CE03CD60514233B86789729102EA09E867FC6D964DEA8C2018EF7D0A2E0E24BF7E348E917116690B9";

            var pair = SecretKeyPair.CreateFromPrivateKey(privKey);

            var sig = pair.Sign(data.FromHex());

            Assert.AreEqual(sig.ToHex(), signature);
            Assert.True(NaclFast.SignDetachedVerify(data.FromHex(), signature.FromHex(), pair.PublicKey));
        }
    }
}
