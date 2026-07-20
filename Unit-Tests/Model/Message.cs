using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions.Messages;
using System.Security.Cryptography;
using System.Text.Json.Nodes;

namespace Unit_Tests.Model
{
    public class Message
    {
        [Test]
        public void CanEncodeCipher()
        {
            var text = File.ReadAllLines(HttpSetUp.VectorsPath + "4.test-cipher.json");

            var keys = JsonObject.Parse(String.Concat(text));

            for (var i = 0; i < keys.AsArray().Count; i++)
            {
                byte[] sk = keys[i]["privateKey"].GetValue<string>().FromHex();
                byte[] pk = keys[i]["otherPublicKey"].GetValue<string>().FromHex();
                byte[] iv = keys[i]["iv"].GetValue<string>().FromHex();
                byte[] cipherText = keys[i]["cipherText"].GetValue<string>().FromHex();
                string clearText = keys[i]["clearText"].GetValue<string>();

                // encrypt & decrypt using sender primary shared key
                byte[] scalarResult = SecureMessage.DeriveSharedKey(sk, pk);
               
                byte[] HKDF_key = SecureMessage.HKDF_Derive(scalarResult);

                byte[] cipherSivResult = SecureMessage.AesGcmSivEncryptor_(iv, HKDF_key, clearText, keys[i]["tag"].GetValue<string>().FromHex(), null);

                byte[] decryptedSiv = SecureMessage.AesGcmSivDecryptor_(iv, HKDF_key, cipherSivResult, keys[i]["tag"].GetValue<string>().FromHex());

                Assert.AreEqual(clearText, decryptedSiv.ToHex());

                // encrypt using sender primary shared key, decrypt using receiver primary shared key
                var sender = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

                var receiver = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

                byte[] senderPrimaryScalarResult = SecureMessage.DeriveSharedKey(sender.KeyPair.PrivateKey, receiver.KeyPair.PublicKey);

                byte[] senderPrimaryHKDF_key = SecureMessage.HKDF_Derive(senderPrimaryScalarResult);

                byte[] receiverPrimaryScalarResult = SecureMessage.DeriveSharedKey(receiver.KeyPair.PrivateKey, sender.KeyPair.PublicKey);

                byte[] receiverPrimaryHKDF_key = SecureMessage.HKDF_Derive(receiverPrimaryScalarResult);

                byte[] senderPrimaryCipherSivResult = SecureMessage.AesGcmSivEncryptor_(iv, senderPrimaryHKDF_key, clearText, keys[i]["tag"].GetValue<string>().FromHex(), null);

                byte[] receiverPrimaryDecryptedSiv = SecureMessage.AesGcmSivDecryptor_(iv, receiverPrimaryHKDF_key, senderPrimaryCipherSivResult, keys[i]["tag"].GetValue<string>().FromHex());

                Assert.AreEqual(clearText, receiverPrimaryDecryptedSiv.ToHex());
            }
        }

        [Test]
        public void CanCreateSharedKey()
        {
            var text = File.ReadAllLines(HttpSetUp.VectorsPath + "3.test-derive-hkdf.json");

            var keys = JsonObject.Parse(String.Concat(text));

            for (var i = 0; i < keys.AsArray().Count; i++)
            {
                string sk = keys[i]["privateKey"].GetValue<string>();
                string pk = keys[i]["otherPublicKey"].GetValue<string>();
                string sharedKey = keys[i]["sharedKey"].GetValue<string>();
                string scalarMultResult = keys[i]["scalarMulResult"].GetValue<string>();

                byte[] scalarResult = SecureMessage.DeriveSharedKey(sk.FromHex(), pk.FromHex());

                byte[] HKDF_key = SecureMessage.HKDF_Derive(scalarResult, System.Text.Encoding.UTF8.GetBytes("catapult"));

                Assert.AreEqual(scalarResult.ToHex(), scalarMultResult);
                Assert.AreEqual(HKDF_key.ToHex(), sharedKey);
            }
        }

        [Test]
        public void CanCreateSecureMessage()
        {
            var secureMessage = SecureMessage.Create(
                    msg: "Hello", 
                    senderPrivateKey: "5949fc564c90ac186cd4f9d2b8298b677bca300b9d8f926ca04e1739e4ed0cba", 
                    receiverPublicKey: "2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0",
                    System.Text.Encoding.UTF8.GetBytes("catapult"));
            
            var decoded = secureMessage.GetDecodedPayload(
                    privateKey: "5949fc564c90ac186cd4f9d2b8298b677bca300b9d8f926ca04e1739e4ed0cba",
                    publicKey: "2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0",
                    System.Text.Encoding.UTF8.GetBytes("catapult"));
            
            Assert.AreEqual("Hello", decoded);
            
            for (int x = 0; x < 100; x++)
            {
                var sender = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);
                var receiver = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

                string msg = "";

                using (var ng = RandomNumberGenerator.Create())
                {
                    byte[] data = new byte[17];
                    
                    ng.GetNonZeroBytes(data);

                    msg = data.ToHex();
                }

                secureMessage = SecureMessage.Create(
                    msg: msg, 
                    senderPrivateKey: sender.KeyPair.PrivateKeyString, 
                    receiverPublicKey: receiver.KeyPair.PublicKeyString,
                    System.Text.Encoding.UTF8.GetBytes("catapult"));
            
                decoded = secureMessage.GetDecodedPayload(
                    privateKey: receiver.KeyPair.PrivateKeyString,
                    publicKey: sender.KeyPair.PublicKeyString,
                    System.Text.Encoding.UTF8.GetBytes("catapult"));

                Assert.That(sender.KeyPair.PrivateKey.ToHex() != receiver.KeyPair.PrivateKey.ToHex());
                Assert.AreEqual(msg, decoded);

                var secureMessageWithoutInfo = SecureMessage.Create(
                    msg: msg,
                    senderPrivateKey: sender.KeyPair.PrivateKeyString,
                    receiverPublicKey: receiver.KeyPair.PublicKeyString);

                var decodedWithoutInfo = secureMessageWithoutInfo.GetDecodedPayload(
                    privateKey: receiver.KeyPair.PrivateKeyString,
                    publicKey: sender.KeyPair.PublicKeyString);

                Assert.That(sender.KeyPair.PrivateKey.ToHex() != receiver.KeyPair.PrivateKey.ToHex());
                Assert.AreEqual(msg, decodedWithoutInfo);
            }
        }
    }
}
