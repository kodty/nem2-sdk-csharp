using Coppery;
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

            Assert.That(st.Payload.ToHex(), Is.EqualTo("E800000000000000AE5824FCFD0F0F2BBE67DDAF7B2004C31AF35B46EE9CC32F5EC8CA6F531075A014C909FEFF7EA7D934C66402F88919D6F3ACE8905541A31A0DEDC98EDCAA960691D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019850416A5D22B404000000B52E115A02000000010002020000000068172E4A424D395695A92AA7DAAFC0B1887846448ABD57AC90172E4A424D395695A92AA7DAAFC0B1887846448ABD57AC98172E4A424D395695A92AA7DAAFC0B1887846448ABD57ACA8172E4A424D395695A92AA7DAAFC0B1887846448ABD57AC"));
        }
    }
}
