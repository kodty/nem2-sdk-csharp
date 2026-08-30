using Coppery;
using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using System.Reactive.Linq;

namespace Integration_Tests.HttpRequestTests
{
    public class ListenerTests
    {
        [Test, Timeout(30000)]
        public async Task ListenForUnconfirmedTransactionAdded()
        {
            for (int x = 0; x < 10; x++)
            {
                var listener = new Listener(HttpSetUp.TestnetNode, HttpSetUp.Port);

                await listener.Open();

                var socketResponses = listener.UnconfirmedTransactionsAdded(Address.CreateFromEncoded(HttpSetUp.address));

                var tx = HttpSetUp.AnnounceTransaction();

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

