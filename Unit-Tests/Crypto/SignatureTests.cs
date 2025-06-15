using Integration_Tests;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.src.Model.Network;
using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using static io.nem2.sdk.Infrastructure.HttpRepositories.TransactionHttp;


namespace Unit_Tests.Crypto
{
    internal class SignatureTests
    {
        [Test]
        public void TestSignature()
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var account = new Account(HttpSetUp.TestSK, NetworkType.Types.TEST_NET);
            var address = Address.CreateFromEncoded("TDRBSRHCPTURSR2M4IWUCRLSLYZCOZXBUJ4OIFA");

            var transaction = TransferTransaction.Create(
                NetworkType.Types.TEST_NET,
                new Deadline(1),
                address,
                new List<Mosaic1> { Mosaic1.CreateFromHexIdentifier("72C0212E67A08BCE", 1000) },
                PlainMessage.Create("hello")
            ).SignWith(keyPair, HttpSetUp.NetworkGenHash.FromHex());

            Assert.True(transaction.VerifySignature());


        } 
    }
}
