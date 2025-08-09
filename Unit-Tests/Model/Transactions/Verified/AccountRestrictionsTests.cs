using CopperCurve;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using System.Diagnostics;

namespace Unit_Tests.Model.Transactions.Verified
{
    internal class AccountRestrictionsTests
    {
        [Test, Timeout(20000)]
        public void CreateAccountRestriction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var accountRestriction = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port)
                .CreateAccountRestrictionTransaction(
                    TransactionTypes.Types.ACCOUNT_ADDRESS_RESTRICTION,
                    0x1,
                    ["NALS4SSCJU4VNFNJFKT5VL6AWGEHQRSERK6VPLA", "SALS4SSCJU4VNFNJFKT5VL6AWGEHQRSERK6VPLA"],
                    ["TALS4SSCJU4VNFNJFKT5VL6AWGEHQRSERK6VPLA", "VALS4SSCJU4VNFNJFKT5VL6AWGEHQRSERK6VPLA"],
                    false
                );

            var st = accountRestriction.WrapVerified(keys);

            Debug.WriteLine(st.Payload.ToHex());

            Assert.That(st.Payload.ToHex(), Is.EqualTo(""));
        }
    }
}
