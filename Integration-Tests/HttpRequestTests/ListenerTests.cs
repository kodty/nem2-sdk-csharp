using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Infrastructure.HttpClients;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequests
{
    public class ListenerTests
    {
        private Tuple<SignedTransaction, IObservable<TransactionAnnounceResponse>> AnnounceTransaction()
        {
            var newAccount = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);

            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    address: Address.CreateFromEncoded(newAccount.Address.Plain),
                    messege: EmptyMessage.Create(),
                    mosaic: Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000),
                    fee: 1000000,
                    embedded: false
                );

            transfer.SetSigner(keys.PublicKeyString);

            var client = new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            return new Tuple<SignedTransaction, IObservable<TransactionAnnounceResponse>>(result, client.Announce(result));

        }

        [Test, Timeout(20000)]
        public async Task ListenForUnconfirmedTransactionAdded()
        {
            for (int x = 0; x < 5; x++)
            {
                var listener = new Listener(HttpSetUp.TestnetNode, HttpSetUp.Port);

                await listener.Open();

                var socketResponses = listener.UnconfirmedTransactionsAdded(Address.CreateFromEncoded(HttpSetUp.address));

                var tx = AnnounceTransaction();

                bool unconfirmed = false;

                socketResponses.Subscribe(t =>
                {
                    var c = t.Transaction.Type.GetTypeValue();

                    if (t.Meta.Hash == tx.Item1.Hash && c == typeof(SimpleTransfer))
                    {
                        Assert.AreEqual(((SimpleTransfer)t.Transaction).SignerPublicKey, HttpSetUp.pubKey);

                        unconfirmed = true;
                    }
                });

                var result = await tx.Item2;

                Assert.AreEqual(result.Message, "packet 9 was pushed to the network via /transactions");

                int sleep = 0;

                while (!unconfirmed)
                {
                    Thread.Sleep(1);
                    sleep++;
                    if (sleep > 100) { break; }
                }

                Assert.IsTrue(unconfirmed);
            }
        }

        [Test]
        public async Task ListenForBlock()
        {
            var listener = new Listener(HttpSetUp.Node, HttpSetUp.Port);

            await listener.Open();

            var block = await listener.NewBlock().Take(1);
            Assert.That(block.Block.Height, Is.GreaterThan(100));
            Assert.AreEqual(1, block.Block.Version);
        }
    }
}

