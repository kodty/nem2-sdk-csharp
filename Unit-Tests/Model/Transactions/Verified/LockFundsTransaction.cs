using Coppery;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    new Tuple<string, ulong>("72C0212E67A08BCE", 101),
                    1440,
                    "98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D",
                    false
                );

            var result = lockFunds.WrapVerified(keys);
            Debug.WriteLine(result.Payload.ToHex());
            Assert.That(result.Payload.ToHex(), Is.EqualTo("B800000000000000EE8E77731A11BCB058A3029DC0223EF4ADC024C8F5B82C7485EACB9AB4AA1C0DD6E5B6C58951A8B6F84D98BF8EC6470232E93B2BC7307BF7A583CA62A9105A0691D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019848416A5D22B404000000B52E115A0200000072C0212E67A08BCE6500000000000000A00500000000000098AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D"));
        }
    }
}
