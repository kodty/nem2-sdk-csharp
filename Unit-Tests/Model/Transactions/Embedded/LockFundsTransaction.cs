using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using System.Diagnostics;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class LockFundsTransaction
    {
        [Test, Timeout(20000)]
        public async Task CreateLockFundsEmbeddedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var lockFunds = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateLockFundsTransaction(
                    new Tuple<string, ulong>("72C0212E67A08BCE", 101),
                    1440,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    true
                );

            var result = lockFunds.Embed(keys.PublicKeyString);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("680000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B452000000000198484172C0212E67A08BCE6500000000000000A00500000000000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D"));
        }
    }
}
