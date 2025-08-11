using Coppery;
using Integration_Tests;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;

using io.nem2.sdk.src.Core.Utils;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Transactions;
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

        [Test, Timeout(20000)]
        public async Task TestNewTransactionFunctions()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    "TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA", 
                    "", 
                    new Tuple<string, ulong>("72C0212E67A08BCE", 100),
                    100,
                    false
                );
           
            var st = transfer.WrapVerified(keys);

            var s = listener.GetTransactionStatus(Address.CreateFromPublicKey(transfer.EntityBody.Signer.ToHex(), NetworkType.Types.TEST_NET))
             .Subscribe(e =>
             {
                 Debug.WriteLine("listener");
                 Debug.WriteLine("e " + e.Data.Code);
                 Debug.WriteLine("e " + e.Data.Deadline);
             });
           
           
            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
           
            var a = await client.Announce(st);
           
            Debug.WriteLine("msg " + a.Message);
           
            var status = await client.GetTransactionStatus(st.Hash);
           
            var listenerStatus = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transfer.EntityBody.Signer.ToHex(), NetworkType.Types.TEST_NET)).Take(1);

            Thread.Sleep(10000);
            Assert.AreEqual(keys.PublicKeyString, listenerStatus.Transaction.SignerPublicKey);

        }

        //[Test, Timeout(20000)]
        //public async Task Announce()
        //{
        //    var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        //    var Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKey.ToHex(), NetworkType.Types.TEST_NET);
        //
        //    var transaction = TransferTransaction.Create(
        //        Signer,
        //        NetworkType.Types.TEST_NET,
        //        Deadline.AutoDeadline(HttpSetUp.TestnetNode, HttpSetUp.Port),
        //        100,
        //        Address.CreateFromEncoded("TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA"),
        //        new List<Mosaic> { Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000) },
        //        PlainMessage.Create("hello")
        //        ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());
        //
        //    var s = listener.GetTransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET))
        //      .Subscribe(e =>
        //      {
        //          Debug.WriteLine("listener");
        //          Debug.WriteLine("e " + e.Data.Code);
        //          Debug.WriteLine("e " + e.Data.Deadline);
        //      });
        //
        //   
        //    var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
        //
        //    var a = await client.Announce(transaction);
        //    
        //    Debug.WriteLine("msg " + a.Message);
        //    Thread.Sleep(30000);
        //    s.Dispose();
        //    
        //    var status = await client.GetTransactionStatus(transaction.Hash);
        //
        //    var listenerStatus = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);
        //
        //    Assert.AreEqual(keyPair.PublicKeyString, listenerStatus.Transaction.SignerPublicKey);
        //}
        //
        //public async Task AnnounceTransaction(ulong amount = 10)
        //{
        //    var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        //    var Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.TEST_NET);
        //
        //    var transaction = TransferTransaction.Create(
        //        Signer,
        //        NetworkType.Types.TEST_NET,
        //        Deadline.AddHours(2),
        //        100,
        //        Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR"),
        //        new List<Mosaic> { Mosaic.CreateFromIdentifierParts([ "symbol", "xym" ], amount) },
        //        PlainMessage.Create("hello")
        //        ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());
        //
        //    await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);
        //}
        //
        //[Test, Timeout(20000)]
        //public async Task AnnounceTransferTransactionWithMosaicWithMessage()
        //{
        //    var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        //    var Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.TEST_NET);
        //
        //    var account = new Account(HttpSetUp.TestSK, NetworkType.Types.TEST_NET);
        //    var address = Address.CreateFromEncoded(HttpSetUp.TestAddress);
        //
        //    var transaction = TransferTransaction.Create(
        //        Signer,
        //        NetworkType.Types.TEST_NET,
        //        new Deadline(1),
        //        100,
        //        address,
        //        new List<Mosaic> { Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000) },
        //        PlainMessage.Create("hello")
        //    ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());
        //
        //    var a = listener.GetTransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET))
        //        .Subscribe(e =>
        //        {
        //            Debug.WriteLine(e.Data.Code);
        //        });
        //
        //    var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);
        //
        //    await client.Announce(transaction);
        //
        //    var status = await client.GetTransactionStatus(transaction.Hash);
        //
        //    var listenerStatus = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);
        //
        //    Assert.AreEqual(keyPair.PublicKeyString, listenerStatus.Transaction.SignerPublicKey);
        //}
        //
        //[Test, Timeout(20000)]
        //public async Task AnnounceTransferTransactionWithMosaicWithSecureMessage()
        //{
        //    var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        //
        //    var Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.TEST_NET);
        //
        //    var transaction = TransferTransaction.Create(
        //        Signer,
        //        NetworkType.Types.TEST_NET,
        //        Deadline.AddHours(2),
        //        100,
        //        Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
        //        new List<Mosaic> { Mosaic.CreateFromIdentifierParts(["symbol", "xym"], 10) },
        //        SecureMessage.Create("hello2", HttpSetUp.TestSK, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")
        //        ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());
        //
        //    await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);
        //
        //    listener.GetTransactionStatus(Address.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.TEST_NET))
        //        .Subscribe(e => Console.WriteLine(e.Data.Code));
        //
        //    var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);
        //
        //    Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        //}
        //
        //[Test, Timeout(20000)]
        //public async Task AnnounceTransferTransactionWithMultipleMosaicsWithSecureMessage()
        //{
        //    var keyPair =
        //        SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        //    var Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.TEST_NET);
        //
        //    var transaction = TransferTransaction.Create(
        //        Signer,
        //        NetworkType.Types.TEST_NET,
        //        Deadline.AddHours(2),
        //        100,
        //        Address.CreateFromEncoded("SAOV4Y5W627UXLIYS5O43SVU23DD6VNRCFP222P2"),
        //        new List<Mosaic>()
        //        {
        //            Mosaic.CreateFromIdentifierParts([ "symbol", "xym" ], 1000000000000),
        //           
        //        },
        //        SecureMessage.Create("hello2", HttpSetUp.TestSK, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")
        //
        //    ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());
        //
        //    await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);
        //
        //    var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);
        //
        //    Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        //}
        //
        //[Test, Timeout(20000)]
        //public async Task AnnounceTransferTransactionWithMultipleMosaicsWithoutMessage()
        //{
        //    var keyPair =
        //        SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
        //    var Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.TEST_NET);
        //
        //    var transaction = TransferTransaction.Create(
        //        Signer,
        //        NetworkType.Types.MIJIN_TEST,
        //        Deadline.AddHours(2),
        //        100,
        //        Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
        //        new List<Mosaic>()
        //        {
        //
        //            Mosaic.CreateFromIdentifierParts([ "happy", "test2" ], 10),
        //            Mosaic.CreateFromIdentifierParts([ "symbol", "xym" ], 10),
        //        },
        //        EmptyMessage.Create()
        //    ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());
        //
        //    await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);
        //
        //    listener.GetTransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET))
        //        .Subscribe(e => Console.WriteLine(e.Data.Code));
        //
        //    var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);
        //
        //    Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        //}
        //
        //internal static TransferTransaction_V1 CreateInnerTransferTransaction(string[] mosaic, ulong amount = 10)
        //{
        //    return TransferTransaction_V1.Create(
        //                NetworkType.Types.TEST_NET,
        //                Deadline.AddHours(2),
        //                Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
        //                new List<Mosaic> { Mosaic.CreateFromIdentifierParts(mosaic, amount) },
        //                PlainMessage.Create("hey")
        //
        //            );
        //}
    }
}