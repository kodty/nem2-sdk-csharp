using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Infrastructure.HttpClients;
using System.Diagnostics;
using System.Reactive.Linq;
using io.nem2.sdk.Infrastructure;

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

            //var a = await client.Announce(st);

            var status = await client.GetTransactionStatus(st.Hash);

            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateHashLockTransaction()
        {          
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateHashLockTransaction(
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 0),
                    1440,
                    "A7D03C0D84B99E6253FF48976084194FBA858B85D37E812790A0CEB65E102402",
                    1000000,
                    false    
                );

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var a = await client.Announce(st);

            var status = await client.GetTransactionStatus(st.Hash);

            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateSecretLockTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateSecretLockTransaction(
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 0),
                    1440,
                    "secret",
                    HashType.Types.SHA3_512,
                    HttpSetUp.Recipient,
                    1000000,
                    false);

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var a = await client.Announce(st);

            var status = await client.GetTransactionStatus(st.Hash);

            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }

        [Test, Timeout(30000)]
        public async Task CreateMosaicSupplyChangeTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateMosaicSupplyChangeTransaction(
                    1000000000,
                    "72C0212E67A08BCE",
                    MosaicSupplyType.Type.INCREASE,
                    1000000,
                    false);

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var a = await client.Announce(st);

            var status = await client.GetTransactionStatus(st.Hash);

            Assert.AreEqual(status.ComposedResponse.Code, "Success");
        }
    }
}