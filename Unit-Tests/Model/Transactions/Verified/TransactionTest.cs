using Coppery;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Articles;
using io.nem2.sdk.Model.Transactions.Messages;
using io.nem2.sdk.Utils;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class TransactionTest
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

        [Test, Timeout(20000)]
        public void CreateMosaicDefinitionTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var transfer = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                 .CreateMosaicDefinitionTransaction(
                    DataConverter.ConvertFrom(IdGenerator.GenerateMosaicId(AddressEncoder.DecodeAddress(PublicAccount.CreateFromPublicKey(keys.PublicKeyString, NetworkType.Types.TEST_NET).Address.Plain), 2745897589)).ToHex(),
                    2745897589,
                    new MosaicProperties(true, true, false, 6, 1000000),
                    500000,
                    false);

            transfer.Fee = DataConverter.ConvertFrom((ulong)500000);
            transfer.Deadline = DataConverter.ConvertFrom((ulong)117657395737);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("96000000000000002AFDC76E12D90490D6DCE8D15DA1FC5929B57E894F23C1C33CEB920CAD193CBCE774A8D96277D5BD6FE0F0007C9E798910FF1C091D240BE7BF92F54776E0A80091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984D4120A10700000000001962ED641B000000555AB6A91955924240420F00000000007512ABA30306"));
        }

        [Test, Timeout(20000)]
        public void CreateNamespaceRentalTest()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey(HttpSetUp.TestSK);

            var root = IdGenerator.GenerateId(0, "testspace", true);

            var transfer = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateNamespaceRegistrationTransaction(
                    431922,
                    0,
                    IdGenerator.GenerateId(0, "testspace", true),
                    NamespaceTypes.Types.RootNamespace,
                    "testspace",
                    100000,
                    false);

            transfer.Fee = DataConverter.ConvertFrom((ulong)100000);
            transfer.Deadline = DataConverter.ConvertFrom((ulong)117657800500);

            var result = transfer.SignTransaction(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("9B00000000000000DB0979FEFABB7FFF49017B78785E340B20D9A7D34C50262A477875EEC604AE3FD11C6E1123DACFE5E184AA9D1CE91F27747601E229F3B7817337C37A2FB42A0691D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B4520000000001984E41A086010000000000348FF3641B0000003297060000000000C70F9CB252E64AB60009746573747370616365"));
        }
    }
}
