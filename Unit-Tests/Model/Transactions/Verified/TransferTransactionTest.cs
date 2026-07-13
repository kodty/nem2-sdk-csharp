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
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateTransferTransaction(
                    Address.CreateFromEncoded("TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA"),
                    EmptyMessage.Create(),
                    Mosaic.CreateFromHexIdentifier("72C0212E67A08BCE", 101),
                    false
                );

            var result = accountRestriction.WrapVerified(keys, HttpSetUp.genHash);

            Assert.That(result.Payload.ToHex(), Is.EqualTo("B000000000000000BEF626718B91B12620FBAA6BC152EB3DD1F0BF1EDDDF2E9347B13DACFC8D100E9D5EDEC29133C42982DD09DE0666BF84373324351AD580DB53BEB3C0ABBE1A0E91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019854416E1D1AEC42000000B52E115A0200000098D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC0000010000000000CE8BA0672E21C0726500000000000000"));
        }
    }
}
