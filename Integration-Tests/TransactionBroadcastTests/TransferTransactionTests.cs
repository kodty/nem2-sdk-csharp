using Coppery;
using Integration_Tests;
using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Infrastructure.HttpClients;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Utils;
using System.Diagnostics;
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
                    Address.CreateFromEncoded(HttpSetUp.Recipient), 
                    PlainMessage.Create("hello"),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                    1000000,
                    false
                );

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);
   
            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var a = await client.Announce(st);
            
            Thread.Sleep(3210);
            var status = await client.GetTransactionStatus(st.Hash);
            
            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(20000)]
        public async Task CreateAggregateCompleteTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var factory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var transfer1 = factory.CreateTransferTransaction(
                   Address.CreateFromEncoded(HttpSetUp.Recipient),
                   PlainMessage.Create("hello"),
                   Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                   1000000,
                   true
               );

            var transfer2 = factory.CreateTransferTransaction(
                   Address.CreateFromEncoded(HttpSetUp.Recipient),
                   PlainMessage.Create("olleh"),
                   Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                   1000000,
                   true
               );

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
               .CreateAggregateComplete(
                [
                    transfer1.Embed(keys.PublicKeyString),
                    transfer2.Embed(keys.PublicKeyString)
                ],
                Account.CreateFromPrivateKey(HttpSetUp.TestSK, NetworkType.Types.TEST_NET).KeyPair.PublicKey,
                10000000);

           //var signed = transfer.WrapVerified(keys, HttpSetUp.genHash);
           //
           //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
           //
           //var a = await client.Announce(signed);
           //
           //Thread.Sleep(4321);
           //var status = await client.GetTransactionStatus(signed.Hash);
           //
           //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateHashLockTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateHashLockTransaction(
                    "72C0212E67A08BCE",
                    10000000,
                    1440,
                    "FBA858B85D37E812790A0CEB65E102402A7D03C0D84B99E6253FF48976084194",
                    1000000,
                    false
                );

           // var st = transfer.WrapVerified(keys, HttpSetUp.genHash);
           //
           // var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
           //
           // var a = await client.Announce(st);
           //
           // Thread.Sleep(4321);
           // var status = await client.GetTransactionStatus(st.Hash);
           // 
           // Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        // multisig

        // account address, mosaic, opperation restrictions

        // account

        // node

        // VRF 

        // voting

        // address alias

        [Test, Timeout(30000)]
        public async Task CreateAccountMetadataTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateAccountMetadataTransaction(
                "TBEAFD6ZBP2J7LTUUWYC2A2ZLXONTWU2ABVCIBA",
                "aaaaaaaaaaaaaaaa",
                8,
                8,
                "bbbbbbbbbbbbbbbb".FromHex(),
                1000000);

            //var st = transfer.WrapVerified(keys, HttpSetUp.genHash);
            //
            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            //var a = await client.Announce(st);
            //
            //var status = await client.GetTransactionStatus(st.Hash);
            //
            //Thread.Sleep(4321);
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateNamespaceRegistrationTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "testspace", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    431922,
                    0,
                    IdGenerator.GenerateId(0, "testspace", true),
                    NamespaceTypes.Types.RootNamespace,
                    "testspace",
                    100000,
                    false);

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            
            var a = await client.Announce(st);
            
            Thread.Sleep(4321);
            var status = await client.GetTransactionStatus(st.Hash);
            
            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateNamespaceMetadataTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceMetadataTransaction(
                "TBEAFD6ZBP2J7LTUUWYC2A2ZLXONTWU2ABVCIBA",
                "aaaaaaaaaaaaaaaa",
                "aaaaaaaaaaaaaaaa",
                8,
                8,
                [],
                1000000
                );

            //var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            ////var a = await client.Announce(st);
            //
            //var status = await client.GetTransactionStatus(st.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicDefinitionTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
            
            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicDefinitionTransaction(
                    DataConverter.ConvertFrom(IdGenerator.GenerateMosaicId(AddressEncoder.DecodeAddress(PublicAccount.CreateFromPublicKey(keys.PublicKeyString, NetworkType.Types.TEST_NET).Address.Plain), 29498)).ToHex(),
                    29498,
                    new MosaicProperties(true, true, false, 6, 863935),
                    500000,
                    false);

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
           
            var a = await client.Announce(st);
           
            Thread.Sleep(4321);
            var status = await client.GetTransactionStatus(st.Hash);
            
            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        // mosaic address restriction

        // mosaic global restriction

        [Test, Timeout(30000)]
        public async Task CreateMosaicAliasTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        
            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicAliasTransaction(
                    "627911F4CC867A0D",
                    "E53FAC6DD7D1A69B",
                    0x1,
                    1000000,
                    false);
        
            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);
            Debug.WriteLine(st.Payload.ToHex());
            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
        
            var a = await client.Announce(st);
        
            Thread.Sleep(4321);
            var status = await client.GetTransactionStatus(st.Hash);
        
            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }


        [Test, Timeout(30000)]
        public async Task CreateMosaicSupplyChangeTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicSupplyChangeTransaction(
                    8919293949000000,
                    "627911F4CC867A0D",
                    MosaicSupplyType.Type.INCREASE,
                    1000000,
                    false);

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var a = await client.Announce(st);

            Thread.Sleep(4321);
            var status = await client.GetTransactionStatus(st.Hash);

            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        // mosaic alias

        [Test, Timeout(30000)]
        public async Task CreateMosaicMetadataTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicMetadataTransaction(
                "TBEAFD6ZBP2J7LTUUWYC2A2ZLXONTWU2ABVCIBA",
                "scopedKey",
                "targetNamespaceId",
                0,
                0,
                [],
                0
                );

           // var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            ////var a = await client.Announce(st);
            //
            //var status = await client.GetTransactionStatus(st.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        // mosaic supply revocation

        [Test, Timeout(30000)]
        public async Task CreateSecretLockTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretLockTransaction(
                    "72C0212E67A08BCE",
                    1440,
                    "secret",
                    HashType.Types.SHA3_512,
                    HttpSetUp.Recipient,
                    1000000,
                    false);

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            ////var a = await client.Announce(st);
            //
            //var status = await client.GetTransactionStatus(st.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateSecretProofTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretProofTransaction(
                    "recipientAddress",
                    "secret",
                    HashType.Types.SHA3_512,
                    "proof",
                    1000000,
                    false);

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            //var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
            //
            ////var a = await client.Announce(st);
            //
            //var status = await client.GetTransactionStatus(st.Hash);
            //
            //Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }
    }
}