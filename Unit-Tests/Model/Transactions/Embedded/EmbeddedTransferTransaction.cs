using CopperCurve;
using Integration_Tests;
using io.nem2.sdk.Model;
using io.nem2.sdk.src.Core.Utils;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Accounts;
using io.nem2.sdk.src.Model.Articles;
using io.nem2.sdk.src.Model.Transactions;
using io.nem2.sdk.src.Model.Transactions.MosaicPropertiesTransactions;
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
        public async Task CreateSupplyChangeEmbeddedTransaction()
        {
            Debug.WriteLine(1);
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            Debug.WriteLine(2);
            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            Debug.WriteLine(3);
            var factory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            Debug.WriteLine(4);
            var supplyChange = factory.CreateMosaicSupplyChangeTransaction(
                    10,
                    DataConverter.ConvertFromUInt64(6300565133566699913).ToHex(),
                    MosaicSupplyType.Type.INCREASE,
                    true
                );

            Debug.WriteLine(5);
            var supplyChangePayload = TransactionExtensions.PrepareEmbeddedTransaction<MosaicSupplyChangeTransaction1>(supplyChange, publicAcc);

            Debug.WriteLine(supplyChangePayload.Payload.ToHex());

            Assert.That(supplyChangePayload.Payload.ToHex(), Is.EqualTo("390000000000000000000000000000000000000000000000000000000000000000000000000000000000000001984D420A000000000000008969746E9B1A705701"));
        }

        [Test, Timeout(20000)]
        public async Task CreateEmbeddedTransaction()
        {
            var keys = SecretKeyPair.CreateFromPrivateKey("98AA70CA43E5D3B95CD303A57892D0BA953C204A4D937AF4386ED658A8FA555D");

            var publicAcc = new PublicAccount(keys.PublicKeyString, NetworkType.Types.TEST_NET);

            var factory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var transfer = factory.CreateTransferTransaction(
                    "TBA6LOHEA6A465G2X5MSQF66JBYR254GJDPK7MQ",
                    "",
                    new Tuple<string, ulong>("672B0000CE560000", 101),
                    true
                );

            var payload = TransactionExtensions.PrepareEmbeddedTransaction<TransferTransaction_V1>(transfer, publicAcc);


            Debug.WriteLine(payload.Payload.ToHex());

            Assert.That(payload.Payload.ToHex(), Is.EqualTo("5800000000000000000000000000000000000000000000000000000000000000000000000000000000000000019854419841E5B8E40781CF74DABF592817DE48711D778648DEAFB20000010000000000672B0000CE5600006500000000000000"));


            var supplyChange = factory.CreateMosaicSupplyChangeTransaction(
                    10,
                    DataConverter.ConvertFromUInt64(6300565133566699913).ToHex(),
                    MosaicSupplyType.Type.INCREASE,
                    true
                );

            var supplyChangePayload = TransactionExtensions.PrepareEmbeddedTransaction<MosaicSupplyChangeTransaction1>(supplyChange, publicAcc);

            Debug.WriteLine(supplyChangePayload.Payload.ToHex());

            Assert.That(supplyChangePayload.Payload.ToHex(), Is.EqualTo("5800000000000000000000000000000000000000000000000000000000000000000000000000000000000000019854419841E5B8E40781CF74DABF592817DE48711D778648DEAFB20000010000000000672B0000CE5600006500000000000000"));


            var transactionFactory = new TransactionFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);

            var aggTx = transactionFactory.CreateAggregateBonded("61E0F8B9AB2FE3E008DCE1380FECDAF5BCFB1851247BF990771154177A0B7E78", payload.Payload.Concat(supplyChangePayload.Payload).ToArray(), new byte[] { }, false);

            var aggPayload = TransactionExtensions.PrepareTransaction<AggregateTransaction1>(aggTx, keys);

            Debug.WriteLine(aggPayload.Payload.ToHex());

            Assert.That(aggPayload.Payload.ToHex(), Is.EqualTo("20020000000000003E080DCE5B32319CA6899808CA664C3961C77A85BB42B192F36394D7B46C79FE4EC2AD6DA50E38836D4ADCDD992C020137F047C1228C351B9533176AB18CE0AFFDDDB26B9750C36F0C0F06914658E6DC86AE0C323ADBB3520D42DD85138616EB0000000002984142E0FEEEEFFEEEEFFEE0711EE7711EE7713F2BE873F569828C88CD0DE37BB31C998FA0AAEB3308A1FFBF3D01CE49E8E9F7A8000000000000006000000000000000000000000000000000000000000000000000000000000000000000000000000000000000019854419841E5B8E40781CF74DABF592817DE48711D778648DEAFB20000010000000000672B0000CE5600006400000000000000410000000000000000000000000000000000000000000000000000000000000000000000000000000000000001984D428869746E9B1A70570A0000000000000001000000000000000000000000000000BD6072E843DF052681FE12FCB825CC873C670BEC51E73F5B460043677D6B1EBB119DB71F2916E710BC2195251D422AF0CB2CB378C2F0F2521907F8102912EA38AD3BED2820F6AEA6656B0D89E5BDA7B2533409864B8A6C961DCA9D173AE399790000000000000000062F6371FD45C2ADB840D85B3E7AFCB22678365733264291705210A1661C6DC8F55D9667E12F30C7CEC0280A51F09F02C26F28E435E1CA1617765FB792C3AAA3350BC8ECD2116B8BDD3FC26E779C2A05DD788F0E59502E92C92DADA6C25C6A90"));
        }
    }
}
