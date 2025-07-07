using Integration_Tests;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Model.Network;
using System.Reactive.Linq;

namespace Unit_Tests.Crypto
{
    internal class SignatureTests
    {
        [Test]
        public async Task TestSignature()
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var account = new Account(HttpSetUp.TestSK, NetworkType.Types.TEST_NET);
            var address = Address.CreateFromEncoded("TDRBSRHCPTURSR2M4IWUCRLSLYZCOZXBUJ4OIFA");

            var nodeClient = new NodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var ts = await nodeClient.GetNodeTime();

            var transaction = TransferTransaction.Create(
                account.PublicAccount,
                NetworkType.Types.TEST_NET,
                new Deadline(ts.CommunicationTimestamps.SendTimestamp, TimeSpan.FromMinutes(10)),
                100,
                address,
                new List<Mosaic1> { Mosaic1.CreateFromHexIdentifier("72C0212E67A08BCE", 1000) },
                PlainMessage.Create("hello")
            ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());

            Assert.True(transaction.VerifySignature());


        } 
    }
}
