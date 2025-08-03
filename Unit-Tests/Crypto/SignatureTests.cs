using Integration_Tests;

using CopperCurve;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using System.Reactive.Linq;
using io.nem2.sdk.src.Model.Transactions.Messages;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Articles;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Transactions;

namespace Unit_Tests.Crypto
{
    internal class SignatureTests
    {
        [Test, Timeout(20000)]
        public async Task TestSignature()
        {
            var keyPair = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var account = new Account(HttpSetUp.TestSK, NetworkType.Types.TEST_NET);
            var address = Address.CreateFromEncoded("TDRBSRHCPTURSR2M4IWUCRLSLYZCOZXBUJ4OIFA");

            var nodeClient = new NodeHttp(HttpSetUp.TestnetNode, HttpSetUp.Port);

            var ts = await nodeClient.GetNodeTime();

            var factory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var tx = factory.CreateTransferTransaction(address.Plain, "hello", new Tuple<string, ulong>("72C0212E67A08BCE", 1000), false);

            var st = tx.WrapVerified(keyPair);

            Assert.True(st.VerifySignature());


        }
        [Test, Timeout(20000)]
        public async Task CosignatureSignatureTest()
        {

        }      
    }
}
