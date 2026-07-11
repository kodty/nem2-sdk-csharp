using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients.Listeners;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
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
        public async Task TestNewTransactionFunctions()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    "TB3LCAYOKFB7S552N7UQIVHZZL6EUXTO2OPBJGY", 
                    "hello", 
                    new Tuple<string, ulong>("72C0212E67A08BCE", 200),
                    50,
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