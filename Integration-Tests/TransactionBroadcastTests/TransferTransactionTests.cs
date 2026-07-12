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
        public async Task TestNewTransactionFunctions()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var acc = new AccountHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).GetAccount(keys.PublicKeyString).Wait();

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    Address.CreateFromEncoded("TBEAFD6ZBP2J7LTUUWYC2A2ZLXONTWU2ABVCIBA"), 
                    PlainMessage.Create("hello"),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                    1000000,
                    false
                );

            var st = transfer.WrapVerified(keys, HttpSetUp.genHash);

            var bl = listener.NewBlock().Subscribe(e =>
            {
                Debug.WriteLine("block");
                Debug.WriteLine("e " + e.Block.Height);
            });

            var s = listener.GetTransactionStatus(Address.CreateFromEncoded("TD4MZ66MVUX6ETNIXNLF6SA7YRTPU6C4X56ZJQA"))
             .Subscribe(e =>
             {         
                    Debug.WriteLine("listener");
                    Debug.WriteLine("e " + e.Status);           
             });
                    
            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var a = await client.Announce(st);
            Debug.WriteLine(a.Message);

            Thread.Sleep(1000);
            var status = await client.GetTransactionStatus(st.Hash);

            Debug.WriteLine(status.ComposedResponse.Code);
            var listenerStatus = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transfer.EntityBody.Signer.ToHex(), NetworkType.Types.TEST_NET)).Take(1);

            Assert.AreEqual(keys.PublicKeyString, "");

        }
    }
}