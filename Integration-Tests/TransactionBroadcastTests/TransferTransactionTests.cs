//
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using Integration_Tests;


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
            var keyPair =
                KeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.CreateHours(2),
                Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR"),
                new List<Mosaic1> { Mosaic1.CreateFromIdentifier("nem:xem", amount) },
                PlainMessage.Create("hello")
                ).SignWith(keyPair);

            await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);
        }

        [Test, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMosaicWithMessage()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var account = new Account("E45030D2A22D97FDC4C78923C4BBF7602BBAC3B018FFAD2ED278FB49CD6F218C", NetworkType.Types.TEST_NET);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.CreateHours(2),
                account.Address,
                new List<Mosaic1> { Mosaic1.CreateFromIdentifier("nem:xem", 100000000000) },
                PlainMessage.Create("hello")
            ).SignWith(keyPair);

            listener.GetTransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET))
                .Subscribe(e =>
                {
                    Console.WriteLine(e.Status);
                });

            await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        }

        [Test, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMosaicWithSecureMessage()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.CreateHours(2),
                Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                new List<Mosaic1> { Mosaic1.CreateFromIdentifier("nem:xem", 10) },
                SecureMessage.Create("hello2", HttpSetUp.TestSK, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")
                ).SignWith(keyPair);

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
                KeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                Deadline.CreateHours(2),
                Address.CreateFromEncoded("SAOV4Y5W627UXLIYS5O43SVU23DD6VNRCFP222P2"),
                new List<Mosaic1>()
                {
                    Mosaic1.CreateFromIdentifier("nem:xem", 1000000000000),
                    //Mosaic.CreateFromIdentifier("happy:test2", 10), YOU DID NOT BREAK THIS!
                },
                SecureMessage.Create("hello2", HttpSetUp.TestSK, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")

            ).SignWith(keyPair);

            await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        }

        [Test, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMultipleMosaicsWithoutMessage()
        {
            var keyPair =
                KeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                new List<Mosaic1>()
                {

                    Mosaic1.CreateFromIdentifier("happy:test2", 10),
                    Mosaic1.CreateFromIdentifier("nem:xem", 10),
                },
                EmptyMessage.Create()
            ).SignWith(keyPair);

            await new TransactionHttp(HttpSetUp.TestnetNode, HttpSetUp.Port).Announce(transaction);

            listener.GetTransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.TEST_NET)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Transaction.SignerPublicKey);
        }

        internal static TransferTransaction CreateInnerTransferTransaction(string mosaic, ulong amount = 10)
        {
            return TransferTransaction.Create(
                        NetworkType.Types.TEST_NET,
                        Deadline.CreateHours(2),
                        Address.CreateFromEncoded("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                        new List<Mosaic1> { Mosaic1.CreateFromIdentifier(mosaic, amount) },
                        PlainMessage.Create("hey")

                    );
        }
    }
}