using IntegrationTests.Infrastructure.Transactions;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using Coppery;
using System.Reactive.Linq;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;

namespace Integration_Tests.HttpRequests
{
    public class ListenerTests
    {
        [Test]
        public async Task ListenForBlock()
        {
            var listener = new Listener(HttpSetUp.Node, HttpSetUp.Port);

            await listener.Open();

            var block = await listener.NewBlock().Take(1);
            Assert.That(block.Block.Height, Is.GreaterThan(100));
            Assert.AreEqual(1, block.Block.Version);
        }

        //[Test, Timeout(20000)]
        //public async Task ListenForUnconfirmedTransactionAdded()
        //{
        //    var listener = new Listener(HttpSetUp.TestnetNode, HttpSetUp.Port);
        //
        //    await listener.Open();
        //
        //    var tx = listener.UnconfirmedTransactionsAdded(Address.CreateFromEncoded(HttpSetUp.TestAddress)).Take(1);
        //
        //    await new TransferTransactionTests().AnnounceTransaction();
        //
        //    var result = await tx;
        //
        //    Assert.AreEqual("", result.Transaction.SignerPublicKey);
        //}

       // [Test, Timeout(20000)]
       // public async Task ListenForPartialTransactionAdded()
       // {
       //     var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);
       //
       //     var aggregateTransaction = AggregateTransaction.CreateBonded(
       //         NetworkType.Types.TEST_NET,
       //         Deadline.AddHours(2),
       //         new List<Transaction>
       //         {
       //             TransferTransactionTests.CreateInnerTransferTransaction(["symbol", "xym"]),
       //         },
       //         null
       //     ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());
       //
       //     var hashLock = TransactionFactory.(NetworkType.Types.TEST_NET, Deadline.AddHours(2), 0, 10000, mosaic: new Mosaic(new MosaicId("symbol:xym"), 10000000), transaction: aggregateTransaction)
       //         .SignWith(SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK), HttpSetUp.NetworkGenHash.FromHex());
       //
       //     await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(hashLock);
       //
       //     var listener = new Listener(HttpSetUp.TestnetNode, HttpSetUp.Port);
       //
       //     await listener.Open();
       //
       //     await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(
       //         keyPair.PublicKeyString,
       //         NetworkType.Types.TEST_NET)
       //     ).Take(1);
       //
       //     await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).AnnounceAggregateTransaction(aggregateTransaction);
       //
       //     var result = await listener.AggregateBondedAdded(Address.CreateFromPublicKey(
       //         keyPair.PublicKeyString,
       //         NetworkType.Types.TEST_NET)
       //     ).Take(1);
       //
       //     Assert.AreEqual("10CC07742437C205D9A0BC0434DC5B4879E002114753DE70CDC4C4BD0D93A64A", result.Transaction.SignerPublicKey);
       // }

        //[Test, Timeout(20000)]
        //public async Task ListenForUnconfirmedTransactionRemoved()
        //{
        //    var listener = new Listener(HttpSetUp.TestnetNode, HttpSetUp.Port);
        //
        //    await listener.Open();
        //
        //    var tx = listener.UnconfirmedTransactionsRemoved(Address.CreateFromEncoded("SBHQVG-3J27J4-7X7YQJ-F6WE7K-GUP76D-BASFNO-ZGEO")).Take(1);
        //
        //    await new TransferTransactionTests().AnnounceTransaction();
        //
        //    var result = await tx;
        //
        //    Assert.AreEqual("10CC07742437C205D9A0BC0434DC5B4879E002114753DE70CDC4C4BD0D93A64A", result.Transaction.SignerPublicKey);
        //}

        //[Test, Timeout(20000)]
        //public async Task ListenForConfirmedTransactionAdded()
        //{
        //    var listener = new Listener(HttpSetUp.TestnetNode, HttpSetUp.Port);
        //
        //    await listener.Open();
        //
        //    var tx = listener.ConfirmedTransactionsGiven(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR")).Take(1);
        //
        //    await new TransferTransactionTests().AnnounceTransaction();
        //
        //    var result = await tx;
        //
        //    Assert.AreEqual("B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE", result.Transaction.SignerPublicKey);
        //}

        //[Test, Timeout(20000)]
        //public async Task ListenForTransactionStatus()
        //{
        //    var listener = new Listener(HttpSetUp.TestnetNode, HttpSetUp.Port);
        //
        //    await listener.Open();
        //
        //    var tx = listener.GetTransactionStatus(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR")).Take(1);
        //
        //    await new TransferTransactionTests().AnnounceTransaction(200);
        //
        //    var result = await tx;
        //
        //    Assert.AreEqual("Failure_Core_Insufficient_Balance", result.Data.Code);
        //}
    }
}

