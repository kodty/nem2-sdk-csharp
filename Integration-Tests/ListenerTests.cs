using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests
{/*
    [TestClass]
    public class ListenerTests
    {
        [TestMethod, Timeout(30000)]
        public async Task ListenForBlock()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var block = await listener.NewBlock().Take(1);

            Assert.AreEqual(36867, block.Version);
        }

        [TestMethod, Timeout(20000)]
        public async Task ListenForUnconfirmedTransactionAdded()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var tx = listener.UnconfirmedTransactionsAdded(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR")).Take(1);

            await new TransferTransactionTests().AnnounceTransaction();

            var result = await tx;

            Assert.AreEqual("B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE", result.Signer.PublicKey);
        }

        //[TestMethod, Timeout(20000)]
        public async Task ListenForPartialTransactionAdded()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var aggregateTransaction = AggregateTransaction.CreateBonded(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                new List<Transaction>
                {
                TransferTransactionTests.CreateInnerTransferTransaction("nem:xem"),
                },
                null
            ).SignWith(keyPair);

            var hashLock = LockFundsTransaction.Create(NetworkType.Types.MIJIN_TEST, Deadline.CreateHours(2), 0, duration: 10000, mosaic: new Mosaic(new MosaicId("nem:xem"), 10000000), transaction: aggregateTransaction)
                .SignWith(KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain));

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(hashLock);

            var listener = new Listener(Config.Domain);

            await listener.Open();

            await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(
                keyPair.PublicKeyString,
                NetworkType.Types.MIJIN_TEST)
            ).Take(1);

            await new TransactionHttp("http://" + Config.Domain + ":3000").AnnounceAggregateBonded(aggregateTransaction);

            var result = await listener.AggregateBondedAdded(Address.CreateFromPublicKey(
                keyPair.PublicKeyString,
                NetworkType.Types.MIJIN_TEST)
            ).Take(1);

            Assert.AreEqual("10CC07742437C205D9A0BC0434DC5B4879E002114753DE70CDC4C4BD0D93A64A", result.Signer);
        }


        public async Task ListenForUnconfirmedTransactionRemoved()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var tx = listener.UnconfirmedTransactionsRemoved(Address.CreateFromEncoded("SBHQVG-3J27J4-7X7YQJ-F6WE7K-GUP76D-BASFNO-ZGEO")).Take(1);

            await new TransferTransactionTests().AnnounceTransaction();

            var result = await tx;

            Assert.AreEqual("10CC07742437C205D9A0BC0434DC5B4879E002114753DE70CDC4C4BD0D93A64A", result.Signer);
        }

        [TestMethod, Timeout(20000)]
        public async Task ListenForConfirmedTransactionAdded()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var tx = listener.ConfirmedTransactionsGiven(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR")).Take(1);

            await new TransferTransactionTests().AnnounceTransaction();

            var result = await tx;

            Assert.AreEqual("B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE", result.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task ListenForTransactionStatus()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var tx = listener.TransactionStatus(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR")).Take(1);

            await new TransferTransactionTests().AnnounceTransaction(2000000000000000000);

            var result = await tx;

            Assert.AreEqual("Failure_Core_Insufficient_Balance", result.Status);
        }
    }*/
}

