using System.Reactive.Linq;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.src.Model.Network;
using Integration_Tests;
using System.Diagnostics;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;


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

        public async Task AnnounceTransaction(ulong amount = 10)
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.AddHours(2),
                Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR"),
                new List<Mosaic1> { Mosaic1.CreateFromIdentifierParts([ "symbol", "xym" ], amount) },
                PlainMessage.Create("hello")
                ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());

            await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);
        }

        [Test, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMosaicWithMessage()
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var account = new Account(HttpSetUp.TestSK, NetworkType.Types.TEST_NET);
            var address = Address.CreateFromEncoded(HttpSetUp.TestAddress);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                new Deadline(1),
                address,
                new List<Mosaic1> { Mosaic1.CreateFromHexIdentifier("72C0212E67A08BCE", 1000) },
                PlainMessage.Create("hello")
            ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());

            var a = listener.GetTransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET))
                .Subscribe(e =>
                {
                    Debug.WriteLine(e.Status);
                });

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            await client.Announce(transaction);

            var status = await client.GetTransactionStatus(transaction.Hash);

            var listenerStatus = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, listenerStatus.Transaction.SignerPublicKey);
        }

        [Test, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMosaicWithSecureMessage()
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.AddHours(2),
                Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                new List<Mosaic1> { Mosaic1.CreateFromIdentifierParts(["symbol", "xym"], 10) },
                SecureMessage.Create("hello2", HttpSetUp.TestSK, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")
                ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());

            await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);

            listener.GetTransactionStatus(Address.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.TEST_NET))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        }

        [Test, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMultipleMosaicsWithSecureMessage()
        {
            var keyPair =
                SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.AddHours(2),
                Address.CreateFromEncoded("SAOV4Y5W627UXLIYS5O43SVU23DD6VNRCFP222P2"),
                new List<Mosaic1>()
                {
                    Mosaic1.CreateFromIdentifierParts([ "symbol", "xym" ], 1000000000000),
                   
                },
                SecureMessage.Create("hello2", HttpSetUp.TestSK, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")

            ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());

            await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        }

        [Test, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMultipleMosaicsWithoutMessage()
        {
            var keyPair =
                SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.AddHours(2),
                Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                new List<Mosaic1>()
                {

                    Mosaic1.CreateFromIdentifierParts([ "happy", "test2" ], 10),
                    Mosaic1.CreateFromIdentifierParts([ "symbol", "xym" ], 10),
                },
                EmptyMessage.Create()
            ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());

            await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);

            listener.GetTransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        }

        internal static TransferTransaction CreateInnerTransferTransaction(string[] mosaic, ulong amount = 10)
        {
            return TransferTransaction.Create(
                        NetworkType.Types.TEST_NET,
                        Deadline.AddHours(2),
                        Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                        new List<Mosaic1> { Mosaic1.CreateFromIdentifierParts(mosaic, amount) },
                        PlainMessage.Create("hey")

                    );
        }
    }
}