using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class TransferTransactionTest
    {
        [Test, Timeout(20000)]
        public void CreateTransferTransactionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    Address.CreateFromEncoded(HttpSetUp.Recipient),
                    PlainMessage.Create("hello"),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 1000000),
                    1000000,
                    false
                );

            transfer.Deadline = DataConverter.ConvertFrom((ulong)117636263499);
            transfer.Fee = DataConverter.ConvertFrom((ulong)1000000);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("B500000000000000254248646F9406FDAAE8DAB81E7AD00D05FEE7AE0D9A04EE3F8EC0EF2C1CF7171D778E5A0CF06B7EFF392156132E6BACCE1E18F066680821687ABAC8971C490591D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B452000000000198544140420F00000000004BEEAA631B0000009848028FD90BF49FAE74A5B02D03595DDCD9DA9A006A24040500010000000000CE8BA0672E21C07240420F000000000068656C6C6F"));
        }
    }
}
