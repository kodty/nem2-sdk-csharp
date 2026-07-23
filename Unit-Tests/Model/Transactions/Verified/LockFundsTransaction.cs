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
                    10000000,
                    1440,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    false
                );

            var result = lockFunds.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("B800000000000000250B2B6B19693ECA787BD7EE6085EC5996D89BEA1E4BCCBB147FF7FBEA48BEDB1A1EA1014D1207421F09137B5EA6552471CDC54448A84D8524FE2CC87749270D91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019848416A5D22B404000000B52E115A02000000CE8BA0672E21C0728096980000000000A00500000000000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D"));
        }
    }
}
