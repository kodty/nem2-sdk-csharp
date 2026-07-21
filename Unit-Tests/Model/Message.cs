using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions.Messages;
using Org.BouncyCastle.Security;
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

                byte[] cipherSivResult = SecureMessage.AesGcmSivEncryptor(iv, HKDF_key, System.Text.Encoding.UTF8.GetBytes(clearText), keys[i]["tag"].GetValue<string>().FromHex(), null);

                byte[] decryptedSiv = SecureMessage.AesGcmSivDecryptor(iv, HKDF_key, cipherSivResult, keys[i]["tag"].GetValue<string>().FromHex());

                Assert.AreEqual(clearText, System.Text.Encoding.UTF8.GetString(decryptedSiv));

                // encrypt using sender primary shared key, decrypt using receiver primary shared key
                var sender = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

                var receiver = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

                byte[] senderPrimaryScalarResult = SecureMessage.DeriveSharedKey(sender.KeyPair.PrivateKey, receiver.KeyPair.PublicKey);

                byte[] senderPrimaryHKDF_key = SecureMessage.HKDF_Derive(senderPrimaryScalarResult);

                byte[] receiverPrimaryScalarResult = SecureMessage.DeriveSharedKey(receiver.KeyPair.PrivateKey, sender.KeyPair.PublicKey);

                byte[] receiverPrimaryHKDF_key = SecureMessage.HKDF_Derive(receiverPrimaryScalarResult);

                byte[] senderPrimaryCipherSivResult = SecureMessage.AesGcmSivEncryptor(iv, senderPrimaryHKDF_key, System.Text.Encoding.UTF8.GetBytes(clearText), keys[i]["tag"].GetValue<string>().FromHex(), null);

                byte[] receiverPrimaryDecryptedSiv = SecureMessage.AesGcmSivDecryptor(iv, receiverPrimaryHKDF_key, senderPrimaryCipherSivResult, keys[i]["tag"].GetValue<string>().FromHex());

                Assert.AreEqual(clearText, System.Text.Encoding.UTF8.GetString(receiverPrimaryDecryptedSiv));
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
            for (int x = 0; x < 100; x++)
            {
                var sender = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);
                var receiver = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

                byte[] msg = new byte[17];

                Random rnd = new Random();
                int len = rnd.Next(1, 2048);

                using (var ng = RandomNumberGenerator.Create())
                {
                    byte[] data = new byte[len];
                    
                    ng.GetNonZeroBytes(data);

                    msg = data;
                }

                var info = len % 2 == 0 ? System.Text.Encoding.UTF8.GetBytes("catapult") : null;

                var random = new SecureRandom();

                var ivData = new byte[16];
                random.NextBytes(ivData);

                // encrypt using sender primary shared key, decrypt using receiver primary shared key
                var senderPrimarySharedKey = SecureMessage.HKDF_Derive(SecureMessage.DeriveSharedKey(sender.KeyPair.PrivateKey, receiver.KeyPair.PublicKey), info, null);

                var senderPrimaryCipher = SecureMessage.AesEncryptor(senderPrimarySharedKey, ivData, System.Text.Encoding.UTF8.GetString(msg));

                var receiverPrimarySharedKey = SecureMessage.HKDF_Derive(SecureMessage.DeriveSharedKey(receiver.KeyPair.PrivateKey, sender.KeyPair.PublicKey), info, null);

                var receiverPrimaryPlainText = SecureMessage.AesDecryptor(receiverPrimarySharedKey, ivData, senderPrimaryCipher);

                Assert.That(sender.KeyPair.PrivateKey.ToHex() != receiver.KeyPair.PrivateKey.ToHex());
                Assert.AreEqual(System.Text.Encoding.UTF8.GetString(msg), receiverPrimaryPlainText);
            }
        }
    }
}
