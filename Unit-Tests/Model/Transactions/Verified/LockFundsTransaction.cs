using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class VerifiedLockFundsTransaction
    {

        [Test, Timeout(20000)]
        public async Task CreateLockFundsVerifiedTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var lockFunds = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateLockFundsTransaction(
                    "72C0212E67A08BCE",
                    1440,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    false
                );

            var result = lockFunds.WrapVerified(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("B00000000000000081CDD5A01B0EF3C34A802CD5ED0F2C88B2F961891241F86DF7A3D58DD4BF08D878F8B0C16666459B100390B39760556F4F478F7BB2A68574C841E4DC8FA1020C91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019848416A5D22B404000000B52E115A0200000072C0212E67A08BCEA00500000000000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D"));
        }
    }
}
