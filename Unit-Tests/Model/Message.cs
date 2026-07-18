using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions.Messages;
using System.Text.Json.Nodes;

namespace Unit_Tests.Model
{
    public class Message
    {
        [Test]
        public void CanCreateSharedKey()
        {
            var text = File.ReadAllLines(HttpSetUp.VectorsPath + "3.test-derive-hkdf.json");

            var keys = JsonObject.Parse(String.Concat(text));

            for (var i = 0; i < 1000; i++)
            {
                string sk = keys[i]["privateKey"].GetValue<string>();
                string pk = keys[i]["otherPublicKey"].GetValue<string>();
                string sharedKey = keys[i]["sharedKey"].GetValue<string>();
                string scalarMultResult = keys[i]["scalarMulResult"].GetValue<string>();
               
                Assert.AreEqual(SecureMessage.HKDFDeriveSharedKey256(sk.FromHex(), pk.FromHex()).ToHex(), sharedKey);
            }
        }

        [Test]
        public void CanCreateSecureMessage()
        {
            var secureMessage = SecureMessage.Create(
                    msg: "Hello", 
                    senderPrivateKey: "5949fc564c90ac186cd4f9d2b8298b677bca300b9d8f926ca04e1739e4ed0cba", 
                    receiverPublicKey: "2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0"
                );
            
            var decoded = secureMessage.GetDecodedPayload(
                    privateKey: "5949fc564c90ac186cd4f9d2b8298b677bca300b9d8f926ca04e1739e4ed0cba",
                    publicKey: "2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0"
                );
            
            Assert.AreEqual("Hello", decoded);
            
            for (int x = 0; x < 100; x++)
            {
                var keys1 = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);
                var keys2 = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);
            
                secureMessage = SecureMessage.Create(
                    msg: "Hello", 
                    senderPrivateKey: keys1.KeyPair.PrivateKeyString, 
                    receiverPublicKey: keys2.KeyPair.PublicKeyString);
            
                decoded = secureMessage.GetDecodedPayload(
                    privateKey: keys1.KeyPair.PrivateKeyString,
                    publicKey: keys2.KeyPair.PublicKeyString);
            
                Assert.AreEqual("Hello", decoded);
            }
        }
    }
}
