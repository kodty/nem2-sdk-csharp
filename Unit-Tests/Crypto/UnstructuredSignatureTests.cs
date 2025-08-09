using CopperCurve;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Articles;
using io.nem2.sdk.src.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Unit_Tests.Model.Transactions.Verified;

namespace Unit_Tests.Crypto
{
    internal class UnstructuredSignatureTests
    {

        [Test]
        public static void TestKeyPair()
        {
            var text = File.ReadAllLines(HttpSetUp.VectorsPath + "UnstructuredTestSign.txt");

            var keys = JsonObject.Parse(String.Concat(text));

            for (var i = 0; i < 10000; i++)
            {
                string sk = keys[i]["privateKey"].GetValue<string>();
                string pk = keys[i]["publicKey"].GetValue<string>();
                string data = keys[i]["data"].GetValue<string>();
                int len = keys[i]["length"].GetValue<int>();
                string sig = keys[i]["signature"].GetValue<string>();

                var keyPair = SecretKeyPair.CreateFromPrivateKey(sk);

                Assert.That(sk, Is.EqualTo(keyPair.PrivateKeyString));

                var finalSig = keyPair.Sign(data.FromHex());

                Assert.That(sig, Is.EqualTo(finalSig.ToHex()));

                SignedTransaction.VerifySignature(data.FromHex(), sig, pk);              
            }
        }

        [Test, Timeout(20000)]
        public async Task AggregateSignture()
        {
            string payload = "2002000000000000B75CCB8D780D5CEF69C8D0B4F60959DD28537B54EED68588B29483D2871A6D78D988D2684EEF974D04BEDA0BFEE310A9EB4210F65F0552FC79EE1BAAA7E3228E0D60B282D0F1A7630D165972F424CDEA90441D5B14497E1333B7F39592532ADC0000000001984142E0FEEEEFFEEEEFFEE0711EE7711EE77161E0F8B9AB2FE3E008DCE1380FECDAF5BCFB1851247BF990771154177A0B7E78A8000000000000006000000000000000000000000000000000000000000000000000000000000000000000000000000000000000019854419841E5B8E40781CF74DABF592817DE48711D778648DEAFB20000010000000000672B0000CE5600006500000000000000410000000000000000000000000000000000000000000000000000000000000000000000000000000000000001984D428969746E9B1A70570A000000000000000100000000000000000000000000000067FA12789F80766D329C7F687C5C5F889A82F5E9C3E7996AE4FFE48C34299DE7622C0CA6CC2EC0C48776FC24BF34FB7F4912B3718457A44D41A32DFCD3DBCEDD7D2AA65325ED925E86EDEAE6AB6CA54ED8B4C0DD090ED9DB3860D295DA9820ED0000000000000000549676227A2A84F8A555F69892B49A3BE02A3B2C71E031E2E8968EBAB867C491B3895F21837F76DF15B3A6D97FD7BA1DC625011619A5542194EE4220AE34E50C510D942C2C306BC0637ECFC9D9BEFA907819C6477254FBAD11C7A0DDDC71B913";
            
            // tx1 transfer
            string RecipientAddress = "TBA6LOHEA6A465G2X5MSQF66JBYR254GJDPK7MQ";
            ulong mosaicId = 95442763262823;
            ulong amount = 101;

            // tx2 mosaic supply change
            ulong mosaicId1 = 6300565133566699913;
            var action = MosaicSupplyType.Type.INCREASE;
            var delta = 10;

            string signerPublicKey = "0D60B282D0F1A7630D165972F424CDEA90441D5B14497E1333B7F39592532ADC";
            string signature = "B75CCB8D780D5CEF69C8D0B4F60959DD28537B54EED68588B29483D2871A6D78D988D2684EEF974D04BEDA0BFEE310A9EB4210F65F0552FC79EE1BAAA7E3228E";
            ulong fee = 18370164183782063840;
            ulong deadline = 8207562320463688160;
            string txHash = "61E0F8B9AB2FE3E008DCE1380FECDAF5BCFB1851247BF990771154177A0B7E78";

            var transactionFactory = new TransactionTestFactory(NetworkType.Types.TEST_NET, HttpSetUp.TestnetNode, HttpSetUp.Port);
            
           //transactionFactory.CreateTransferTransaction

        }
    }
}
