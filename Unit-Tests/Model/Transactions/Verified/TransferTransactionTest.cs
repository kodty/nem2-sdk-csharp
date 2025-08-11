using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class TransferTransactionTest
    {
        [Test, Timeout(20000)]
        public void CreateTransferTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    "TBA6LOHEA6A465G2X5MSQF66JBYR254GJDPK7MQ",
                    "",
                    new Tuple<string, ulong>("72C0212E67A08BCE", 101),
                    false
                );

            var result = accountRestriction.WrapVerified(keys);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("B000000000000000C02DBC08841D086331A2D1E1485933F11F13EB4B59C103E02082FD2A7266BEE44FADA9B706D77F516BA09732EBED15E42260DBDD73D9C85187D73198AE52510A91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019854416A5D22B404000000B52E115A020000009841E5B8E40781CF74DABF592817DE48711D778648DEAFB2000001000000000072C0212E67A08BCE6500000000000000"));
        }
    }
}
