using CopperCurve;
using Integration_Tests;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

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
    }
}
