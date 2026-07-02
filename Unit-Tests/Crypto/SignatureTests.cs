using Integration_Tests;
using Coppery;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients;
using System.Reactive.Linq;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using Unit_Tests.Model.Transactions;
using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using TweetNaclSharp;

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

            var factory = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var tx = factory.CreateTransferTransaction(address.Plain, "hello", new Tuple<string, ulong>("72C0212E67A08BCE", 1000), false);

            var st = tx.WrapVerified(keyPair, HttpSetUp.genHash);

            Assert.True(st.VerifySignature());
        }

        [Test, Timeout(20000)]
        public async Task CosignatureSignatureTest()
        {
            string privKey = "6AA6DAD25D3ACB3385D5643293133936CDDDD7F7E11818771DB1FF2F9D3F9215";
            string signature = "F21E4BE0A914C0C023F724E1EAB9071A3743887BB8824CB170404475873A827B301464261E93700725E8D4427A3E39D365AFB2C9191F75D33C6BE55896E0CC00";
            string data = "E4A92208A6FC52282B620699191EE6FB9CF04DAF48B48FD542C5E43DAA9897763A199AAA4B6F10546109F47AC3564FADE0";
            
            var pair = SecretKeyPair.CreateFromPrivateKey(privKey);
            
            var sig = pair.Sign(data.FromHex());
           
            Assert.AreEqual(sig.ToHex(), signature);
            Assert.True(NaclFast.SignDetachedVerify(data.FromHex(), signature.FromHex(), pair.PublicKey));
        }      
    }
}
