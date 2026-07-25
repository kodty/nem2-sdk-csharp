using Coppery;
using Integration_Tests;
using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Infrastructure.HttpClients;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Utils;
using System.Reactive.Linq;

namespace IntegrationTests.Infrastructure.Transactions
{
    public class TransferTransactionTests
    {
        private Listener listener { get; }

        public TransferTransactionTests()
        {
            listener = new Listener(HttpSetUp.TestnetNode, HttpSetUp.Port);

            listener.Open().Wait();
        }

        [Test, Timeout(30000)]
        public async Task TestStatus()
        {
            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
                   
            var status = await client.GetTransactionStatus("36EC6AAE357E30BEACABA717061A30B6F7F316907D6CB6DE1D2D0ECFFCBC6F3C");

            Assert.That(status.ComposedResponse.Code == "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateTransferTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    Address.CreateFromEncoded(""), 
                    EmptyMessage.Create(),
                    Mosaic.CreateFromHexIdentifier("", 0),
                    0,
                    false
                );

            transfer.SetSigner(keys.PublicKeyString);

            //var st = transfer.SignTransaction(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(20000)]
        public async Task CreateAggregateCompleteTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var factory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var transfer1 = factory.CreateTransferTransaction(
                   Address.CreateFromEncoded(HttpSetUp.Recipient),
                   PlainMessage.Create(""),
                   Mosaic.CreateFromHexIdentifier("", 0),
                   0,
                   true
               );

            transfer1.SetSigner(keys.PublicKeyString);

            var transfer2 = factory.CreateTransferTransaction(
                   Address.CreateFromEncoded(HttpSetUp.Recipient),
                   PlainMessage.Create(""),
                   Mosaic.CreateFromHexIdentifier("", 0),
                   0,
                   true
               );

            transfer2.SetSigner(keys.PublicKeyString);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
               .CreateAggregateComplete(
                [
                    transfer1.SignEmbeddedTransaction(keys),
                    transfer2.SignEmbeddedTransaction(keys)
                ],
                Account.CreateFromPrivateKey(HttpSetUp.TestSK, NetworkType.Types.TEST_NET).KeyPair.PublicKey,
                0);

            transfer.SetSigner(keys.PublicKeyString);

            //var signed = transfer.WrapVerified(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(signed);

            //var status = await client.GetTransactionStatus(signed.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateHashLockTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateHashLockTransaction(
                    "",
                    0,
                    0,
                    "",
                    0,
                    false
                );

            transfer.SetSigner(keys.PublicKeyString);

            //var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateAccountMetadataTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateAccountMetadataTransaction(
                "",
                "",
                0,
                0,
                "".FromHex(),
                0);

            transfer.SetSigner(keys.PublicKeyString);

            //var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateNamespaceRegistrationTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "testspace", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    0,
                    0,
                    IdGenerator.GenerateId(0, "", true),
                    NamespaceTypes.Types.RootNamespace,
                    "",
                    0,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            //var st = transfer.SignTransaction(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateNamespaceMetadataTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceMetadataTransaction(
                "",
                "",
                "",
                0,
                0,
                [],
                0
                );

            transfer.SetSigner(keys.PublicKeyString);

            //var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicDefinitionTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
            
            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicDefinitionTransaction(
                    DataConverter.ConvertFrom(IdGenerator.GenerateMosaicId(AddressEncoder.DecodeAddress(PublicAccount.CreateFromPublicKey(keys.PublicKeyString, NetworkType.Types.TEST_NET).Address.Plain), 0)).ToHex(),
                    0,
                    new MosaicProperties(true, true, false, 0, 0),
                    0,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            //var st = transfer.SignTransaction(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicAliasTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        
            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicAliasTransaction(
                    "",
                    "",
                    0x0,
                    0,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            //var st = transfer.SignTransaction(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }


        [Test, Timeout(30000)]
        public async Task CreateMosaicSupplyChangeTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicSupplyChangeTransaction(
                    0,
                    "",
                    MosaicSupplyType.Type.DECREASE,
                    0,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            //var st = transfer.SignTransaction(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicMetadataTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicMetadataTransaction(
                "",
                "",
                "",
                0,
                0,
                [],
                0);

            transfer.SetSigner(keys.PublicKeyString);

            // var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateSecretLockTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretLockTransaction(
                    "",
                    0,
                    "",
                    HashType.Types.SHA3_512,
                    HttpSetUp.Recipient,
                    0,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            // var st = transfer.SignTransaction(keys, HttpSetUp.genHash);

            // var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateSecretProofTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretProofTransaction(
                    "",
                    "",
                    HashType.Types.SHA3_512,
                    "",
                    1000000,
                    false);

            transfer.SetSigner(keys.PublicKeyString);

            var st = transfer.SignTransaction(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            //var a = await client.Announce(st);

            //var status = await client.GetTransactionStatus(st.Hash);

            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }
    }
}