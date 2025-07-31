using CopperCurve;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Tests.Model.Transactions.Embedded
{
    internal class EmbeddedTransferTransaction
    {
        [Test, Timeout(20000)]
        public async Task CreateEmbeddedTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var factory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var transfer = factory.CreateTransferTransaction(
                    "TBA6LOHEA6A465G2X5MSQF66JBYR254GJDPK7MQ",
                    "",
                    new Tuple<string, ulong>(DataConverter.ConvertFromUInt64(95442763262823).ToHex(), 101)
                );

            var payload = TransactionExtensions.PrepareEmbeddedTransaction<TransferTransaction_V1>(transfer, publicAcc);

            Debug.WriteLine(payload.payload.ToHex());

            Assert.That(payload.payload.ToHex(), Is.EqualTo("580000000000000091D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B45200000000019854419841E5B8E40781CF74DABF592817DE48711D778648DEAFB20000010000000000672B0000CE5600006500000000000000"));
        }
    }
}
